using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLogic
{
    class OrderedPair
    {
        private Card.eCardValue cardValue;
        private int numOfRepititions;

        public OrderedPair(Card.eCardValue cardValue, int numOfRepititions)
        {
            this.cardValue = cardValue;
            this.numOfRepititions = numOfRepititions;
        }

        public StringBuilder OrderedPairToString()
        {
            StringBuilder orderedPaidToString = new StringBuilder();
            orderedPaidToString.Append(
                String.Format("card value {0} has {1} repititions", this.cardValue, this.numOfRepititions));

            return orderedPaidToString;
        }

        public Card.eCardValue CardValue
        {
            get
            {
                return this.cardValue;
            }
        }

        public int NumOfRepititions
        {
            get
            {
                return this.numOfRepititions;
            }
        }
    }
}
