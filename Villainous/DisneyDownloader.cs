using HtmlAgilityPack;
using System.Net;

namespace Villainous
{
    public class DisneyDownloader : BasicDownloader
    {

        public DisneyDownloader(bool downloadCards)
            : base(@"https://disney-villainous.fandom.com/wiki/", "realm.jpg", downloadCards, 340, 1)
        {
        }

        public void Download()
        {
            DownloadVillains();
        }

        protected override IEnumerable<string> GetSpecialCards(string villain)
        {
            return new string[0];
        }
    }
}
