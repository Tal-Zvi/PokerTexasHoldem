using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLogic
{
    class Table
    {
        private Deck deck;
        //private Card[] fiveCardList;
        private List<Card> listOfRevealedCards;

        public Table()
        {
            this.deck = new Deck();
            this.deck.shuffleDeck();
            //this.fiveCardList = new Card[5];
            listOfRevealedCards = new List<Card>();
        }

        public void openFlop()
        {
            for(int i = 0; i < 3; i++)
            {
                this.listOfRevealedCards.Add(this.deck.getFirstCardFromDeck());
            }
        }

        public List<Card> ListOfRevealedCards
        {
            get
            {
                return this.listOfRevealedCards;
            }
        }

        public Deck Deck
        {
            get
            {
                return this.deck;
            }
        }

        public StringBuilder printTable()
        {
            StringBuilder revealedCardsStringBuilder = new StringBuilder();
            foreach (Card card in this.ListOfRevealedCards)
            {
                revealedCardsStringBuilder.Append(card.CardToString());
                revealedCardsStringBuilder.Append(Environment.NewLine);
            }

            revealedCardsStringBuilder.Append(
                String.Format("number of revealed cards: {0}", this.ListOfRevealedCards.Count));
            return revealedCardsStringBuilder;
        }
    }
}
