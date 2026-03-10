namespace Villainous
{
    public class AnimeDownloader : BasicDownloader
    {
        public AnimeDownloader(bool downloadCards)
            : base(@"https://anime-villainous.fandom.com/api.php?action=parse&page=", "Territory.png", downloadCards, 340, 1)
        {
        }

        protected override IEnumerable<string> GetSpecialCards(string villain)
        {
            if (villain.Equals("Isabella"))
            {
                return new string[]
                {
                    @"https://static.wikia.nocookie.net/anime-villainous/images/2/2d/Day.png/revision/latest/scale-to-width-down/250?cb=20231217062647",
                    @"https://static.wikia.nocookie.net/anime-villainous/images/1/15/Night.png/revision/latest/scale-to-width-down/250?cb=20231217062658",
                    @"https://static.wikia.nocookie.net/anime-villainous/images/3/3e/Isabella_Cardback-fs8.png/revision/latest/scale-to-width-down/225?cb=20231217062655",
                    @"https://static.wikia.nocookie.net/anime-villainous/images/b/b7/Isabella_Fate_Cardback-fs8.png/revision/latest/scale-to-width-down/225?cb=20231217062655"
                };
            }
            if (villain.Equals("Ainz"))
            {
                return new string[]
                {
                    @"https://static.wikia.nocookie.net/anime-villainous/images/e/e6/Ainz_Villain_Cardback.png/revision/latest/scale-to-width-down/225?cb=20241109184857",
                    @"https://static.wikia.nocookie.net/anime-villainous/images/e/e4/Ainz_Fate_Cardback.png/revision/latest/scale-to-width-down/225?cb=20241109184640",
                    @"https://static.wikia.nocookie.net/anime-villainous/images/3/39/Ains_Plan_Cardback.png/revision/latest/scale-to-width-down/225?cb=20241109184857"
                };
            }
            if (villain.Equals("Orochimaru"))
            {
                return new string[]
                {
                    @"https://static.wikia.nocookie.net/anime-villainous/images/4/45/Orochimaru_Villain_Cardback.png/revision/latest/scale-to-width-down/225?cb=20221214114841",
                    @"https://static.wikia.nocookie.net/anime-villainous/images/2/24/Orochimaru_Fate_Cardback.png/revision/latest/scale-to-width-down/225?cb=20221214114833"
                };
            }
            if (villain.Equals("Griffith"))
            {
                return new string[]
                {
                    @"https://static.wikia.nocookie.net/anime-villainous/images/4/42/Griffith_Villain_Cardback.png/revision/latest/scale-to-width-down/225?cb=20241109201535",
                    @"https://static.wikia.nocookie.net/anime-villainous/images/7/7e/Griffith_Fate_Cardback.png/revision/latest/scale-to-width-down/225?cb=20241109201522"
                };
            }
            if (villain.Equals("Gendo"))
            {
                return new string[]
                {
                    @"https://static.wikia.nocookie.net/anime-villainous/images/8/84/Gendo_Villain_Cardback.png/revision/latest/scale-to-width-down/225?cb=20221214114746",
                    @"https://static.wikia.nocookie.net/anime-villainous/images/4/4b/Gendo_Fate_Cardback.png/revision/latest/scale-to-width-down/225?cb=20221214114741",
                    @"https://static.wikia.nocookie.net/anime-villainous/images/9/95/Gendo_Angel_Cardback.png/revision/latest/scale-to-width-down/225?cb=20221214114737"
                };
            }
            return new string[0];
        }
        public async Task Download()
        {
            await DownloadVillains();
        }
    }
}
