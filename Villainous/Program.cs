// See https://aka.ms/new-console-template for more information

namespace Villainous
{
    internal class Program
    {
        private static bool _downloadCards = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Download cards: " + (_downloadCards ? "ON" : "OFF"));

            Console.WriteLine("Disney, Marvel, Star Wars or Anime? (d/m/mh/s/dh/a)");
            var ans = Console.ReadLine();
            var isDisney = ans.Equals("d", StringComparison.InvariantCultureIgnoreCase);
            var isMarvel = ans.Equals("m", StringComparison.InvariantCultureIgnoreCase);
            var isStarWars = ans.Equals("s", StringComparison.InvariantCultureIgnoreCase);
            var isDisneyHomebrew = ans.Equals("dh", StringComparison.InvariantCultureIgnoreCase);
            var isAnime = ans.Equals("a", StringComparison.InvariantCultureIgnoreCase);
            var isMarvelHomebrew = ans.Equals("mh", StringComparison.InvariantCultureIgnoreCase);

            if (isDisney)
            {
                Console.WriteLine("Disney Villainous");
                var disney = new DisneyDownloader(_downloadCards);
                disney.Download();
            }
            else if (isMarvel)
            {
                Console.WriteLine("Marvel Villainous");
                var marvel = new MarvelDownloader(_downloadCards);
                marvel.Download();
            }
            else if (isStarWars)
            {
                Console.WriteLine("Star Wars Villainous");
                var marvel = new StarWarsDownloader(_downloadCards);
                marvel.Download();
            }
            else if (isDisneyHomebrew)
            {
                Console.WriteLine("Disney Villainous Homebrew");
                var marvel = new DisneyHomebrewDownloader(_downloadCards);
                marvel.Download();
            }
            else if (isAnime)
            {
                Console.WriteLine("Anime Villainous");
                var anime = new AnimeDownloader(_downloadCards);
                anime.Download();
            }
            else if (isMarvelHomebrew)
            {
                Console.WriteLine("Marvel Villainous Homebrew");
                var marvel = new MarvelHomebrewDownloader(_downloadCards);
                marvel.Download();
            }
        }
    }
}