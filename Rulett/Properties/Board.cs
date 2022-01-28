using System;
using System.Collections.Generic;
using System.Linq;

namespace Rulett.Properties
{
    public static class Board
    {
        private static readonly Dictionary<BetPlace, HashSet<int>> board =
        new Dictionary<BetPlace, HashSet<int>>()
        {
            [BetPlace.COLUMN_01] = new HashSet<int>() { 1, 4, 7, 10, 13, 16,
                    19, 22, 25, 28, 31, 34 },
            [BetPlace.COLUMN_02] = new HashSet<int>() { 2, 5, 8, 11, 14, 17,
                    20, 23, 26, 29, 32, 35 },
            [BetPlace.COLUMN_03] = new HashSet<int>() { 3, 6, 9, 12, 15, 18,
                    21, 24, 27, 30, 33, 36},
            [BetPlace.CLR_BLACK] = new HashSet<int>() { 2, 4, 6, 8, 10, 11,
                    13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35 },
            [BetPlace.CLR_RED] = new HashSet<int>() { 1, 3, 5, 7, 9, 12, 14,
                    16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 },
            [BetPlace.FIRST_FOUR] = new HashSet<int>() { 0, 1, 2, 3},
            [BetPlace.TRIO_00_02] = new HashSet<int>() { 0, 1, 2},
            [BetPlace.TRIO_00_03] = new HashSet<int>() { 0, 2, 3}
        };


        static Board()
        {
            var allBets = Enum.GetValues(typeof(BetPlace));
            int[] allNr = new int[36];
            int q = 0;
            while (q < 36) { allNr[q] = ++q; }

            foreach (BetPlace b in allBets) 
            {
                string nm = b.ToString();
                string init = nm.Substring(0, 3);
                if (init.Equals("Sin")) 
                {
                    board.Add(b, new HashSet<int>() { Byte.Parse(nm.Substring(7, 2)) });
                } else if (init.Equals("HSP") || init.Equals("VSP"))
                {
                    board.Add(b, new HashSet<int>() 
                    { 
                        Byte.Parse(nm.Substring(7, 2)), 
                        Byte.Parse(nm.Substring(10, 2)) 
                    });
                } else if (init.Equals("COR"))
                {
                    byte n1 = Byte.Parse(nm.Substring(7, 2));
                    byte n2 = Byte.Parse(nm.Substring(10, 2));
                    board.Add(b, new HashSet<int>() { n1, ++n1, n2, --n2 });
                } else if (init.Equals("STR"))
                {
                    byte n = Byte.Parse(nm.Substring(7, 2));
                    board.Add(b, new HashSet<int>() { n, ++n, ++n });
                } else if (init.Equals("SIX"))
                {
                    byte n = Byte.Parse(nm.Substring(9, 2));
                    var temp = from nr in allNr where nr >= n && nr < (n + 6) select nr;
                    //var temp = allNr.TakeWhile(nr => nr >= n && nr < m);
                    board.Add(b, new HashSet<int>(temp));
                } else if (init.Equals("THI"))
                {
                    byte n1 = Byte.Parse(nm.Substring(6, 2));
                    byte n2 = Byte.Parse(nm.Substring(9, 2));
                    board.Add(b, new HashSet<int>(from nr in allNr where nr >= n1 && nr <= n2 select nr));
                } else if (init.Equals("HAL"))
                {
                    byte n1 = Byte.Parse(nm.Substring(5, 2));
                    byte n2 = Byte.Parse(nm.Substring(8, 2));
                    board.Add(b, new HashSet<int>(from nr in allNr where nr >= n1 && nr <= n2 select nr));
                } else if (b == BetPlace.NUM_EVEN)
                    board.Add(b, new HashSet<int>(from nr in allNr where (nr % 2 == 0) select nr));
                else if (b == BetPlace.NUM_ODDS)
                    board.Add(b, new HashSet<int>(from nr in allNr where (nr % 2 == 1) select nr));

            }
        }

        public static HashSet<int> GetWinningNrs (BetPlace place)
        {
            return board[place];
        }

        public static double Prize (BetPlace place)
        {
            string s = place.ToString().Substring(0, 3);

            if (s.Equals("Sin")) { return 35.0; }

            if (s.Equals("HSP") || s.Equals("VSP")) { return 17.0; }

            if (s.Equals("TRI")) { return 11.6; }

            if (s.Equals("STR")) { return 11.0; }

            if (s.Equals("COR") || s.Equals("FIR")) { return 8.0; }

            if (s.Equals("SIX")) { return 5.0; }

            if (s.Equals("THI") || s.Equals("COL")) { return 2.0; }

            return 1.0;
        }

        public static void WriteOutTable()
        {
            foreach (BetPlace b in board.Keys)
            {
                var x = board[b];
                Console.WriteLine(b.ToString() + ": " + string.Join(", ", x));
            }
        }
    }


}