using Terraria;
using Terraria.UI;

namespace Slackenhash
{
    public class Card
    {
        public Slackenhash.CardKind kind;
        public Slackenhash.CardSubkind subkind;
        public string title;
        public string description;
        public int power;
        public Slackenhash.EquipmentSlot slot;
        public int value;
        public Equipment item;
        public bool isBig;

        public Card(
            Slackenhash.CardKind _kind,
            Slackenhash.CardSubkind _subkind,
            string _title,
            string _description,
            int _power,
            Slackenhash.EquipmentSlot _slot,
            int _value,
            Equipment _item,
            bool _isBig = false
        )
        {
            kind = _kind;
            subkind = _subkind;
            title = _title;
            description = _description;
            power = _power;
            slot = _slot;
            value = _value;
            item = _item;
            isBig = _isBig;
        }

        public override string ToString()
        {
            if (kind == Slackenhash.CardKind.Chamber)
            {
                return "Chamber Card: " + title;
            } else if (kind == Slackenhash.CardKind.Loot)
            {
                return "Loot Card: " + title;
            } else
            {
                return base.ToString();
            }
        }

        public void OnPrimary(UIMouseEvent evt, UIElement listener)
        {
            if (subkind == Slackenhash.CardSubkind.Equipment)
            {
                SlackenhashPlayer player = Main.LocalPlayer.GetModPlayer<SlackenhashPlayer>();

                player.EquipItem(item);
            }
        }
    }
}
