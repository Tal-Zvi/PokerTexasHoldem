using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PokerLogic
{
    class Program
    {
        public static void Main()
        {
            // note
            HandCalculations calc = new HandCalculations();
            Card card1 = new Card(Card.eCardValue.A, Card.eCardSuit.Spades);
            Card card2 = new Card(Card.eCardValue.Two, Card.eCardSuit.Spades);
            Card card3 = new Card(Card.eCardValue.Three, Card.eCardSuit.Clubs);
            Card card4 = new Card(Card.eCardValue.A, Card.eCardSuit.Hearts);
            //Card card5 = new Card(Card.eCardValue.Nine, Card.eCardSuit.Diamonds);

            Card firstPlayer1 = new Card(Card.eCardValue.A, Card.eCardSuit.Diamonds);
            Card firstPlayer2 = new Card(Card.eCardValue.Nine, Card.eCardSuit.Hearts);

            Card secondPlayer1 = new Card(Card.eCardValue.Q, Card.eCardSuit.Clubs);
            Card secondPlayer2 = new Card(Card.eCardValue.Five, Card.eCardSuit.Spades);
            List<Card> tableCards = new List<Card>();

            tableCards.Add(card1);
            tableCards.Add(card2);
            tableCards.Add(card3);
            tableCards.Add(card4);
            //tableCards.Add(card5);
            List<Card> firstPlayerHand = new List<Card>();
            firstPlayerHand.Add(firstPlayer1);
            firstPlayerHand.Add(firstPlayer2);
            List<Card> secondPlayerHand = new List<Card>();
            secondPlayerHand.Add(secondPlayer1);
            secondPlayerHand.Add(secondPlayer2);
            Player firstPlayer = new Player("Tal");
            firstPlayer.giveCardsToPlayer(firstPlayerHand);
            Player secondPlayer = new Player("Daniel");
            secondPlayer.giveCardsToPlayer(secondPlayerHand);


            Console.WriteLine("Cards on the table are: ");
            Console.WriteLine(calc.printCards(tableCards));
            Console.WriteLine(firstPlayer.PlayerName + "'s cards are: ");
            Console.WriteLine(calc.printCards(firstPlayer.Hand));
            Console.WriteLine(secondPlayer.PlayerName + "'s cards are: ");
            Console.WriteLine(calc.printCards(secondPlayer.Hand));
            Console.WriteLine(firstPlayer.PlayerName + " has:");
            object[] firstPlayerArray = calc.evaluateHand(tableCards, firstPlayer.Hand);
            Console.WriteLine(firstPlayerArray[0]);
            Console.WriteLine(calc.printCards((List<Card>)firstPlayerArray[1]));
            Console.WriteLine(secondPlayer.PlayerName + " has:");
            object[] secondPlayerArray = calc.evaluateHand(tableCards, secondPlayer.Hand);
            Console.WriteLine(secondPlayerArray[0]);
            Console.WriteLine(calc.printCards((List<Card>)secondPlayerArray[1]));

            double[] oddsArray = calc.getChanceOfWinning(firstPlayer, secondPlayer, tableCards);
            Console.WriteLine("first player's odds:  " + oddsArray[0]);
            Console.WriteLine("second player's odds: " + oddsArray[1]);
            Console.WriteLine("Tie odds:             " + oddsArray[2]);
            Console.WriteLine("and the sum is: " + (oddsArray[0] + oddsArray[1] + oddsArray[2]));


            //Player winnerPlayer = calc.GetWinnerPlayer(firstPlayer, secondPlayer, tableCards);
            //// just initializing in a way that will not cause an error
            //HandCalculations.eHandValue winnerHandValue = HandCalculations.eHandValue.HighCard;
            //List<Card> winnerCards = new List<Card>();
            //string nameOfWinner = "";
            //if(winnerPlayer == firstPlayer)
            //{
            //    winnerHandValue = (HandCalculations.eHandValue)firstPlayerArray[0];
            //    winnerCards = (List<Card>)firstPlayerArray[1];
            //    nameOfWinner = firstPlayer.PlayerName + " has";
            //}
            //else if(winnerPlayer == secondPlayer)
            //{
            //    winnerHandValue = (HandCalculations.eHandValue)secondPlayerArray[0];
            //    winnerCards = (List<Card>)secondPlayerArray[1];
            //    nameOfWinner = secondPlayer.PlayerName + " has";
            //}
            //else
            //{
            //    winnerHandValue = (HandCalculations.eHandValue)secondPlayerArray[0];
            //    nameOfWinner = "Both players have";
            //    winnerCards = (List<Card>)secondPlayerArray[1];
            //}
            //
            //Console.WriteLine(nameOfWinner + " won with " + winnerHandValue + ":");
            //Console.WriteLine(calc.printCards(winnerCards));
            Console.ReadLine();
        }
    }
}
