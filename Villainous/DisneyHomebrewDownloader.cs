using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Villainous
{
    public class DisneyHomebrewDownloader : BasicDownloader
    {

        public DisneyHomebrewDownloader(bool downloadCards)
            : base(@"https://disney-villainous-homebrew.fandom.com/wiki/", "realm.jpg", downloadCards, 340, 1)
        {
        }

        public void Download()
        {
            DownloadVillains();
        }

        protected override IEnumerable<string> GetSpecialCards(string villain)
        {
            if (villain.Equals("Bill_Cipher", StringComparison.InvariantCultureIgnoreCase))
            {
                return new string[]
                {
                    @$"https://static.wikia.nocookie.net/disney-villainous-homebrew/images/1/1e/Claymation_Creatures-0.png/revision/latest/scale-to-width-down/{DefaultCardScale}?cb=20220414124344",
                    @$"https://static.wikia.nocookie.net/disney-villainous-homebrew/images/d/d1/Dinosaurs-0.png/revision/latest/scale-to-width-down/{DefaultCardScale}?cb=20220414124344",
                    $@"https://static.wikia.nocookie.net/disney-villainous-homebrew/images/c/ca/Ghosts-0.png/revision/latest/scale-to-width-down/141?cb=20220414124358",
                    $@"https://static.wikia.nocookie.net/disney-villainous-homebrew/images/1/1b/Gnomes-0.png/revision/latest?cb=20220414124408",
                    $@"https://static.wikia.nocookie.net/disney-villainous-homebrew/images/6/63/Living_Video_Game-0.png/revision/latest?cb=20220414124416",
                    $@"https://static.wikia.nocookie.net/disney-villainous-homebrew/images/b/ba/Wax_Figures-0.png/revision/latest?cb=20220414124326",
                    $@"https://static.wikia.nocookie.net/disney-villainous-homebrew/images/b/ba/Undead-0.png/revision/latest?cb=20220414124508",
                    $@"https://static.wikia.nocookie.net/disney-villainous-homebrew/images/9/95/Time_Travel-0.png/revision/latest?cb=20220414124459",
                    $@"https://static.wikia.nocookie.net/disney-villainous-homebrew/images/e/e0/Telekinesis-1.png/revision/latest?cb=20220414124451",
                    $@"https://static.wikia.nocookie.net/disney-villainous-homebrew/images/1/14/Summerween_Trickster-0.png/revision/latest?cb=20220414124442",
                    $@"https://static.wikia.nocookie.net/disney-villainous-homebrew/images/f/f8/Shape_Shifter-0.png/revision/latest?cb=20220414124432",
                    $@"https://static.wikia.nocookie.net/disney-villainous-homebrew/images/c/c4/Possession-1.png/revision/latest?cb=20220414124423"
                };
            }
            else if (villain.Equals("Chef_Skinner", StringComparison.InvariantCultureIgnoreCase))
            {
                return new string[] { "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/b/b6/Cook_Action_2-fs8.png/revision/latest?cb=20220626101615",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/3/39/Linguini.png/revision/latest?cb=20220626102016" };
            }
            else if (villain.Equals("Miss_Finster", StringComparison.InvariantCultureIgnoreCase))
            {
                return new string[]
                {
                    //heroes
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/d/d5/TJ.png/revision/latest/scale-to-width-down/141?cb=20210522221244",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/f/f3/Spinelli_Evidence.png/revision/latest/scale-to-width-down/141?cb=20210522221309",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/3/3f/Vince_Evidence.png/revision/latest/scale-to-width-down/141?cb=20210522221331",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/5/58/Gretchen_Evidence.png/revision/latest?cb=20210522221409",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/a/a3/Mikey_Evidence.png/revision/latest?cb=20210522221349",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/4/40/Gus_Evidence.png/revision/latest?cb=20210522221436",
                    //items
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/9/92/Ball_Stash.png/revision/latest?cb=20230302211113",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/4/49/Da_Finster.png/revision/latest?cb=20230302211122",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/2/2a/Lost_and_Found.png/revision/latest?cb=20230302211128",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/a/a5/Speedy.png/revision/latest?cb=20230302211135",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/1/18/Whistle.png/revision/latest?cb=20230302211142",
                    //locations
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/a/a0/Playground_Evidence.png/revision/latest?cb=20210522221753",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/8/80/Classroom_Evidence.png/revision/latest?cb=20210522221816",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/5/59/Halls_Evidence.png/revision/latest?cb=20210522221833",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/7/77/Detention_Evidence.png/revision/latest?cb=20210522221854"
                };
            }
            else if (villain.Equals("Yubaba", StringComparison.InvariantCultureIgnoreCase))
            {
                return new string[]
                {
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/e/e8/Kasuga-fs8.png/revision/latest",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/d/dd/Kasuga_Card_back-fs8.png/revision/latest",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/1/14/Onama-fs8.png/revision/latest",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/1/15/Onama_Card_back-fs8.png/revision/latest",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/d/d8/Radish-fs8.png/revision/latest",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/7/75/Radish_Card_back1-fs8.png/revision/latest",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/d/d9/Stink_Spirit-fs8.png/revision/latest",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/8/83/Stink_Spirit_Card_back-fs8.png/revision/latest",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/2/27/Otori_Sama-fs8.png/revision/latest",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/e/e2/Otori_Card_back-fs8.png/revision/latest",
                };
            }
            else if (villain.Equals("Hawk_Moth", StringComparison.InvariantCultureIgnoreCase))
            {
                return new string[]
                {
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/9/92/Black_Cat_Ring.png/revision/latest/",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/e/e0/Fox_Necklace.png/revision/latest?cb=20210626060402",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/6/68/Ladybug_Earrings.png/revision/latest?cb=20210626060442",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/c/cf/Bee_Hair_Comb.png/revision/latest?cb=20210626060544",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/5/53/Moth_Brooch.png/revision/latest?cb=20210626060642",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/3/34/Peacock_Brooch.png/revision/latest?cb=20210626060752",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/a/a4/Turtle_Bracelet.png/revision/latest?cb=20210626060955",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/3/39/Linguini.png/revision/latest?cb=20220626102016"
                };
            }
            else if (villain.Equals("Jessie_and_James"))
            {
                return new string[]
                {
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/e/ed/Blastoise1.png/revision/latest?cb=20211213150617",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/f/f3/Chansey1.png/revision/latest?cb=20211213150918",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/9/95/Charizard.png/revision/latest?cb=20211213150950",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/f/f3/Growlithe1.png/revision/latest?cb=20211213151012",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/8/8e/Muk.png/revision/latest?cb=20211213151029",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/b/b2/Onix.png/revision/latest?cb=20211213151044",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/7/7e/Pikachu1.png/revision/latest?cb=20211213151225",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/2/2f/Staryu1.png/revision/latest?cb=20211213151241",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/8/8c/Togepi1.png/revision/latest?cb=20211213151301",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/f/fc/Zubat1.png/revision/latest?cb=20211213151431"
                };
            }
            else if (villain.Equals("Monokuma", StringComparison.InvariantCultureIgnoreCase))
            {
                return new string[]
                {
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/b/bf/Motive-0-.png/revision/latest?cb=20220109105011",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/b/be/Motive-1-.png/revision/latest?cb=20220109105012",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/4/49/Motive-2-.png/revision/latest?cb=20220109105012",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/e/e2/Monokuma_Realm.png/revision/latest?cb=20220109104749"
                };
            }
            else if (villain.Equals("Manfred_von_Karma", StringComparison.InvariantCultureIgnoreCase))
            {
                return new string[]
                {
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/9/99/Argument_Tokens_bACK-fs8.png/revision/latest/scale-to-width-down/1000?cb=20211230143050",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/f/fb/Argument_Tokens_1-fs8.png/revision/latest/scale-to-width-down/1000?cb=20211230143049",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/0/00/Argument_Tokens_2-fs8.png/revision/latest/scale-to-width-down/1000?cb=20211230143049",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/6/62/Argument_Tokens_3-fs8.png/revision/latest/scale-to-width-down/1000?cb=20211230143049",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/7/7f/Argument_Tokens_4-fs8.png/revision/latest/scale-to-width-down/1000?cb=20211230143049",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/c/cd/Argument_Tokens_5-fs8.png/revision/latest/scale-to-width-down/1000?cb=20211230143049",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/c/c8/Argument_Tokens_6-fs8.png/revision/latest/scale-to-width-down/1000?cb=20211230143050",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/8/8d/Argument_Tokens_7-fs8.png/revision/latest/scale-to-width-down/1000?cb=20211230143050",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/2/23/Argument_Tokens_8-fs8.png/revision/latest/scale-to-width-down/1000?cb=20211230143049",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/f/f9/Argument_Tokens_9-fs8.png/revision/latest/scale-to-width-down/1000?cb=20211230143050",
                    "https://static.wikia.nocookie.net/disney-villainous-homebrew/images/6/6e/Argument_Tokens_10-fs8.png/revision/latest/scale-to-width-down/1000?cb=20211230143050"
                };
            }
            return new string[0];
        }
    }
}
