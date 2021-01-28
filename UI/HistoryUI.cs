using System;
using Terraria.UI;

namespace Slackenhash.UI
{
    internal class HistoryUI : UIState
    {
        public ListUI list;

        public override void OnInitialize()
        {

            list = new ListUI();
            list.Activate();

            list.list.Width.Pixels = 230;

            Slackenhash.instance.ShowModal("History", list);
        }

        public void Add(string entry)
        {
            list.Add("[" + DateTime.Now.ToShortTimeString() + "]: " + entry);
        }
    }
}
