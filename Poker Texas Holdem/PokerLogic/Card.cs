using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLogic
{
    internal class Card
    {
        private int cardValue;
        private eCardSuit cardSuit;

        public Card(eCardValue value, eCardSuit suit)
        {
            this.cardValue = (int)value;
            this.cardSuit = suit;
        }

        public int CardValue
        {
            get
            {
                return this.cardValue;
            }
        }

        public eCardSuit CardSuit
        {
            get
            {
                return this.cardSuit;
            }
        }

        public StringBuilder CardToString()
        {
            StringBuilder cardStringBuilder = new StringBuilder();
            if (cardValue <= 10)
            {
                cardStringBuilder.Append(string.Format("{0}", this.CardValue));
            }
            else
            {
                cardStringBuilder.Append(string.Format("{0}", (eCardValue)this.CardValue));
            }

            cardStringBuilder.Append(String.Format(" of {0}", this.cardSuit));

            return cardStringBuilder;
        }

        public bool isEqual(Card.eCardValue value, Card.eCardSuit suit)
        {
            return this.cardValue == (int)value && this.cardSuit == suit;
        }

        public static Card GetCardFromList(List<Card> list, Card.eCardValue value, Card.eCardSuit suit)
        {
            Card cardToReturn = null;
            foreach(Card card in list)
            {
                if(card.CardValue == (int)value && card.CardSuit == suit)
                {
                    cardToReturn = card;
                }
            }

            return cardToReturn;
        }

        public enum eCardValue
        {
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            J = 11,
            Q = 12,
            K = 13,
            A = 14
        }

        public enum eCardSuit
        {
            Hearts = 1,
            Diamonds = 2,
            Spades = 3,
            Clubs = 4
        }
    }
}
