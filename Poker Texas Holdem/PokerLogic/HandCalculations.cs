using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerLogic
{
    class HandCalculations
    {
        public HandCalculations()
        {

        }

        // returns the best eHandValue and the Five cards representing this value
        public object[] evaluateHand(List<Card> Revealedcards, List<Card> hand)
        {
            object[] evaluateHandDetails = new object[2];
            // initiating the array
            evaluateHandDetails[0] = eHandValue.HighCard;
            evaluateHandDetails[1] = new List<Card>();
            bool continueSearching = true;
            eHandValue maxHandValue = eHandValue.HighCard;
            List<Card> sevenCardsList = Revealedcards.Concat(hand).ToList();
            List<Card> sortedSevenCardsByCardValue = sevenCardsList.OrderBy(o => o.CardValue).ToList();
            List<Card> sortedSevenCardsByCardSuit = sevenCardsList.OrderBy(o => o.CardSuit).ToList();
            bool isFlush = (bool)this.isFlush(sortedSevenCardsByCardSuit)[0];
            bool isStraight = (bool)this.isStraight(sortedSevenCardsByCardValue)[0];
            List<List<Card>> flushLists = (List<List<Card>>)this.isFlush(sortedSevenCardsByCardSuit)[1];
            List<List<Card>> straightLists = (List<List<Card>>)this.isStraight(sortedSevenCardsByCardValue)[1];
            List<Card> topFiveCards = new List<Card>();
            bool isFlushSuitableForStraightFlush;

            // checks a Straight Flush or RoyalFlush
            if (isFlush && isStraight)
            {
                
                // checks Royal Flush: the lowest card value of the highest Straight/Flush list must be 10
                if (flushLists[flushLists.Count - 1][0].CardValue == straightLists[straightLists.Count - 1][0].CardValue && flushLists[flushLists.Count - 1][0].CardValue == 10)
                {
                    // we detected a Royal Flush. 
                    maxHandValue = eHandValue.RoyalFlush;
                    topFiveCards = flushLists[flushLists.Count - 1];
                    // Max value found. Stop searching here
                    continueSearching = false;
                }

                else
                {
                    // to detect a straight flush, one of the straight cards values must match one of the Flushes card values
                    foreach(List<Card> currentStraightList in straightLists)
                    {
                        foreach (List<Card> currentFlushList in flushLists)
                        {
                            if (currentStraightList[0].CardValue == currentFlushList[0].CardValue && currentStraightList[4].CardValue == currentFlushList[4].CardValue) 
                            {
                                // we detected a Straight Flush. 
                                maxHandValue = eHandValue.StraightFlush;
                                topFiveCards = currentFlushList;
                                // Max value found. Stop searching here
                                continueSearching = false;
                            }
                        }
                    }
                }
            }

            else if(numOfIdenticalNumbersInHand(sortedSevenCardsByCardValue).Count > 0)
            {
                // set maxHandValue to be a pair/TwoPair/ThreeOfaKind/FullHouse/FourOfaKind
                maxHandValue = (eHandValue)getHandValue(numOfIdenticalNumbersInHand(sortedSevenCardsByCardValue))[0];
                if (maxHandValue == eHandValue.FullHouse || maxHandValue == eHandValue.FourOfaKind)
                {
                    //Max value found. Stop searching here
                    continueSearching = false;
                }
            }
        
            //checks flush
            if(continueSearching && isFlush)
            {
                maxHandValue = eHandValue.Flush;
                topFiveCards = flushLists[flushLists.Count - 1];
                continueSearching = false;
            }
            
            //checks straight
            if(continueSearching && isStraight)
            {
                maxHandValue = eHandValue.Straight;
                topFiveCards = straightLists[straightLists.Count - 1];
                continueSearching = false;
            }

            

            if(topFiveCards.Count == 0)
            {
                // topFiveCards has not been populated yet

                if(maxHandValue == eHandValue.HighCard)
                {
                    // take highest 5 cards
                    topFiveCards = sortedSevenCardsByCardValue.GetRange(sortedSevenCardsByCardValue.Count - 5, 5);
                }
                else
                {
                    topFiveCards = getTopFiveCards(sortedSevenCardsByCardValue, (List<OrderedPair>)getHandValue(numOfIdenticalNumbersInHand(sortedSevenCardsByCardValue))[1]);
                }
            }

            //remove
            //foreach (OrderedPair op in ((List<OrderedPair>)getHandValue(numOfIdenticalNumbersInHand(sortedSevenCardsByCardValue))[1]))
            //{
            //    Console.WriteLine(op.OrderedPairToString());
            //}
            evaluateHandDetails[0] = maxHandValue;
            evaluateHandDetails[1] = topFiveCards;
            return evaluateHandDetails;
        }
        //change to private
        //gets a list of cards in increasing order according to cardValue
        public object[] isStraight(List<Card> allRevealedCards)
        {
            // isStraightInCards[0] == true/false. isStraightInCards[1] represents a list of all different Straights, in increasing order
            object[] isStraightInCards = new object[2];
            isStraightInCards[0] = new bool();
            isStraightInCards[1] = new List<List<Card>>();
            List<List<Card>> listOfStraights = new List<List<Card>>();
            List<Card> tempList = allRevealedCards.ToList();

            int cardValue = tempList[0].CardValue;
            bool firstTry = false;
            bool secondTry = false;
            bool thirdTry = false;
            int tempIndex = 0;
            bool isStraightStartsWithA = false;
            List<Card> straightStartsWithA = new List<Card>();

            // remove duplicates (of card values)
            foreach (Card tempCardFromList in tempList.ToList())
            {
                if(tempIndex == 0)
                {
                    // skip the first card
                    tempIndex++;
                    continue;
                }

                if(tempCardFromList.CardValue == cardValue)
                {
                    
                    tempList.Remove(tempCardFromList);
                }
                else
                {
                    cardValue = tempCardFromList.CardValue;
                }
            }
            
            // remove last card in list in case the last 2 cards share the same cardValue
            if (tempList[tempList.Count - 1].CardValue == tempList[tempList.Count - 2].CardValue)
            {
                tempList.Remove(tempList[tempList.Count - 1]);
            }

            // check if there is a straight starting with an A
            if((Card.eCardValue)tempList[0].CardValue == Card.eCardValue.Two
               && tempList[0].CardValue + 1 == tempList[1].CardValue
               && tempList[1].CardValue + 1 == tempList[2].CardValue
               && tempList[2].CardValue + 1 == tempList[3].CardValue)
            {
                if((Card.eCardValue)tempList[4].CardValue == Card.eCardValue.A)
                {
                    isStraightStartsWithA = true;
                    straightStartsWithA.Add(tempList[4]);
                }
                else if(tempList.Count >= 6 && (Card.eCardValue)tempList[5].CardValue == Card.eCardValue.A)
                {
                    isStraightStartsWithA = true;
                    straightStartsWithA.Add(tempList[5]);
                    
                }
                else if(tempList.Count == 7 && (Card.eCardValue)tempList[6].CardValue == Card.eCardValue.A)
                {
                    isStraightStartsWithA = true;
                    straightStartsWithA.Add(tempList[6]);
                }

                // concatenate the first 4 cards (2,3,4,5) to the list. So, it becomes: A,2,3,4,5
                straightStartsWithA = straightStartsWithA.Concat((List<Card>)tempList.GetRange(0, 4)).ToList();
            }

            if(tempList.Count >= 5)
            {
                firstTry = tempList[0].CardValue + 1 == tempList[1].CardValue
                           && tempList[1].CardValue + 1 == tempList[2].CardValue
                           && tempList[2].CardValue + 1 == tempList[3].CardValue
                           && tempList[3].CardValue + 1 == tempList[4].CardValue;
            }
            
            if(tempList.Count >= 6)
            {
                secondTry = tempList[1].CardValue + 1 == tempList[2].CardValue
                            && tempList[2].CardValue + 1 == tempList[3].CardValue
                            && tempList[3].CardValue + 1 == tempList[4].CardValue
                            && tempList[4].CardValue + 1 == tempList[5].CardValue;
            }

            if(tempList.Count == 7)
            {
                thirdTry = tempList[2].CardValue + 1 == tempList[3].CardValue
                           && tempList[3].CardValue + 1 == tempList[4].CardValue
                           && tempList[4].CardValue + 1 == tempList[5].CardValue
                           && tempList[5].CardValue + 1 == tempList[6].CardValue;
            }

            // set isStraightInCards[1] to hold all the different Straights in increasing order (if exists)
            if (isStraightStartsWithA || firstTry || secondTry || thirdTry)
            {
                isStraightInCards[0] = true;

                if(isStraightStartsWithA)
                {
                    // if there is a straight starting with an A, add it to be the first straight in the straights list. It is lowest-value straight
                    listOfStraights.Add(straightStartsWithA);
                }

                if(firstTry)
                {
                    listOfStraights.Add((List<Card>)tempList.GetRange(0, 5));
                }

                if(secondTry)
                {
                    listOfStraights.Add((List<Card>)tempList.GetRange(1, 5));
                }

                if(thirdTry)
                {
                    listOfStraights.Add((List<Card>)tempList.GetRange(2, 5));
                }
                
                isStraightInCards[1] = listOfStraights;
            }
            
            return isStraightInCards;
        }

        //change to private
        //received 7-cards in suit order
        public object[] isFlush(List<Card> allRevealedCards)
        {
            // isFlushInCards[0] == true/false. isFlushInCards[1] represents a list of all different Flushes, in increasing order.

            object[] isFlushInCards = new object[2];
            isFlushInCards[0] = new bool();
            isFlushInCards[1] = new List<List<Card>>();
            List<List<Card>> listOfFlushes = new List<List<Card>>();
            bool firstTry = false;
            bool secondTry = false;
            bool thirdTry = false;
            List<Card> tempList = allRevealedCards.ToList();
            int numOfRepititions = 1;
            Card.eCardSuit suitOfFlush = allRevealedCards[0].CardSuit;
            for(int i = 1; i < tempList.Count; i++)
            {
                if(tempList[i].CardSuit == suitOfFlush)
                {
                    numOfRepititions++;
                    if(numOfRepititions == 5)
                    {
                        //suit of flush found here
                        break;
                    }
                }
                else
                {
                    numOfRepititions = 1;
                    suitOfFlush = tempList[i].CardSuit;
                }
            }
            
            if(numOfRepititions >= 5)
            {
                //there is a Flush, we now detect the highest Flush
                isFlushInCards[0] = true;

                //remove all cards which their suit is not suitOfFlush
                foreach(Card card in tempList.ToList())
                {
                    if(card.CardSuit != suitOfFlush)
                    {
                        tempList.Remove(card);
                    }
                }
                // reorder the list of unique suit in increasing order
                tempList = tempList.OrderBy(o => o.CardValue).ToList();

                // check if there is a flush starting with an A
                if(tempList[0].CardSuit == tempList[1].CardSuit && tempList[1].CardSuit == tempList[2].CardSuit
                                                                && tempList[2].CardSuit == tempList[3].CardSuit)
                {
                    if(tempList[0].CardSuit == tempList[4].CardSuit && (Card.eCardValue)tempList[4].CardValue == Card.eCardValue.A)
                    {
                        List<Card> flushListToAdd = new List<Card>();
                        flushListToAdd.Add(tempList[4]);
                        flushListToAdd = flushListToAdd.Concat((List<Card>)tempList.GetRange(0, 4)).ToList();
                        listOfFlushes.Add(flushListToAdd);
                    }
                    else if(tempList.Count >= 6 && tempList[0].CardSuit == tempList[5].CardSuit && (Card.eCardValue)tempList[5].CardValue == Card.eCardValue.A)
                    {
                        List<Card> flushListToAdd = new List<Card>();
                        flushListToAdd.Add(tempList[5]);
                        flushListToAdd = flushListToAdd.Concat((List<Card>)tempList.GetRange(0, 4)).ToList();
                        listOfFlushes.Add(flushListToAdd);
                    }
                    else if(tempList.Count == 7 && tempList[0].CardSuit == tempList[6].CardSuit && (Card.eCardValue)tempList[6].CardValue == Card.eCardValue.A)
                    {
                        List<Card> flushListToAdd = new List<Card>();
                        flushListToAdd.Add(tempList[6]);
                        flushListToAdd = flushListToAdd.Concat((List<Card>)tempList.GetRange(0, 4)).ToList();
                        listOfFlushes.Add(flushListToAdd);
                    }
                }


                firstTry = tempList[0].CardSuit == tempList[1].CardSuit
                           && tempList[1].CardSuit == tempList[2].CardSuit
                           && tempList[2].CardSuit == tempList[3].CardSuit
                           && tempList[3].CardSuit == tempList[4].CardSuit;
                if(tempList.Count >= 6)
                {
                    secondTry = tempList[1].CardSuit == tempList[2].CardSuit
                                && tempList[2].CardSuit == tempList[3].CardSuit
                                && tempList[3].CardSuit == tempList[4].CardSuit
                                && tempList[4].CardSuit == tempList[5].CardSuit;
                }

                if(tempList.Count == 7)
                {
                    thirdTry = tempList[2].CardSuit == tempList[3].CardSuit
                               && tempList[3].CardSuit == tempList[4].CardSuit
                               && tempList[4].CardSuit == tempList[5].CardSuit
                               && tempList[5].CardSuit == tempList[6].CardSuit;
                }

                
                if (firstTry)
                {
                    listOfFlushes.Add((List<Card>)tempList.GetRange(0, 5));
                }
                
                if (secondTry)
                {
                    listOfFlushes.Add((List<Card>)tempList.GetRange(1, 5));
                }
                
                if (thirdTry)
                {
                    listOfFlushes.Add((List<Card>)tempList.GetRange(2, 5));
                }

                isFlushInCards[1] = listOfFlushes;
            }

            return isFlushInCards;
        }

        // change to private
        // this method detects a pair, two-pair, three of a kind, Full House or four of a kind 
        // gets a list of cards in increasing order according to cardValue
        public List<OrderedPair> numOfIdenticalNumbersInHand(List<Card> allRevealedCards)
        {
            int numOfCards = 0;
            List<Card> tempSevenCardsList = allRevealedCards.ToList();
            List<OrderedPair> resultList = new List<OrderedPair>();
            int currentRepititions = 1;
            int numOfCardsChecked = 1;
            Card prevCard = tempSevenCardsList[0];
            int cardValueToMatch = prevCard.CardValue;
            foreach(Card c in tempSevenCardsList)
            {
                numOfCards++;
            }

            tempSevenCardsList.Remove(prevCard);
            foreach (Card card in tempSevenCardsList)
            {
                Card currentCard = card;
                numOfCardsChecked++;

                if (currentCard.CardValue == cardValueToMatch)
                {
                    currentRepititions++;
                    if(currentRepititions == 4)
                    {
                        resultList.Add(new OrderedPair((Card.eCardValue)currentCard.CardValue, 4));
                        currentRepititions = 1;
                        continue;
                    }

                    //In case that this is the last card to check from the seven cards:
                    if(numOfCardsChecked == numOfCards)
                    {
                        if(currentRepititions == 3)
                        {
                            resultList.Add(new OrderedPair((Card.eCardValue)prevCard.CardValue, 3));
                        }
                        else if(currentRepititions == 2)
                        {
                            resultList.Add(new OrderedPair((Card.eCardValue)prevCard.CardValue, 2));
                        }
                    }
                }
                else
                {
                    if(currentRepititions == 3)
                    {
                        resultList.Add(new OrderedPair((Card.eCardValue)prevCard.CardValue, 3));
                    }

                    else if (currentRepititions == 2)
                    {
                        resultList.Add(new OrderedPair((Card.eCardValue)prevCard.CardValue, 2));
                    }
                    currentRepititions = 1;
                    cardValueToMatch = currentCard.CardValue;
                }

                prevCard = currentCard;
            }
        
            return resultList;
        }

        // gets all revealed cards (5-7 cards)
        // returns Pair/TwoPair/ThreeOfaKind/FullHouse/FourOfaKind
        private object[] getHandValue(List<OrderedPair> list)
        {
            eHandValue handVal = eHandValue.Pair;
            List<OrderedPair> listOfBestCards = new List<OrderedPair>();
            // the method returns handValue array
            object[] handValue = new object[2];
            handValue[0] = handVal;
            handValue[1] = new List<OrderedPair>();
            List<OrderedPair> tempList = list.OrderBy(o => o.NumOfRepititions).ToList();

            if (tempList.Count == 1)
            {
                // it has to be a Pair/ThreeOfaKind/FourOfaKind
                if (tempList[0].NumOfRepititions == 2)
                {
                    handVal = eHandValue.Pair;
                }

                else if (tempList[0].NumOfRepititions == 3)
                {
                    handVal = eHandValue.ThreeOfaKind;
                }
                else if (tempList[0].NumOfRepititions == 4)
                {
                    handVal = eHandValue.FourOfaKind;
                }

                // listOfBestCards must include these cards
                listOfBestCards.Add(tempList[0]);
            }

            else if(tempList.Count == 2)
            {
                if(tempList[1].NumOfRepititions == 2)
                {
                    //has to be 2-Pair in this case
                    handVal = eHandValue.TwoPair;

                    // listOfBestCards must include these cards
                    listOfBestCards.Add(tempList[0]);
                    listOfBestCards.Add(tempList[1]);
                }
                else if(tempList[0].NumOfRepititions == 2 && tempList[1].NumOfRepititions == 3)
                {
                    //has to be a full house
                    handVal = eHandValue.FullHouse;

                    // listOfBestCards must include these cards
                    listOfBestCards.Add(tempList[0]);
                    listOfBestCards.Add(tempList[1]);
                }
                else if(tempList[0].NumOfRepititions == 2 && tempList[1].NumOfRepititions == 4)
                {
                    // has to be a Four Of A Kind
                    handVal = eHandValue.FourOfaKind;

                    // listOfBestCards must include the fourOfaKind, and one card of the Pair
                    OrderedPair tempPair = new OrderedPair(tempList[0].CardValue, 1);
                    listOfBestCards.Add(tempPair);
                    listOfBestCards.Add(tempList[1]);
                }
                else if(tempList[0].NumOfRepititions == 3 && tempList[1].NumOfRepititions == 3)
                {
                    // has to be a full house
                    handVal = eHandValue.FullHouse;

                    // listOfBestCards must include the higher threeOfaKind, and two cards of the lower threeOfaKind
                    OrderedPair tempPair = new OrderedPair(tempList[0].CardValue, 2);
                    listOfBestCards.Add(tempPair);
                    listOfBestCards.Add(tempList[1]);
                }
                else if(tempList[0].NumOfRepititions == 3 && tempList[1].NumOfRepititions == 4)
                {
                    // has to be a Four Of A Kind
                    handVal = eHandValue.FourOfaKind;

                    // listOfBestCards must include the fourOfaKind, and one card of the threeOfaKind
                    OrderedPair tempPair = new OrderedPair(tempList[0].CardValue, 1);
                    listOfBestCards.Add(tempPair);
                    listOfBestCards.Add(tempList[1]);
                }

                
            }
            else
            {
                // in this case, len of list must be 3. Therefore, it is representing either a Two Pair or a Full House
                if(tempList[2].NumOfRepititions == 3)
                {
                    // has to be a full house
                    handVal = eHandValue.FullHouse;

                    // listOfBestCards must include the ThreeOfaKind and the higher pair:
                    listOfBestCards.Add(tempList[1]);
                    listOfBestCards.Add(tempList[2]);
                }
                else
                {
                    // has to be a Two Pair
                    handVal = eHandValue.TwoPair;

                    // listOfBestCards must include the ThreeOfaKind and the 2 highest pairs:
                    listOfBestCards.Add(tempList[1]);
                    listOfBestCards.Add(tempList[2]);
                }
            }

            handValue[0] = handVal;
            handValue[1] = listOfBestCards;
            return handValue;
        }

        private List<Card> getTopFiveCards(List<Card> allRevealedCardsSortedByValue, List<OrderedPair> listOfBestCards)
        {
            List<Card> listToReturn = new List<Card>();
            List<Card> allRevealedCardsSortedByValueCopy = allRevealedCardsSortedByValue.ToList();
            List<OrderedPair> listOfBestCardsCopy = listOfBestCards.ToList();
            int numOfCardsInListToReturn = 0;
            List<Card.eCardValue> cardValuesOfBestCards = new List<Card.eCardValue>();
            bool isBreak = false;
            foreach(OrderedPair tempPair in listOfBestCardsCopy)
            {
                cardValuesOfBestCards.Add(tempPair.CardValue);
            }

            // remove !isBreak
            for(int i = allRevealedCardsSortedByValueCopy.Count - 1; i >= 0 && !isBreak; i--)
            {
                if (allRevealedCardsSortedByValue[i].CardValue == (int)cardValuesOfBestCards[cardValuesOfBestCards.Count - 1])
                {
                    listToReturn.Add(allRevealedCardsSortedByValue[i]);
                    allRevealedCardsSortedByValueCopy.Remove(allRevealedCardsSortedByValue[i]);
                    numOfCardsInListToReturn++;
                }
            }
           
            cardValuesOfBestCards.Remove(cardValuesOfBestCards[cardValuesOfBestCards.Count - 1]);
            if (cardValuesOfBestCards.Count > 0 && listToReturn.Count < 4)
            {
                for (int i = allRevealedCardsSortedByValueCopy.Count - 1; i >= 0 && !isBreak; i--)
                {
                    if (allRevealedCardsSortedByValueCopy[i].CardValue == (int)cardValuesOfBestCards[cardValuesOfBestCards.Count - 1])
                    {
                        listToReturn.Add(Card.GetCardFromList(allRevealedCardsSortedByValue, (Card.eCardValue)allRevealedCardsSortedByValueCopy[i].CardValue, allRevealedCardsSortedByValueCopy[i].CardSuit));
                        allRevealedCardsSortedByValueCopy.Remove(allRevealedCardsSortedByValueCopy[i]);
                        numOfCardsInListToReturn++;
                        if (numOfCardsInListToReturn == 5)
                        {
                            isBreak = true;
                        }
                    }
                }
            }
    
            while (numOfCardsInListToReturn < 5)
            {
                listToReturn.Add(allRevealedCardsSortedByValueCopy[allRevealedCardsSortedByValueCopy.Count - 1]);
                allRevealedCardsSortedByValueCopy.Remove(
                    allRevealedCardsSortedByValueCopy[allRevealedCardsSortedByValueCopy.Count - 1]);
                numOfCardsInListToReturn++;
            }

            return listToReturn;
        }

        public StringBuilder printCards(List<Card> listOfCards)
        {
            StringBuilder sb = new StringBuilder();
            foreach(Card card in listOfCards)
            {
                sb.Append(card.CardToString());
                sb.Append(Environment.NewLine);
            }

            return sb;
        }

        public enum eHandValue
        {
            HighCard = 1,
            Pair = 2,
            TwoPair = 3,
            ThreeOfaKind = 4,
            Straight = 5,
            Flush = 6,
            FullHouse = 7,
            FourOfaKind = 8,
            StraightFlush = 9,
            RoyalFlush = 10
        }

        public Player GetWinnerPlayer(Player firstPlayer, Player secondPlayer, List<Card> fiveCardsOnTable)
        {
            object[] firstPlayerArray = evaluateHand(fiveCardsOnTable, firstPlayer.Hand);
            object[] secondPlayerArray = evaluateHand(fiveCardsOnTable, secondPlayer.Hand);
            eHandValue firstPlayerHandValue = (eHandValue)firstPlayerArray[0];
            eHandValue secondPlayerHandValue = (eHandValue)secondPlayerArray[0];
            List<Card> firstPlayerTopCards = (List<Card>)firstPlayerArray[1];
            List<Card> secondPlayerTopCards = (List<Card>)secondPlayerArray[1];
            Player winnerPlayer;

            if(firstPlayerHandValue > secondPlayerHandValue)
            {
                winnerPlayer = firstPlayer;
            }
            else if(secondPlayerHandValue > firstPlayerHandValue)
            {
                winnerPlayer = secondPlayer;
            }
            else
            {
                winnerPlayer = getWinnerFromTie(firstPlayer, secondPlayer, firstPlayerTopCards, secondPlayerTopCards, firstPlayerHandValue);
            }

            // returns null when it's a tie
            return winnerPlayer;
        }

        // a method that extracts the winner in case that both have the same eHandValue in hand
        private Player getWinnerFromTie(Player firstPlayer, Player secondPlayer, List<Card> firstPlayerTopCards, List<Card> secondPlayerTopCards, eHandValue handValue)
        {
            Player winnerPlayer = null;
            switch(handValue)
            {
                case eHandValue.HighCard:
                    winnerPlayer = getWinnerFromHighCard(firstPlayer, secondPlayer, firstPlayerTopCards, secondPlayerTopCards);
                    break;
                case eHandValue.Pair:
                    winnerPlayer = getWinnerFromPair(firstPlayer, secondPlayer, firstPlayerTopCards, secondPlayerTopCards);
                    break;
                case eHandValue.TwoPair:
                    winnerPlayer = getWinnerFromTwoPair(firstPlayer, secondPlayer, firstPlayerTopCards, secondPlayerTopCards);
                    break;
                case eHandValue.ThreeOfaKind:
                    winnerPlayer = getWinnerFromThreeOfaKind(firstPlayer, secondPlayer, firstPlayerTopCards, secondPlayerTopCards);
                    break;
                case eHandValue.Straight:
                    winnerPlayer = getWinnerFromStraight(firstPlayer, secondPlayer, firstPlayerTopCards, secondPlayerTopCards);
                    break;
                case eHandValue.Flush:
                    winnerPlayer = getWinnerFromFlush(firstPlayer, secondPlayer, firstPlayerTopCards, secondPlayerTopCards);
                    break;
                case eHandValue.FullHouse:
                    winnerPlayer = getWinnerFromFullHouse(firstPlayer, secondPlayer, firstPlayerTopCards, secondPlayerTopCards);
                    break;
                case eHandValue.FourOfaKind:
                    winnerPlayer = getWinnerFromFourOfaKind(firstPlayer, secondPlayer, firstPlayerTopCards, secondPlayerTopCards);
                    break;
                case eHandValue.StraightFlush:
                    winnerPlayer = getWinnerFromStraightFlush(firstPlayer, secondPlayer, firstPlayerTopCards, secondPlayerTopCards);
                    break;
                case eHandValue.RoyalFlush:
                    // in this case we have a tie
                    winnerPlayer = null;
                    break;
                default:
                    break;
            }

            return winnerPlayer;
        }

        private Player getWinnerFromHighCard(Player firstPlayer, Player secondPlayer, List<Card> firstPlayerTopCards, List<Card> secondPlayerTopCards)
        {
            
            Player winnerPlayer = null;

            for(int i = 4; i >= 0; i--)
            {
                if(firstPlayerTopCards[i].CardValue > secondPlayerTopCards[i].CardValue)
                {
                    winnerPlayer = firstPlayer;
                    break;
                }
                else if(firstPlayerTopCards[i].CardValue < secondPlayerTopCards[i].CardValue)
                {
                    winnerPlayer = secondPlayer;
                    break;
                }
            }

            return winnerPlayer;
        }

        private Player getWinnerFromTwoPair(Player firstPlayer, Player secondPlayer, List<Card> firstPlayerTopCards, List<Card> secondPlayerTopCards)
        {
            Player winnerPlayer = null;

            // Compare the higher pair
            if(firstPlayerTopCards[0].CardValue > secondPlayerTopCards[0].CardValue)
            {
                winnerPlayer = firstPlayer;
            }
            else if(firstPlayerTopCards[0].CardValue < secondPlayerTopCards[0].CardValue)
            {
                winnerPlayer = secondPlayer;
            }
            else
            {
                // Both players have the same high pair. Now, compare the lower pair
                if(firstPlayerTopCards[2].CardValue > secondPlayerTopCards[2].CardValue)
                {
                    winnerPlayer = firstPlayer;
                }
                else if (firstPlayerTopCards[2].CardValue < secondPlayerTopCards[2].CardValue)
                {
                    winnerPlayer = secondPlayer;
                }
                else
                {
                    // Both players have the same 2-pair. Compare the high card
                    if(firstPlayerTopCards[4].CardValue > secondPlayerTopCards[4].CardValue)
                    {
                        winnerPlayer = firstPlayer;
                    }
                    else if(firstPlayerTopCards[4].CardValue < secondPlayerTopCards[4].CardValue)
                    {
                        winnerPlayer = secondPlayer;
                    }
                }
            }

            // winnerPlayer remains null if both players have the same hand value
            return winnerPlayer;
        }

        private Player getWinnerFromPair(Player firstPlayer, Player secondPlayer, List<Card> firstPlayerTopCards, List<Card> secondPlayerTopCards)
        {
            Player winnerPlayer = null;

            for (int i = 0; i <= 4 ; i++)
            {
                if (firstPlayerTopCards[i].CardValue > secondPlayerTopCards[i].CardValue)
                {
                    winnerPlayer = firstPlayer;
                    break;
                }
                else if (firstPlayerTopCards[i].CardValue < secondPlayerTopCards[i].CardValue)
                {
                    winnerPlayer = secondPlayer;
                    break;
                }

                // Checking i = 1 is redundant since it is the same pair as i = 0
                if (i == 0)
                {
                    i++;
                }
            }

            return winnerPlayer;
        }

        private Player getWinnerFromThreeOfaKind(Player firstPlayer, Player secondPlayer, List<Card> firstPlayerTopCards, List<Card> secondPlayerTopCards)
        {
            Player winnerPlayer = null;

            for (int i = 0; i <= 4; i++)
            {
                if (firstPlayerTopCards[i].CardValue > secondPlayerTopCards[i].CardValue)
                {
                    winnerPlayer = firstPlayer;
                    break;
                }
                else if (firstPlayerTopCards[i].CardValue < secondPlayerTopCards[i].CardValue)
                {
                    winnerPlayer = secondPlayer;
                    break;
                }

                // Checking i = 1 or i = 2 is redundant since it is the same ThreeOfaKind as i = 0
                if (i == 0)
                {
                    i += 2;
                }
            }

            return winnerPlayer;
        }

        private Player getWinnerFromStraight(Player firstPlayer, Player secondPlayer, List<Card> firstPlayerTopCards, List<Card> secondPlayerTopCards)
        {
            Player winnerPlayer = null;

            // compare only the lower card value in the straight to detect winner
            if(firstPlayerTopCards[0].CardValue > secondPlayerTopCards[0].CardValue)
            {
                winnerPlayer = firstPlayer;
            }
            else if(firstPlayerTopCards[0].CardValue < secondPlayerTopCards[0].CardValue)
            {
                winnerPlayer = secondPlayer;
            }

            return winnerPlayer;
        }

        private Player getWinnerFromFlush(Player firstPlayer, Player secondPlayer, List<Card> firstPlayerTopCards, List<Card> secondPlayerTopCards)
        {
            Player winnerPlayer = null;
            
            for (int i = 4; i >= 0; i--)
            {
                if (firstPlayerTopCards[i].CardValue > secondPlayerTopCards[i].CardValue)
                {
                    winnerPlayer = firstPlayer;
                    break;
                }
                else if (firstPlayerTopCards[i].CardValue < secondPlayerTopCards[i].CardValue)
                {
                    winnerPlayer = secondPlayer;
                    break;
                }
            }

            return winnerPlayer;
        }

        private Player getWinnerFromFullHouse(Player firstPlayer, Player secondPlayer, List<Card> firstPlayerTopCards, List<Card> secondPlayerTopCards)
        {
            Player winnerPlayer = null;

            // Compare the ThreeOfaKind value
            if(firstPlayerTopCards[0].CardValue > secondPlayerTopCards[0].CardValue)
            {
                winnerPlayer = firstPlayer;
            }
            else if(firstPlayerTopCards[0].CardValue < secondPlayerTopCards[0].CardValue)
            {
                winnerPlayer = secondPlayer;
            }
            else
            {
                //in this case both players have the same ThreeOfaKind, check now the pair
                if (firstPlayerTopCards[3].CardValue > secondPlayerTopCards[3].CardValue)
                {
                    winnerPlayer = firstPlayer;
                }
                else if (firstPlayerTopCards[3].CardValue < secondPlayerTopCards[3].CardValue)
                {
                    winnerPlayer = secondPlayer;
                }
            }

            return winnerPlayer;
        }

        private Player getWinnerFromFourOfaKind(Player firstPlayer, Player secondPlayer, List<Card> firstPlayerTopCards, List<Card> secondPlayerTopCards)
        {
            Player winnerPlayer = null;

            // Compare the FourOfaKind value
            if (firstPlayerTopCards[0].CardValue > secondPlayerTopCards[0].CardValue)
            {
                winnerPlayer = firstPlayer;
            }
            else if (firstPlayerTopCards[0].CardValue < secondPlayerTopCards[0].CardValue)
            {
                winnerPlayer = secondPlayer;
            }
            else
            {
                //in this case both players have the same FourOfaKind, check now the High Card
                if (firstPlayerTopCards[4].CardValue > secondPlayerTopCards[4].CardValue)
                {
                    winnerPlayer = firstPlayer;
                }
                else if (firstPlayerTopCards[4].CardValue < secondPlayerTopCards[4].CardValue)
                {
                    winnerPlayer = secondPlayer;
                }
            }

            return winnerPlayer;
        }

        private Player getWinnerFromStraightFlush(Player firstPlayer, Player secondPlayer, List<Card> firstPlayerTopCards, List<Card> secondPlayerTopCards)
        {
            Player winnerPlayer = null;

            // Compare the last card value of straightFlush
            if (firstPlayerTopCards[4].CardValue > secondPlayerTopCards[4].CardValue)
            {
                winnerPlayer = firstPlayer;
            }
            else if (firstPlayerTopCards[4].CardValue < secondPlayerTopCards[4].CardValue)
            {
                winnerPlayer = secondPlayer;
            }

            return winnerPlayer;
        }

        // we try this method when the river is opened (4 cards are on table)
        public double[] getChanceOfWinning(Player firstPlayer, Player secondPlayer, List<Card> tableCards)
        {
            
            double[] oddsArray = new double[3];
            int firstPlayerWinsCounter = 0;
            int secondPlayerWinsCounter = 0;
            int tieCounter= 0;
            Deck deck = new Deck();
            // remove all opened cards from deck
            foreach(Card card in firstPlayer.Hand)
            {
                deck.removeCardFromDeck((Card.eCardValue)card.CardValue, card.CardSuit);
            }
            foreach (Card card in secondPlayer.Hand)
            {
                deck.removeCardFromDeck((Card.eCardValue)card.CardValue, card.CardSuit);
            }
            foreach (Card card in tableCards)
            {
                deck.removeCardFromDeck((Card.eCardValue)card.CardValue, card.CardSuit);
            }

            foreach(Card card in deck.CardList)
            {
                List<Card> tempList = new List<Card>();
                tempList.Add(card);

                List<Card> listOfFiveCard = tableCards.Concat(tempList).ToList();
                Player winnerPlayer = GetWinnerPlayer(firstPlayer, secondPlayer, listOfFiveCard);
                if(winnerPlayer == firstPlayer)
                {
                    firstPlayerWinsCounter++;
                }
                else if(winnerPlayer == secondPlayer)
                {
                    secondPlayerWinsCounter++;
                }
                else
                {
                    tieCounter++;
                }
            }

            oddsArray[0] = (double)firstPlayerWinsCounter / 44;
            oddsArray[1] = (double)secondPlayerWinsCounter / 44;
            oddsArray[2] = (double)tieCounter / 44;

            return oddsArray;
        }
    }
}
