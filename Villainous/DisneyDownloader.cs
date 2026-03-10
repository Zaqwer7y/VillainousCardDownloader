using HtmlAgilityPack;
using System.Net;

namespace Villainous
{
    public class DisneyDownloader : BasicDownloader
    {

        public DisneyDownloader(bool downloadCards)
            : base(@"https://disney-villainous.fandom.com/api.php?action=parse&page=", "realm.jpg", downloadCards, 340, 1)
        {
        }

        public async Task Download()
        {
            await DownloadVillains();
        }

        protected override IEnumerable<string> GetSpecialCards(string villain)
        {
            return new string[0];
        }
    }
}
