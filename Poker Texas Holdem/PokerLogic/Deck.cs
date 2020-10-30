using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PokerLogic
{
    class Deck
    {
        private List<Card> cardList;
        private int deckSize;

        public Deck()
        {
            this.cardList = new List<Card>();
            this.deckSize = 0;
            foreach (Card.eCardSuit cardSuit in Enum.GetValues(typeof(Card.eCardSuit)))
            {
                foreach(Card.eCardValue cardValue in Enum.GetValues(typeof(Card.eCardValue)))
                {
                    cardList.Add(new Card(cardValue, cardSuit));
                    this.deckSize++;
                }
            }
        }

        public List<Card> CardList
        {
            get
            {
                return this.cardList;
            }
        }

        public int DeckSize
        {
            get
            {
                return this.deckSize;
            }
        }

        private Card getCardFromDeck(Card.eCardValue value, Card.eCardSuit suit)
        {
            Card cardToReturn = null;
            foreach (Card card in this.cardList)
            {
                if (card.isEqual(value, suit))
                {
                    cardToReturn = card;
                    break;
                }
            }

            return cardToReturn;
        }

        public void removeCardFromDeck(Card.eCardValue value, Card.eCardSuit suit)
        {
            Card card = getCardFromDeck(value, suit);
            if (card == null)
            {
                throw new Exception("This card is not in the suit");
            }

            this.cardList.Remove(card);
            this.deckSize--;
        }

        public StringBuilder DeckToString()
        {
            StringBuilder deckStringBuilder = new StringBuilder();
            foreach(Card card in cardList)
            {
                deckStringBuilder.Append(card.CardToString());
                deckStringBuilder.Append(Environment.NewLine);
            }

            deckStringBuilder.Append(String.Format("There are {0} cards in the deck", this.deckSize));
            return deckStringBuilder;
        }
        
        public void shuffleDeck()
        {
            // Shuffles the deck in a completely random order
            this.cardList = this.cardList.OrderBy(a => Guid.NewGuid()).ToList();
        }

        public Card getFirstCardFromDeck()
        {
            //need to make sure not empty
            Card firstCard = this.cardList[0];
            // need to be able to remove the card directly 
            removeCardFromDeck((Card.eCardValue)firstCard.CardValue, firstCard.CardSuit);
            return firstCard;
        }
    }
}
