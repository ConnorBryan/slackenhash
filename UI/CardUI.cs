using Terraria.UI;
using Terraria.GameContent.UI.Elements;

namespace Slackenhash.UI
{
    internal class CardUI : UIState
    {
        public UIPanel card;

        private UIPanel top;
        private UIPanel bottom;
        private Card sourceCard;

        public CardUI(Card _sourceCard)
        {
            sourceCard = _sourceCard;
        }

        public override void OnInitialize()
        {
            card = new UIPanel();
            card.Width.Pixels = Slackenhash.CARD_WIDTH;
            card.Height.Pixels = Slackenhash.CARD_WIDTH * Slackenhash.CARD_HEIGHT_TO_WIDTH_RATIO;

            top = new UIPanel();
            top.Top.Percent = 0.1f;
            top.Width.Percent = 1.0f;
            top.Height.Percent = 0.54f;

            bottom = new UIPanel();
            bottom.Top.Percent = 0.67f;
            bottom.Width.Percent = 1.0f;
            bottom.Height.Percent = 0.33f;

            card.Append(top);
            card.Append(bottom);

            AddSourceText();
            AddActions();

            Append(card);
        }

        private void AddSourceText()
        {
            // Title
            UIText title = new UIText(sourceCard.title, 0.8f);
            card.Append(title);
            // Power
            UIText power = new UIText("+" + sourceCard.power.ToString(), 1.1f);
            power.Left.Percent = 0.85f;
            card.Append(power);
            // Image
            // Subkind
            string text = Slackenhash.CARD_SUBKIND_TO_TITLE[sourceCard.subkind].ToUpper();

            if (sourceCard.subkind == Slackenhash.CardSubkind.Equipment)
            {
                if (sourceCard.slot != Slackenhash.EquipmentSlot.None)
                {
                    text += " [" + Slackenhash.EQUIPMENT_SLOT_TO_TITLE[sourceCard.slot] + "]";
                }

                if (sourceCard.isBig)
                {
                    text += " (Big)";
                }
            }

            UIText subkind = new UIText(text, 0.4f);
            subkind.Top.Pixels = -20;
            bottom.Append(subkind);

            // Description
            UIText description = new UIText(sourceCard.description, 0.5f);
            bottom.Append(description);
        }

        private void AddActions()
        {
            string primaryActionName;

            switch (sourceCard.subkind)
            {
                case Slackenhash.CardSubkind.Foe:
                    primaryActionName = "Fight";
                    break;
                case Slackenhash.CardSubkind.Race:
                case Slackenhash.CardSubkind.Class:
                    primaryActionName = "Become";
                    break;
                case Slackenhash.CardSubkind.Trap:
                    primaryActionName = "Set";
                    break;
                case Slackenhash.CardSubkind.Bonus:
                    primaryActionName = "Play";
                    break;
                case Slackenhash.CardSubkind.Curse:
                    primaryActionName = "Cast";
                    break;
                case Slackenhash.CardSubkind.Equipment:
                    primaryActionName = "Wear";
                    break;
                case Slackenhash.CardSubkind.Effect:
                    primaryActionName = "Cause";
                    break;
                default:
                    primaryActionName = "Play";
                    break;
            }

            UITextPanel<string> primaryAction = new UITextPanel<string>(primaryActionName, 0.5f);
            primaryAction.Top.Percent = 0.95f;
            primaryAction.PaddingTop = 5f;
            primaryAction.PaddingBottom = 5f;
            primaryAction.OnMouseDown += new MouseEvent(sourceCard.OnPrimary);
            bottom.Append(primaryAction);

            UITextPanel<string> secondaryAction = new UITextPanel<string>("Toss", 0.5f);
            secondaryAction.Top.Percent = 0.95f;
            secondaryAction.Left.Percent = 0.3f;
            secondaryAction.PaddingTop = 5f;
            secondaryAction.PaddingBottom = 5f;
            bottom.Append(secondaryAction);

            if (sourceCard.subkind == Slackenhash.CardSubkind.Equipment && sourceCard.value > 0) {
                // Value
                if (sourceCard.subkind == Slackenhash.CardSubkind.Equipment)
                {
                    UIText value = new UIText(sourceCard.value + "g", 0.5f);
                    value.Top.Pixels = -20;
                    value.Left.Percent = 0.8f;
                    bottom.Append(value);
                }

                UITextPanel<string> tertiaryAction = new UITextPanel<string>("Sell", 0.5f);
                tertiaryAction.Top.Percent = 0.95f;
                tertiaryAction.Left.Percent = 0.6f;
                tertiaryAction.PaddingTop = 5f;
                tertiaryAction.PaddingBottom = 5f;
                bottom.Append(tertiaryAction);
            }
        }
    }
}
