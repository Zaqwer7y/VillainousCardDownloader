namespace Villainous
{
    public class StarWarsDownloader : BasicDownloader
    {

        public StarWarsDownloader(bool downloadCards)
            : base(@"https://sw-villainous.fandom.com/api.php?action=parse&page=", "Sector.jpg", downloadCards, 1050, 1)
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
