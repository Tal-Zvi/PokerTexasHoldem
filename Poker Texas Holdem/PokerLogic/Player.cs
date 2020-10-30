using System;
using System.Collections.Generic;
using System.Text;

namespace PokerLogic
{
    internal class Player
    {
        private string playerName;
        private int playerWallet;
        private bool hasEnoughMoneyToPlay;
        private List<Card> hand = new List<Card>();

        public Player(string playerName)
        {
            this.playerName = playerName;
            this.playerWallet = 10000;
            this.hasEnoughMoneyToPlay = true;
        }

        public List<Card> Hand
        {
            get
            {
                return this.hand;
            }
        }

        public string PlayerName
        {
            get
            {
                return this.playerName;
            }
        }

        public int PlayerWallet {
            get
            {
                return this.playerWallet;
            }
        }

        public void addMoneyToWallet(int amount)
        {
            this.playerWallet += amount;
        }

        public void reduceMoneyFromWallet(int amount)
        {
            if(amount >= playerWallet)
            {
                playerWallet = 0;
                hasEnoughMoneyToPlay = false;
            }
            else
            {
                this.playerWallet -= amount;
            }
        }

        public void giveCardsToPlayer(List<Card> i_Hand)
        {
            this.hand = i_Hand;
        }

        public StringBuilder getPlayerDetails()
        {
            StringBuilder playerStringBuilder = new StringBuilder();
            playerStringBuilder.Append(String.Format("{0} has {1}$ in the wallet", this.playerName, this.playerWallet));
            return playerStringBuilder;
        }
    }
}