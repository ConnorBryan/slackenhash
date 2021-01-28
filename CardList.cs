using System.Collections.Generic;

namespace Slackenhash
{
    public class CardList
    {
        public static List<Card> chamberCards = new List<Card>()
        {
            new Card(
                Slackenhash.CardKind.Chamber,
                Slackenhash.CardSubkind.Foe,
                "Example Foe",
                "An example foe.",
                1,
                Slackenhash.EquipmentSlot.None,
                0,
                new Equipment(
                    "Example Headwear",
                    1,
                    Slackenhash.EquipmentSlot.None,
                    false
                )
            )
        };

        public static List<Card> lootCards = new List<Card>()
        {
            new Card(
                Slackenhash.CardKind.Loot,
                Slackenhash.CardSubkind.Equipment,
                "Example Headwear",
                "An example helmet.",
                1,
                Slackenhash.EquipmentSlot.Headwear,
                250,
                new Equipment(
                    "Example Headwear",
                    1,
                    Slackenhash.EquipmentSlot.Headwear,
                    true
                ),
                true
            )
        };
    }
}
