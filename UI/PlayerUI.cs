using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

namespace Slackenhash.UI
{
    internal class PlayerUI : UIState
    {
        public List<UIPanel> players;
        public List<SlackenhashPlayer> sourcePlayers;

        public override void OnInitialize()
        {
            players = new List<UIPanel>();
        }

        public void Show()
        {
            RemoveAllChildren();

            players = new List<UIPanel>();
            sourcePlayers = new List<SlackenhashPlayer>()
            {
                Main.LocalPlayer.GetModPlayer<SlackenhashPlayer>(),
            };

            int playerIndex = 0;
            foreach (SlackenhashPlayer somePlayer in sourcePlayers)
            {
                UIPanel playerPanel = new UIPanel();
                playerPanel.Width.Pixels = Slackenhash.PLAYER_WIDTH;
                playerPanel.Height.Pixels = Slackenhash.PLAYER_WIDTH * Slackenhash.PLAYER_HEIGHT_TO_WIDTH_RATIO;
                playerPanel.Top.Pixels = -(playerPanel.Height.Pixels + 30);
                playerPanel.Top.Percent = 1f;
                playerPanel.Left.Pixels = 30 + playerIndex * Slackenhash.PLAYER_WIDTH;

                UIText power = new UIText("+" + somePlayer.power.ToString(), 1.3f);
                power.Top.Pixels = 30;
                power.Left.Pixels = 10;

                playerPanel.Append(power);

                UIText name = new UIText(somePlayer.player.name.ToUpper(), 0.8f);
                name.Top.Pixels = 5;
                name.Left.Pixels = 45;

                playerPanel.Append(name);

                UIText raceClass = new UIText(somePlayer.race + " " + somePlayer.klass, 0.6f);
                raceClass.Top.Pixels = 25;
                raceClass.Left.Pixels = 45;

                playerPanel.Append(raceClass);

                UIText levelChamber = new UIText(Slackenhash.LEVEL_NAME_LOOKUP[somePlayer.level] + " " + Slackenhash.ROMAN_NUMERAL_LOOKUP[somePlayer.chamber], 0.5f);
                levelChamber.Top.Pixels = 45;
                levelChamber.Left.Pixels = 45;

                playerPanel.Append(levelChamber);

                UITextPanel<string> handButton = new UITextPanel<string>("Hand".ToUpper(), 0.6f);
                handButton.Height.Pixels = 5;
                handButton.Height.Percent = 0;
                handButton.Width.Pixels = 20;
                handButton.Top.Set(-(handButton.Height.Pixels + 30), 1f);
                handButton.Left.Pixels = 5;
                handButton.OnMouseDown += new MouseEvent(ShowHandPanel);

                playerPanel.Append(handButton);

                UITextPanel<string> equipmentButton = new UITextPanel<string>("Equipment".ToUpper(), 0.5f);
                equipmentButton.Height.Pixels = 5;
                equipmentButton.Height.Percent = 0;
                equipmentButton.Width.Pixels = 25;
                equipmentButton.Top.Set(-(equipmentButton.Height.Pixels + 30), 1f);
                equipmentButton.Left.Pixels = 55;
                equipmentButton.OnMouseDown += new MouseEvent(ShowEquipmentPanel);

                playerPanel.Append(equipmentButton);

                UITextPanel<string> actionsButton = new UITextPanel<string>("Actions".ToUpper(), 0.5f);
                actionsButton.Height.Pixels = 5;
                actionsButton.Height.Percent = 0;
                actionsButton.Width.Pixels = 25;
                actionsButton.Top.Set(-(actionsButton.Height.Pixels + 30), 1f);
                actionsButton.Left.Pixels = 130;
                actionsButton.OnMouseDown += new MouseEvent(ShowActionsPanel);

                playerPanel.Append(actionsButton);

                Append(playerPanel);

                players.Add(playerPanel);

                somePlayer.index = playerIndex;

                playerIndex++;
            }
        }

        private SlackenhashPlayer GetRelevantPlayer(UIElement listener)
        {
            int index = -1;

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] == listener.Parent)
                {
                    index = i;
                    break;
                }
            }

            SlackenhashPlayer relevantPlayer = sourcePlayers[index];

            return relevantPlayer;
        }
        
        private void ShowHandPanel(UIMouseEvent evt, UIElement listener)
        {
            SlackenhashPlayer relevantPlayer = GetRelevantPlayer(listener);

            if (relevantPlayer != null)
            {
                ListUI handPanel = new ListUI();
                handPanel.Activate();

                
                foreach (Card card in relevantPlayer.hand)
                {
                    handPanel.Add(card.title, OnClickCard);
                }

                Slackenhash.instance.ShowModal(relevantPlayer.player.name + "'s Hand", handPanel);
            }
        }

        private void ShowEquipmentPanel(UIMouseEvent evt, UIElement listener)
        {
            SlackenhashPlayer relevantPlayer = GetRelevantPlayer(listener);

            if (relevantPlayer != null)
            {
                ListUI equipmentPanel = new ListUI();
                equipmentPanel.Activate();

                equipmentPanel.Add("Item A", OnClickCard);
                equipmentPanel.Add("Item B", OnClickCard);

                foreach (Equipment item in relevantPlayer.equipment)
                {
                    equipmentPanel.Add(item.title, OnClickCard);
                }

                Slackenhash.instance.ShowModal(relevantPlayer.player.name + "'s Equipment", equipmentPanel);
            }
        }

        private void ShowActionsPanel(UIMouseEvent evt, UIElement listener)
        {
            SlackenhashPlayer relevantPlayer = GetRelevantPlayer(listener);

            if (relevantPlayer != null)
            {
                ListUI actionsPanel = new ListUI();
                actionsPanel.Activate();

                actionsPanel.Add("Action A", OnClickCard);
                actionsPanel.Add("Action B", OnClickCard);

                //foreach (SlackenhashAction action in relevantPlayer.actions)
                //{
                //    actionsPanel.Add(action.title, OnClickCard);
                //}

                Slackenhash.instance.ShowModal(relevantPlayer.player.name + "'s Actions", actionsPanel);
            }
        }

        private void OnClickCard(UIMouseEvent evt, UIElement listener)
        {
            CardUI shownCard = new CardUI(CardList.lootCards[0]);
            shownCard.Activate();

            Slackenhash.instance.ShowModal("Card", shownCard);
        }
    }
}
