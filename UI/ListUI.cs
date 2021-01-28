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
            list.Width.Pixels = 200;
            list.Height.Pixels = 300;
            list.OverflowHidden = true;

            Append(list);
        }

        public void Add(string title, Action<UIMouseEvent, UIElement> OnSelect = null)
        {
            UIText item = new UIText(title, 0.6f);
            item.Top.Pixels = size * 20;
            item.Left.Pixels = 10;
            item.Left.Percent = 0;
            item.Height.Pixels = 20;

            if (OnSelect != null)
            {
                item.OnMouseDown += new MouseEvent(OnSelect);
            }

            item.OnMouseOver += new MouseEvent(OnItemMouseOver);
            item.OnMouseOut += new MouseEvent(OnItemMouseOut);

            list.Append(item);
            list.Height.Pixels += 20;

            size++;
        }

        private void OnItemMouseOver(UIMouseEvent evt, UIElement listener)
        {
            UIText caret = new UIText(">");
            caret.Top.Pixels = -4;
            caret.Left.Pixels = -13;
            listener.Append(caret);
        }

        private void OnItemMouseOut(UIMouseEvent evt, UIElement listener)
        {
            listener.RemoveAllChildren();
        }
    }
}
