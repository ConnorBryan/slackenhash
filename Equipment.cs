namespace Slackenhash
{
    public class Equipment
    {
        public string title;
        public int power;
        public Slackenhash.EquipmentSlot slot;
        public bool isBig;

        public Equipment(string _title, int _power, Slackenhash.EquipmentSlot _slot, bool _isBig)
        {
            title = _title;
            power = _power;
            slot = _slot;
            isBig = _isBig;
        }
    }
}
