using System;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

namespace Slackenhash.UI
{
    internal class ListUI : UIState
    {
        public UIPanel list;

        private int size = 0;

        public override void OnInitialize()
        {
            list = new UIPanel();
            list.Width.Pixels = Slackenhash.PLAYER_WIDTH;
            list.Height.Pixels = Slackenhash.PLAYER_WIDTH;
            list.OverflowHidden = true;

            Append(list);
        }

        public void Add(string title, Action<UIMouseEvent, UIElement> OnSelect = null)
        {
            UITextPanel<string> item = new UITextPanel<string>(title, 0.5f);
            item.Top.Pixels = 40 * size;

            if (OnSelect != null)
            {
                item.OnMouseDown += new MouseEvent(OnSelect);
            }

            list.Append(item);

            size++;
        }
    }
}
