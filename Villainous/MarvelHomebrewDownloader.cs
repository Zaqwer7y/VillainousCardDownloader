namespace Villainous
{
    public class MarvelHomebrewDownloader : BasicDownloader
    {
        public MarvelHomebrewDownloader(bool downloadCards)
            : base(@"https://disney-villainous-homebrew.fandom.com/wiki/", "Domain.png", downloadCards, 340, 1)
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
