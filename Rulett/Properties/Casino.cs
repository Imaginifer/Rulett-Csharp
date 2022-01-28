using System;
using System.Collections.Generic;
using System.Linq;

namespace Rulett.Properties
{
    public class Casino
    {
        private int tokens;
        private List<Bet> bets;

        public Casino()
        {
            tokens = 0;
            bets = new List<Bet>();
        }

        public int Tokens { get { return tokens; } set { tokens = value; } }

        public List<Bet> Bets { get {return bets; } }

        private int Spin()
        {
            var r = new Random();
            return r.Next(37);
        }

        public bool PlaceBet(BetPlace place, int amount)
        {
            if (tokens < amount)
                return false;
            tokens -= amount;
            bets.Add(new Bet(place, amount));
            return true;
        }

        public bool RemoveBet (int nr)
        {
            if (nr <= 0 || bets.Count < nr)
                return false;
            var b = bets[nr - 1];
            tokens += b.Amount;
            bets.Remove(b);
            return true;
        }

        public string Play ()
        {
            int rolled = Spin();
            string resp = "A nyerő szám: " + rolled;

            Bet[] winners = bets.TakeWhile(b => Board.GetWinningNrs(b.Place)
            .Contains(rolled)).ToArray();
            bets.Clear();
            if (winners.Any())
                return resp + " Gratulálunk nyereményéhez: " + ResolveWinningBets(winners);
            return resp +" Egyetlen tét sem nyert!";
        }

        private int ResolveWinningBets(Bet[] winners)
        {
            int q = 0;
            foreach (Bet b in winners) { q += b.Amount + (int) Math.Floor(Board.Prize(b.Place) * b.Amount); }
            tokens += q;
            return q;
        }

        public string[] ListExtantBets()
        {
            var res = new List<string>();
            foreach(Bet b in bets) { res.Add(b.Place.ToString() + " " + b.Amount); }
            return res.ToArray();
        }
    }
}
