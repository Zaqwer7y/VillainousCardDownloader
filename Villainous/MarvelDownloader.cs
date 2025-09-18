using HtmlAgilityPack;
using System.Net;
using System.Web;

namespace Villainous
{
    internal class MarvelDownloader : BasicDownloader
    {
        private static readonly string _urlBase = @"https://marvel-villainous-infinite-power.fandom.com/wiki/";
        private static readonly int _defaultCardScale = 525;

        public MarvelDownloader(bool downloadCards)
            : base(_urlBase, "Domain.png", downloadCards, _defaultCardScale, 0.96)
        {
        }

        public void Download()
        {
            Console.WriteLine("Villains or expansions? (v/e)");
            var ans = Console.ReadLine();
            var isVillain = ans.Equals("v", StringComparison.InvariantCultureIgnoreCase);
            if (isVillain)
                DownloadVillains();
            else
                DownloadCommonFate();
        }

        protected override IEnumerable<string> GetSpecialCards(string villain)
        {
            if (villain.Equals("Thanos", StringComparison.InvariantCultureIgnoreCase))
            {
                return new string[] 
                {
                    @$"https://static.wikia.nocookie.net/marvel-villainous-infinite-power/images/a/ad/Mind_Stone_ITEM.png/revision/latest/scale-to-width-down/{_defaultCardScale}?cb=20230725053149",
                    @$"https://static.wikia.nocookie.net/marvel-villainous-infinite-power/images/4/45/Power_Stone_ITEM.png/revision/latest/scale-to-width-down/{_defaultCardScale}?cb=20230725053219",
                    @$"https://static.wikia.nocookie.net/marvel-villainous-infinite-power/images/c/c7/Reality_Stone_ITEM.png/revision/latest/scale-to-width-down/{_defaultCardScale}?cb=20230725053251",
                    @$"https://static.wikia.nocookie.net/marvel-villainous-infinite-power/images/2/2a/Soul_Stone_ITEM.png/revision/latest/scale-to-width-down/{_defaultCardScale}?cb=20230725053321",
                    @$"https://static.wikia.nocookie.net/marvel-villainous-infinite-power/images/3/38/Space_Stone_ITEM.png/revision/latest/scale-to-width-down/{_defaultCardScale}?cb=20230725053351",
                    @$"https://static.wikia.nocookie.net/marvel-villainous-infinite-power/images/6/60/Time_Stone_ITEM.png/revision/latest/scale-to-width-down/{_defaultCardScale}?cb=20230725053418",
                    @$"https://static.wikia.nocookie.net/marvel-villainous-infinite-power/images/8/8a/Mind_Stone_SPECIAL.png/revision/latest/scale-to-width-down/{_defaultCardScale}?cb=20230725053452",
                    @$"https://static.wikia.nocookie.net/marvel-villainous-infinite-power/images/2/2c/Power_Stone_SPECIAL.png/revision/latest/scale-to-width-down/{_defaultCardScale}?cb=20230725053523",
                    @$"https://static.wikia.nocookie.net/marvel-villainous-infinite-power/images/9/9c/Reality_Stone_SPECIAL.png/revision/latest/scale-to-width-down/{_defaultCardScale}?cb=20230725053550",
                    @$"https://static.wikia.nocookie.net/marvel-villainous-infinite-power/images/2/2d/Soul_Stone_SPECIAL.png/revision/latest/scale-to-width-down/{_defaultCardScale}?cb=20230725053616",
                    @$"https://static.wikia.nocookie.net/marvel-villainous-infinite-power/images/e/ec/Space_Stone_SPECIAL.png/revision/latest/scale-to-width-down/{_defaultCardScale}?cb=20230725053650",
                    @$"https://static.wikia.nocookie.net/marvel-villainous-infinite-power/images/4/46/Time_Stone_SPECIAL.png/revision/latest/scale-to-width-down/{_defaultCardScale}?cb=20230725053714",
                };
            }
            return new string[0]; 
        }

        private void DownloadCommonFate()
        {
            bool userContinue = true;
            while (userContinue)
            {
                Console.Write("Choose an expansion: ");
                var expansionStr = Console.ReadLine();
                DownloadCommonFateByExpansion(expansionStr);
                Console.WriteLine("Would you like to continue? (y/n)");
                var ans = Console.ReadLine();
                userContinue = ans.Equals("y", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        private void DownloadCommonFateByExpansion(string expansion)
        {
            var idStr = HttpUtility.HtmlEncode($"{expansion.Replace(' ', '_')}_-_Card_Gallery");
            using (var webClient = new WebClient())
            {
                string source = webClient.DownloadString(_urlBase);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(source);
                var expansionEl = htmlDoc.GetElementbyId(idStr);
                var fateCardsParagraphEl = expansionEl.ParentNode.NextSibling.NextSibling;
                var cardAnchors = fateCardsParagraphEl.ChildNodes.Where(a => a.Attributes.Any(a => a.Name == "href"));
                if (cardAnchors.Any())
                {
                    foreach (var cardAnchor in cardAnchors)
                        DownLoadCardLink(false, cardAnchor, webClient, "Fate " + expansion);
                }
                else
                {
                    if (fateCardsParagraphEl.InnerText.StartsWith("2 Copies", StringComparison.InvariantCultureIgnoreCase))
                    {
                        cardAnchors = fateCardsParagraphEl.NextSibling.ChildNodes.Where(a => a.Attributes.Any(a => a.Name == "href"));
                        foreach (var cardAnchor in cardAnchors)
                        {
                            DownLoadCardLink(false, cardAnchor, webClient, "Fate " + expansion + "\\2");
                        }
                        fateCardsParagraphEl = fateCardsParagraphEl.NextSibling.NextSibling;
                    }
                    if (fateCardsParagraphEl.InnerText.StartsWith("1 Copy", StringComparison.InvariantCultureIgnoreCase))
                    {
                        cardAnchors = fateCardsParagraphEl.NextSibling.ChildNodes.Where(a => a.Attributes.Any(a => a.Name == "href"));
                        foreach (var cardAnchor in cardAnchors)
                            DownLoadCardLink(false, cardAnchor, webClient, "Fate " + expansion);
                    }
                }
            }
        }
    }
}
