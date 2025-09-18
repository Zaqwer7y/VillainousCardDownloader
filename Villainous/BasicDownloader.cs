using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

        public void DownloadVillains()
        {
            bool userContinue = true;
            while (userContinue)
            {
                PrepareCards();
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


        protected void PrepareCards()
        {
            Console.Write("Enter a villain: ");
            _currentVillainStr = Console.ReadLine().Replace(' ', '_');
            var sourceLink = DomainBaseStr + _currentVillainStr;
            var cardsDic = new Dictionary<int, List<string>>();
            using (var webClient = new WebClient())
            {
                string source = webClient.DownloadString(sourceLink);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(source);
                var anchors = htmlDoc.DocumentNode.SelectNodes("//a");
                //realm
                var realmAnchor = FindRealm(webClient, anchors);
                DownloadLink(true, null, realmAnchor, BoardName, webClient);
                //card backs
                var cardBacks = FindCardBacks(anchors).ToList();
                for (int i = 0; i < cardBacks.Count; i++)
                    DownloadLink(true, null, cardBacks[i], "CardBack" + i.ToString() + ".png", webClient);
                //guide
                var guides = FindGuides(anchors).ToList();
                for (int i = 0; i < guides.Count; i++)
                    DownloadLink(true, null, guides[i], i.ToString() + ".png", webClient, "Guide");
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
                            var cardPath = DownLoadCardLink(true, cardAnchor, webClient, count.ToString());
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
                    DownLoadCardLink(true, link, null, webClient, "Special");
            }
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

        private HtmlNode FindRealm(WebClient webClient, HtmlNodeCollection anchors)
        {
            for (int i = 0; i < anchors.Count; i++)
            {
                var anchor = anchors[i];
                if (!anchor.ChildAttributes("href").Any())
                    continue;
                var link = anchor.Attributes["href"].Value;
                if (link.Contains(BoardName) && LinkExists(webClient, anchor))
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
                    && LinkExists(webClient, link))
                    return anchor;

            }
            Console.WriteLine("Realm not found");
            return null;
        }

        private bool LinkExists(WebClient webClient, HtmlNode anchor)
        {
            return LinkExists(webClient, anchor.Attributes["href"].Value);
        }

        private bool LinkExists(WebClient webClient, string url)
        {
            try
            {
                using (var stream = webClient.OpenRead(url))
                {
                    return true;
                }
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

        protected string DownLoadCardLink(bool villainSpecific, HtmlNode anchor, WebClient webClient, string subFolder)
        {
            var link = anchor.Attributes["href"].Value;
            if (link.Contains("File:"))
                link = anchor.ChildNodes.First().Attributes["data-src"].Value;
            return DownLoadCardLink(villainSpecific, link, anchor, webClient, subFolder);
        }

        protected string DownLoadCardLink(bool villainSpecific, string link, HtmlNode anchor, WebClient webClient, string subFolder)
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
            return DownloadLink(villainSpecific, linkEdited, anchor, cardName, webClient, subFolder);
        }

        private string DownloadLink(bool villainSpecific, string url, HtmlNode anchor, string fileName, WebClient webClient, string subFolder = null)
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
                    webClient.DownloadFile(url ?? anchor.Attributes["href"].Value, filePath);
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
                            webClient.DownloadFile(imgSrc, filePath);
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
    }
}
