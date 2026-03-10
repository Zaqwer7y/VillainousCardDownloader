namespace Villainous
{
    public class MarvelHomebrewDownloader : BasicDownloader
    {
        public MarvelHomebrewDownloader(bool downloadCards)
            : base(@"https://disney-villainous-homebrew.fandom.com/api.php?action=parse&page=", "Domain.png", downloadCards, 340, 1)
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
