using System;
using System.Collections.Generic;
using System.Linq;

namespace Rulett.Properties
{
    public class TextUI
    {
        private Casino c;
        private readonly BetPlace[] bets;

        public TextUI()
        {
            c = new Casino();
            bets = Enum.GetValues(typeof(BetPlace)) as BetPlace[];
        }

        public void RunGame()
        {
            do
            {
                Console.WriteLine("Üdvözöljük a virtuális rulettasztalnál!");
                //Board.WriteOutTable();
                c.Tokens = AskInputNr(int.MaxValue, "a zsetonjainak számát");
                GameLoop();
            } while (!Quit("a játékból"));
        }

        private bool Quit(string place)
        {
            Console.WriteLine("Kilép {0}? I/N", place);
            string s = Console.ReadLine();
            return s.ToLower().Equals("i");
        }

        private int AskInputNr(int limit, string subject)
        {
            Console.WriteLine("Kérjük, írja be {0}:", subject);
            int nr = -1;
            string resp = Console.ReadLine();
            if(int.TryParse(resp, out nr)) { nr = int.Parse(resp); }
            return nr > limit ? -1 : nr;
        }

        private int DisplayMenu(string[] choices)
        {
            int q = 0;
            foreach (string s in choices) { Console.WriteLine("  "+ (q+1) + " - " + choices[q++]); }
            Console.WriteLine("  Más - Kilépés"); 
            return AskInputNr(choices.Length, "a választott számot");
        }

        private void GameLoop()
        {
            bool can = true;
            do
            {
                Console.WriteLine("__________"); Console.WriteLine("");
                Console.WriteLine("Zsetonjainak száma: " + c.Tokens);
                string[] s = { "Tét felhelyezése", "Tétek áttekintése", "Mehet a játék!" };
                switch(DisplayMenu(s))
                {
                    case 1:
                        PlaceBets();
                        break;
                    case 2:
                        ListBets();
                        break;
                    case 3:
                        Console.WriteLine(c.Play());
                        break;
                    default:
                        can = false;
                        break;
                }

            } while (can);
        }

        private void ListBets()
        {
            if(c.Bets.Count() == 0) { Console.WriteLine("Még nincs tétje!"); return; }
            //string[] res = (string[]) from b in c.Bets select b.Place.ToString() + " " + b.Amount;
            Console.WriteLine("A sorszámuk megadásával leveheti tétjeit.");
            int q = DisplayMenu(c.ListExtantBets());
            if(q > 0) { c.RemoveBet(q); }
        }

        private void PlaceBets()
        {
            Console.WriteLine("Mire kíván tétet tenni?");
            string[] str1 = {"Számra", "Vízszintes párra", "Függőleges párra",
                 "Sorra", "Sarokra", "Sorpárra vagy első négyre", "Oszlopra", 
                     "Tucatra", "Magasra-alacsonyra","Páros-páratlanra", "Színre"};
            switch (DisplayMenu(str1))
            {
                case 1:
                    int nr1 = AskInputNr(36, "a kívánt számot!");
                    int bet1 = AskInputNr(c.Tokens, "a kívánt tétet!");
                    if(bet1 <= 0 || nr1 < 0 || 
                    !c.PlaceBet(FindTheEnum("Sin", nr1), bet1))
                        Console.WriteLine("Ilyen tétet nem tehet föl!");
                    break;
                case 2:
                    string[] hspl = {"1-2", "2-3", "4-5", "5-6", "7-8", "8-9", "10-11",
                         "11-12", "13-14", "14-15", "16-17", "17-18", "19-20",
                         "20-21", "22-23", "23-24", "25-26", "26-27", "28-29",
                         "29-30", "31-32", "32-33", "34-35", "35-36"};
                    PlaceTheBet(hspl, "HSP");
                    break;
                case 3:
                    string[] vspl = {"1-4", "2-5", "3-6", "4-7", "5-8", "6-9", "7-10",
                         "8-11", "9-12", "10-13", "11-14", "12-15", "13-14",
                         "15-18", "16-19", "17-20", "18-21", "19-22", "20-23",
                         "21-24", "22-25", "23-26", "24-27", "25-28", "26-29",
                         "27-30", "28-31", "29-32", "30-33", "31-34", "32-35", "33-36"};
                    PlaceTheBet(vspl, "VSP");
                    break;
                case 4:
                    string[] tri = {"1--3", "4--6", "7--9", "10--12",
                         "13--15", "16--18", "19--21", "22--24", "25--27", "28--30",
                         "31--33", "34--36"};
                    PlaceTheBet(tri, "STR");
                    break;
                case 5:
                    string[] four = {"1-5", "2-6", "4-8", "5-9", "7-11",
                         "8-12", "10-14", "11-15", "13-17", "14-18", "16-20", "17-21",
                         "19-23", "20-24", "22-26", "23-27", "25-29", "26-30",
                         "28-32", "29-33", "31-35", "32-36", "0-1-2", "0-2-3"};
                    PlaceTheBet(four, "COR");
                    break;
                case 6:
                    string[] six = {"1--6", "4--9", "7--12", "10--15", "13--18",
                         "16--21", "19--24", "22--27", "25--30", "28--33", "31--36", "0-1-2-3"};
                    PlaceTheBet(six, "SIX");
                    break;
                case 7:
                    string[] col = { "Bal", "Közép", "Jobb" };
                    PlaceTheBet(col, "COL");
                    break;
                case 8:
                    string[] th = { "1--12", "13--24", "25--36" };
                    PlaceTheBet(th, "THI");
                    break;
                case 9:
                    string[] ha = { "1--18", "19--36" };
                    PlaceTheBet(ha, "HAL");
                    break;
                case 10:
                    string[] nu = { "Páratlan", "Páros" };
                    PlaceTheBet(nu, "NUM");
                    break;
                case 11:
                    string[] clr = { "Piros", "Fekete" };
                    PlaceTheBet(clr, "CLR");
                    break;
                default:
                    break;
            }
        }

        private void PlaceTheBet(string[] s, string init)
        {
            int nr = DisplayMenu(s);
            int bet = AskInputNr(c.Tokens, "a kívánt tétet");
            //Console.WriteLine(nr + " és " + bet);
            if (bet <= 0 || nr < 0 || !c.PlaceBet(FindTheEnum(init, nr), bet))
                Console.WriteLine("Ilyen tétet nem tehet föl!");
        }

        private BetPlace FindTheEnum(string init, int nr)
        {
            if (init.Equals("COR") && nr > 22)
                return nr == 23 ? BetPlace.TRIO_00_02 : BetPlace.TRIO_00_03;
            if (init.Equals("SIX") && nr == 12) { return BetPlace.FIRST_FOUR; }

            //var relevant = bets.TakeWhile(b => b.ToString().Substring(0, 3).Equals(init)).ToArray(); //hiba!
            var relevant = new List<BetPlace>();
            foreach (BetPlace b in bets)
            {
                if(b.ToString().Substring(0, 3).Equals(init)) { relevant.Add(b); }
            }
            return relevant[--nr];
        }


    }
}
