using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Villainous
{
    public abstract class BasicDownloader
    {
        public readonly double DefaultCardScale;
        public readonly double FateCardScale;
        private string _currentVillainStr = null;

        protected string DomainBaseStr = null;
        protected string BoardName = null;
        protected bool DownloadCards = true;

        public BasicDownloader(string domainBaseStr, string boardName, bool downloadCards, double defaultCardScale, double fateCardScale)
        {
            DomainBaseStr = domainBaseStr;
            BoardName = boardName;
            DownloadCards = downloadCards;
            DefaultCardScale = defaultCardScale;
            FateCardScale = fateCardScale;
        }

        protected abstract IEnumerable<string> GetSpecialCards(string villain);

        public async Task DownloadVillains()
        {
            bool userContinue = true;
            while (userContinue)
            {
                await PrepareCards();
                Console.WriteLine("Would you like to continue? (y/n)");
                var ans = Console.ReadLine();
                userContinue = ans.Equals("y", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        protected bool IsSectionDownloadable(string sectionName)
        {
            return sectionName != null &&
                        (sectionName.Contains("copy", StringComparison.InvariantCultureIgnoreCase)
                        || sectionName.Contains("copies", StringComparison.InvariantCultureIgnoreCase)
                        || sectionName.Contains("card", StringComparison.InvariantCultureIgnoreCase));

        }


        protected async Task PrepareCards()
        {
            Console.Write("Enter a villain: ");
            _currentVillainStr = Console.ReadLine().Replace(' ', '_');
            var sourceLink = DomainBaseStr + _currentVillainStr;
            var cardsDic = new Dictionary<int, List<string>>();
            var webClient = GetClient();
            using (webClient)
            {
                string source =  await GetHtmlString(webClient, sourceLink);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(source);
                var anchors = htmlDoc.DocumentNode.SelectNodes("//a");
                //realm
                var realmAnchor = await FindRealm(webClient, anchors);
                await DownloadLink(true, null, realmAnchor, BoardName, webClient);
                //card backs
                var cardBacks = FindCardBacks(anchors).ToList();
                for (int i = 0; i < cardBacks.Count; i++)
                    await DownloadLink(true, null, cardBacks[i], "CardBack" + i.ToString() + ".png", webClient);
                //guide
                var guides = FindGuides(anchors).ToList();
                for (int i = 0; i < guides.Count; i++)
                    await DownloadLink(true, null, guides[i], i.ToString() + ".png", webClient, "Guide");
                var allBoldEls = htmlDoc.DocumentNode.SelectNodes("//b")?.Where(e => e != null).ToList() ?? new List<HtmlNode>();
                if (!allBoldEls.Any())
                    allBoldEls = htmlDoc.DocumentNode.SelectNodes("//li").ToList();
                var boldEls = allBoldEls.Where(boldEl => IsSectionDownloadable(boldEl.InnerText)).ToList();
                for (int i = 0; i < boldEls.Count; i++)
                {
                    var boldEl = boldEls[i];
                    if (!GetCardCount(boldEl, out int count))
                        continue;
                    if (!cardsDic.ContainsKey(count))
                        cardsDic.Add(count, new List<string>());
                    var parent = boldEl.ParentNode;
                    var linksParagraphEl = parent.NextSibling;
                    if (linksParagraphEl.Name == "#text")
                        linksParagraphEl = linksParagraphEl.NextSibling;
                    while (linksParagraphEl != null)
                    {
                        if (linksParagraphEl.NodeType == HtmlNodeType.Text)
                            linksParagraphEl = parent;
                        var cardAnchors = linksParagraphEl.ChildNodes.Where(a => a.ChildAttributes("href").Any());
                        if (!cardAnchors.Any())
                        {
                            cardAnchors = linksParagraphEl.ChildNodes.SelectMany(span => span.ChildNodes)
                                .Where(a => a.ChildAttributes("href").Any());
                        }
                        foreach (var cardAnchor in cardAnchors)
                        {
                            var cardPath = await DownLoadCardLink(true, cardAnchor, webClient, count.ToString());
                            cardsDic[count].Add(cardPath);
                        }
                        if (cardAnchors.Any())
                            linksParagraphEl = linksParagraphEl.NextSibling;
                        else
                            linksParagraphEl = null;
                    }
                }
                //special cards
                foreach (var link in GetSpecialCards(_currentVillainStr))
                    await DownLoadCardLink(true, link, null, webClient, "Special");
            }
        }

        private HttpClient GetClient()
        {
            //var handler = new HttpClientHandler()
            //{
            //    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            //    UseCookies = true,          // retain Cloudflare challenge cookies
            //    CookieContainer = new System.Net.CookieContainer(),
            //    AllowAutoRedirect = true,
            //};

            //var client = new HttpClient(handler);

            //// Order matters — match Chrome's real header order
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Accept",
            //    "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language",
            //    "en-US,en;q=0.9");
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding",
            //    "gzip, deflate, br");
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Connection",
            //    "keep-alive");
            //client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
            //    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Referer",
            //    "https://www.google.com/");
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Dest", "document");
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Mode", "navigate");
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Site", "none");

            return new HttpClient();
        }

        private async Task<string> GetHtmlString(HttpClient client, string link)
        {
            string url = link + "&prop=text&format=json";

            string json = await client.GetStringAsync(url);

            using var doc = JsonDocument.Parse(json);

            string html = doc
                .RootElement
                .GetProperty("parse")
                .GetProperty("text")
                .GetProperty("*")
                .GetString();

            return html;
        }

        public bool GetCardCount(HtmlNode boldEl, out int count)
        {
            var innerText = boldEl.InnerText;
            innerText = innerText.Replace("One", "1");
            innerText = innerText.Replace("Two", "2");
            innerText = innerText.Replace("Three", "3");
            innerText = innerText.Replace("Four", "4");
            innerText = innerText.Replace("Five", "5");
            innerText = innerText.Replace("Six", "6");
            return int.TryParse(innerText.Trim(' ')[0].ToString(), out count);
        }

        private async Task<HtmlNode> FindRealm(HttpClient webClient, HtmlNodeCollection anchors)
        {
            for (int i = 0; i < anchors.Count; i++)
            {
                var anchor = anchors[i];
                if (!anchor.ChildAttributes("href").Any())
                    continue;
                var link = anchor.Attributes["href"].Value;
                if (link.Contains(BoardName) && await LinkExists(webClient, anchor))
                    return anchor;
            }
            //not found? Try again
            for (int i = 0; i < anchors.Count; i++)
            {
                var anchor = anchors[i];
                if (!anchor.ChildAttributes("href").Any())
                    continue;
                var link = anchor.Attributes["href"].Value;
                if (link.Contains("http") &&
                    (link.Contains("realm", StringComparison.InvariantCultureIgnoreCase) ||
                    link.Contains("domain", StringComparison.InvariantCultureIgnoreCase) ||
                    link.Contains("sector", StringComparison.InvariantCultureIgnoreCase) ||
                    link.Contains("territory", StringComparison.InvariantCultureIgnoreCase) ||
                    link.Contains("board", StringComparison.InvariantCultureIgnoreCase))
                    && await LinkExists(webClient, link))
                    return anchor;

            }
            Console.WriteLine("Realm not found");
            return null;
        }

        private async Task<bool> LinkExists(HttpClient webClient, HtmlNode anchor)
        {
            return await LinkExists(webClient, anchor.Attributes["href"].Value);
        }

        private async Task<bool> LinkExists(HttpClient webClient, string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Head, url);
                var response = await webClient.SendAsync(request);

                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private IEnumerable<HtmlNode> FindCardBacks(HtmlNodeCollection anchors)
        {
            for (int i = 0; i < anchors.Count; i++)
            {
                var anchor = anchors[i];
                if (!anchor.ChildAttributes("href").Any())
                    continue;
                var link = anchor.Attributes["href"].Value;
                if (link.Contains("File:"))
                    continue;
                if (link.Contains("cardback", StringComparison.InvariantCultureIgnoreCase) ||
                    link.Contains("card_back", StringComparison.InvariantCultureIgnoreCase) ||
                    link.Contains("villain_back", StringComparison.InvariantCultureIgnoreCase) ||
                    link.Contains("fate_back", StringComparison.InvariantCultureIgnoreCase))
                    yield return anchor;
            }
        }

        private IEnumerable<HtmlNode> FindGuides(HtmlNodeCollection anchors)
        {
            for (int i = 0; i < anchors.Count; i++)
            {
                var anchor = anchors[i];
                if (!anchor.ChildAttributes("href").Any())
                    continue;
                var link = anchor.Attributes["href"].Value;
                if (link.Contains("http") && link.Contains("Guide", StringComparison.InvariantCultureIgnoreCase))
                    yield return anchor;
            }
        }

        protected async Task<string> DownLoadCardLink(bool villainSpecific, HtmlNode anchor, HttpClient webClient, string subFolder)
        {
            var link = anchor.Attributes["href"].Value;
            if (link.Contains("File:"))
                link = anchor.ChildNodes.First().Attributes["data-src"].Value;
            return await DownLoadCardLink(villainSpecific, link, anchor, webClient, subFolder);
        }

        protected async Task<string> DownLoadCardLink(bool villainSpecific, string link, HtmlNode anchor, HttpClient webClient, string subFolder)
        {
            var extensionStr = ".png/";
            if (link.IndexOf(extensionStr) < 0)
                extensionStr = ".jpg/";
            if (link.IndexOf(extensionStr) < 0)
                extensionStr = ".gif/";
            var linkEdited = link.Substring(0, link.IndexOf(extensionStr) + extensionStr.Length - 1);
            var cardName = linkEdited.Substring(linkEdited.LastIndexOf("/") + 1);
            var cardScale = DefaultCardScale;
            if (cardName.StartsWith("Fate", StringComparison.InvariantCultureIgnoreCase) ||
                cardName.StartsWith("Card_Fate", StringComparison.InvariantCultureIgnoreCase))
                cardScale *= FateCardScale;
            var cardScaleInt = (int)Math.Round(cardScale);
            linkEdited += $"/revision/latest/scale-to-width-down/{cardScaleInt}?cb=20190225014443\"";
            return await DownloadLink(villainSpecific, linkEdited, anchor, cardName, webClient, subFolder);
        }

        private async Task<string> DownloadLink(bool villainSpecific, string url, HtmlNode anchor, string fileName, HttpClient webClient, string subFolder = null)
        {
            if (string.IsNullOrEmpty(url) && anchor == null)
                return null;

            var folder = villainSpecific ? $"{_currentVillainStr}\\" : "";
            if (subFolder != null)
                folder += subFolder + "\\";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            var filePath = $"{folder}{fileName}";
            if (!DownloadCards)
                return filePath;
            int retryCount = 0;
            do
            {
                if (retryCount > 0)
                    Console.WriteLine("Retry: " + retryCount);
                try
                {
                    await DownloadFile(webClient, url ?? anchor.Attributes["href"].Value, filePath);
                    Console.WriteLine($"Downloaded {fileName}");
                    break;
                }
                catch (Exception e)
                {
                    var imgNode = anchor?.ChildNodes.FirstOrDefault(n => n.Name.Equals("img"));
                    try
                    {
                        if (imgNode != null)
                        {
                            var imgSrc = imgNode.Attributes["src"].Value;
                            if (!imgSrc.Contains("http"))
                                imgSrc = imgNode.Attributes["data-src"].Value;
                            await DownloadFile(webClient, imgSrc, filePath);
                            Console.WriteLine($"Downloaded {fileName}");
                            break;
                        }
                        else
                            throw e;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Fail to download {fileName}");
                        retryCount++;
                    }
                }
            }
            while (retryCount < 3);
            return filePath;
        }
        private async Task DownloadFile(HttpClient client, string url, string path)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var bytes = await response.Content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync(path, bytes);
        }
    }
}
