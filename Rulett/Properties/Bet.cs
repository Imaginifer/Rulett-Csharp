using System;
namespace Rulett.Properties
{
    public class Bet
    {
        private readonly BetPlace place;
        private readonly int amount;

        public Bet( BetPlace place, int amount )
        {
            this.place = place;
            this.amount = amount;
        }

        public BetPlace Place { get { return place; } }
        public int Amount { get {return amount; } }
    }
}
