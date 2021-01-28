using System;
using Terraria.UI;

namespace Slackenhash.UI
{
    internal class HistoryUI : UIState
    {
        public ListUI list;

        private ModalUI modalUI;

        public override void OnInitialize()
        {

            list = new ListUI();
            list.Activate();

            list.list.Width.Pixels = 230;

            modalUI = new ModalUI("History", list.list, false);
            modalUI.Activate();

            Append(modalUI);
        }

        public void Add(string entry)
        {
            list.Add("[" + DateTime.Now.ToShortTimeString() + "]: " + entry);
        }
    }
}
