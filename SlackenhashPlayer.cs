using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.UI;

namespace Slackenhash
{
    public class SlackenhashPlayer : ModPlayer
    {
        public Slackenhash.Race race;
        public Slackenhash.Class klass;
        public Slackenhash.Phase phase;
        public List<Card> hand;
        public List<Equipment> equipment;
        public List<string> actions;
        public int chamber = 1;
        public int level = 1;
        public string foe;

        public int power
        {
            get
            {
                int fromEquipment = 0;
                foreach (Equipment item in equipment)
                {
                    fromEquipment += item.bonus;
                }

                return level + fromEquipment;
            }
        }

        public SlackenhashPlayer()
        {
            phase = Slackenhash.Phase.Waiting;
            hand = new List<Card>();
            equipment = new List<Equipment>();
            actions = new List<string>();
        }

        public void DealCard(Card card)
        {
            hand.Add(card);
        }

        public void TakeTurn()
        {
            Slackenhash.instance.AddHistoryLog(player.name + " started their turn.");

            AdvanceChamber();
        }

        private void AdvanceChamber()
        {
            Slackenhash.instance.AddHistoryLog(player.name + " entered a chamber.");

            phase = Slackenhash.Phase.BreachingTheChamber;

            DrawCard();
        }

        private void DrawCard()
        {
            Card drawnCard = Slackenhash.instance.game.DrawCard(this, Slackenhash.CardKind.Chamber);

            if (drawnCard.subkind == Slackenhash.CardSubkind.Foe)
            {
                Slackenhash.instance.AddHistoryLog(player.name + " spots a foe.");

                foe = drawnCard.title;
                phase = Slackenhash.Phase.FightingAFoe;
            }
            else
            {
                Slackenhash.instance.AddHistoryLog(player.name + " seems to be alone.");

                phase = Slackenhash.Phase.TakingItIn;
            }

            PromptDecision();
        }

        private void PromptDecision()
        {
            if (phase == Slackenhash.Phase.FightingAFoe)
            {
                Slackenhash.instance.ShowPrompt(foe + " stands before you...\nRun, cry or fight?", new List<(string, UIElement.MouseEvent)>() {
                    ("Run", Run),
                    ("Cry", Cry),
                    ("Fight", Fight),
                }, 10);
            } else
            {
                Slackenhash.instance.ShowPrompt("Lurk or Loot?", new List<(string, UIElement.MouseEvent)>() {
                    ("Lurk", Lurk),
                    ("Loot", Loot),
                });
            }
        }

        private void Run(UIMouseEvent evt, UIElement listener)
        {
            Slackenhash.instance.Logger.Debug("Ran");
        }

        private void Cry(UIMouseEvent evt, UIElement listener)
        {
            Slackenhash.instance.Logger.Debug("Cried");
        }

        private void Fight(UIMouseEvent evt, UIElement listener)
        {
            Slackenhash.instance.Logger.Debug("Fought");
        }

        private void Lurk(UIMouseEvent evt, UIElement listener)
        {
            Slackenhash.instance.Logger.Debug("Lurked");
        }

        private void Loot(UIMouseEvent evt, UIElement listener)
        {
            Slackenhash.instance.Logger.Debug("Looted");
            phase = Slackenhash.Phase.SheddingWeight;
        }

        private void FinishTurn()
        {
            phase = Slackenhash.Phase.Waiting;

            Slackenhash.instance.game.FinishTurn();
        }
    }
}
