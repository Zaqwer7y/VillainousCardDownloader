namespace Villainous
{
    public class StarWarsDownloader : BasicDownloader
    {

        public StarWarsDownloader(bool downloadCards)
            : base(@"https://sw-villainous.fandom.com/wiki/", "Sector.jpg", downloadCards, 1050, 1)
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
