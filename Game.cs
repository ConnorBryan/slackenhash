using System;
using System.Collections.Generic;
using Terraria;
using Terraria.IO;
using Microsoft.Xna.Framework;

namespace Slackenhash
{
    public class Game
    {
        public static void Shuffle<T>(Random rng, T[] array)
        {
            int n = array.Length;

            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        public List<Card> chamberDeck;
        public List<Card> lootDeck;
        public History history;

        private bool inProgress;
        private bool determiningWinner;
        private int activePlayerIndex;
        private int updateCount;
        private int roundCount;
        private List<SlackenhashPlayer> playerOrder;
        private Random rng = new Random();
        private bool readyForNextTurn;

        public void Start()
        {
            history = new History();

            Reset();

            history.Log("A new game was started.");

            // Add and shuffle players.
            foreach (PlayerFileData data in Main.PlayerList)
            {
                Player player = data.Player;
                bool playerAlreadyThere = false;

                foreach (SlackenhashPlayer _player in playerOrder) {
                    if (_player.Name == player.GetModPlayer<SlackenhashPlayer>().Name)
                    {
                        playerAlreadyThere = true;
                    }
                }

                if (!playerAlreadyThere)
                {
                    playerOrder.Add(player.GetModPlayer<SlackenhashPlayer>());
                }
            }

            history.Log("Shuffling player order...");
            Shuffle(rng, playerOrder.ToArray());

            // Add and shuffle decks.
            // -- Chamber
            for (int i = 0; i < Slackenhash.DECK_SIZE; i++)
            {
                int which = rng.Next(0, CardList.chamberCards.Count);
                Card cardToAdd = CardList.chamberCards[which];

                chamberDeck.Add(cardToAdd);
            }

            // -- Loot
            for (int j = 0; j < Slackenhash.DECK_SIZE; j++)
            {
                int which = rng.Next(0, CardList.lootCards.Count);
                Card cardToAdd = CardList.lootCards[which];

                lootDeck.Add(cardToAdd);
            }

            history.Log("Shuffling Chamber deck...");
            Shuffle(rng, chamberDeck.ToArray());

            history.Log("Shuffling Loot deck...");
            Shuffle(rng, lootDeck.ToArray());

            // Deal to players.
            foreach (SlackenhashPlayer player in playerOrder)
            {
                for (int k = 0; k < Slackenhash.INITIAL_DEAL_SIZE; k++)
                {
                    DrawCard(player, Slackenhash.CardKind.Chamber);
                    DrawCard(player, Slackenhash.CardKind.Loot);
                }
            }

            // Kick things off.
            inProgress = true;
            roundCount++;

            Tick();
        }

        public void Reset()
        {
            history.Log("The game was reset.");

            inProgress = false;
            determiningWinner = false;
            activePlayerIndex = 0;
            updateCount = 0;
            roundCount = 0;
            playerOrder = new List<SlackenhashPlayer>();
            chamberDeck = new List<Card>();
            lootDeck = new List<Card>();
            readyForNextTurn = true;
        }

        public void Update(GameTime gameTime)
        {
            if (inProgress)
            {
                updateCount++;

                Tick();
            }
        }

        public void FinishTurn()
        {
            readyForNextTurn = true;
        }

        public Card DrawCard(SlackenhashPlayer player, Slackenhash.CardKind deck)
        {
            history.Log(player.player.name + " drew a " + Slackenhash.CARD_KIND_TO_TITLE[deck] + " card.");

            switch (deck)
            {
                case Slackenhash.CardKind.Chamber:
                    Card chamberCard = chamberDeck[0];
                    player.DealCard(chamberCard);
                    chamberDeck.RemoveAt(0);
                    return chamberCard;
                case Slackenhash.CardKind.Loot:
                    Card lootCard = lootDeck[0];
                    player.DealCard(lootCard);
                    lootDeck.RemoveAt(0);
                    return lootCard;
                default:
                    return null;
            }
        }

        private void Tick()
        {
            if (updateCount >= Slackenhash.FIFTEEN_MINUTES && !determiningWinner)
            {
                DetermineWinner();
            } else if (readyForNextTurn)
            {
                readyForNextTurn = false;
                activePlayerIndex++;

                if (activePlayerIndex >= playerOrder.Count)
                {
                    activePlayerIndex = 0;
                }

                SlackenhashPlayer activePlayer = playerOrder[activePlayerIndex];

                activePlayer.TakeTurn();
            }
        }

        private void DetermineWinner()
        {
            determiningWinner = true;
        }
    }
}
