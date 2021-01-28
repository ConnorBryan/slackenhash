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
        public int index = -1;
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
                    fromEquipment += item.power;
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

        public bool EquipItem(Equipment item)
        {
            int previousSlotItemIndex = -1;
            int i = 0;
            bool hasBigItemAlready = false;

            foreach (Equipment _item in equipment)
            {
                if (_item.isBig)
                {
                    hasBigItemAlready = true;
                }

                if (_item.slot == item.slot)
                {
                    previousSlotItemIndex = i;
                }

                i++;
            }

            if (item.isBig && hasBigItemAlready)
            {
                return false;
            } else
            {
                if (previousSlotItemIndex != -1)
                {
                    equipment.RemoveAt(previousSlotItemIndex);
                }

                equipment.Add(item);

                Slackenhash.instance.UpdatePower();

                return true;
            }
        }

        public void DealCard(Card card)
        {
            hand.Add(card);
        }

        public void TakeTurn()
        {
            Slackenhash.instance.AddHistoryLog(player.name + " started their turn.");
            Slackenhash.instance.ShowPrompt(player.name + ", you're up!", new List<(string, UIElement.MouseEvent)>{
                ("...", AdvanceChamber)
            }, 5);
        }

        private void AdvanceChamber(UIMouseEvent evt, UIElement listener)
        {
            Slackenhash.instance.AddHistoryLog(player.name + " entered a chamber.");

            phase = Slackenhash.Phase.BreachingTheChamber;

            Slackenhash.instance.ShowPrompt(player.name + " approaches the chamber door...", new List<(string, UIElement.MouseEvent)>{
                ("...", DrawCard)
            }, 5);
        }

        private void DrawCard(UIMouseEvent evt, UIElement listener)
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
            Slackenhash.instance.Logger.Debug('X');
            Slackenhash.instance.ShowPrompt(player.name + " runs away screaming!", new List<(string, UIElement.MouseEvent)>{
                ("...", FinishTurn)
            }, 10);
        }

        private void Cry(UIMouseEvent evt, UIElement listener)
        {
            Slackenhash.instance.Logger.Debug('Y');
            Slackenhash.instance.ShowPrompt(player.name + " cried for help!", new List<(string, UIElement.MouseEvent)>{
                ("...", Fight)
            }, 10);
        }

        private void Fight(UIMouseEvent evt, UIElement listener)
        {
            Slackenhash.instance.Logger.Debug('Z');
            Slackenhash.instance.ShowPrompt(player.name + " steps up to the plate!", new List<(string, UIElement.MouseEvent)>{
                ("...", FinishTurn)
            }, 30);
        }

        private void Lurk(UIMouseEvent evt, UIElement listener)
        {
            Slackenhash.instance.ShowPrompt(player.name + " is looking for a fight!", new List<(string, UIElement.MouseEvent)>{
                ("...", FinishTurn)
            }, 10);
        }

        private void Loot(UIMouseEvent evt, UIElement listener)
        {
            Slackenhash.instance.ShowPrompt(player.name + " is taking what they can get.", new List<(string, UIElement.MouseEvent)>{
                ("...", FinishTurn)
            }, 10);
        }

        private void FinishTurn(UIMouseEvent evt, UIElement listener)
        {
            phase = Slackenhash.Phase.SheddingWeight;

            Slackenhash.instance.ShowPrompt(player.name + " is loosening their load.", new List<(string, UIElement.MouseEvent)>{
                ("...", Slackenhash.instance.game.FinishTurn)
            }, 15);
        }
    }
}
