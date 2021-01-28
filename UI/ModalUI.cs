using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;

namespace Slackenhash.UI
{
    internal class ModalUI : UIState
    {
        public DragableUIPanel modal;

        private string title;
        private UIElement element;
        private bool dismissable;
        private UIScrollbar scrollbar;
        private float scrollDistance = 0;

        public ModalUI(string _title, UIElement _element, bool _dismissable = true)
        {
            title = _title;
            element = _element;
            dismissable = _dismissable;
        }

        public override void OnInitialize()
        {
            modal = new DragableUIPanel(200);
            modal.Top.Percent = 0.45f;
            modal.Left.Percent = 0.45f;
            modal.Width.Pixels = Slackenhash.CARD_WIDTH;
            modal.Height.Pixels = Slackenhash.CARD_WIDTH * Slackenhash.CARD_HEIGHT_TO_WIDTH_RATIO;
            modal.OverflowHidden = true;
            modal.SetPadding(0);

            UIPanel top = new UIPanel();
            top.Width.Percent = 1.0f;
            top.Height.Pixels = 20;
            top.SetPadding(0);

            UIText modalTitle = new UIText(title.ToUpper(), 0.67f);
            modalTitle.Top.Pixels = 5;
            modalTitle.Left.Pixels = 25;

            top.Append(modalTitle);

            if (dismissable)
            {
                UIText close = new UIText("X", 0.67f);
                close.Top.Pixels = 5;
                close.Left.Pixels = -25;
                close.Left.Percent = 1f;
                close.Width.Pixels = 10;
                close.Height.Pixels = 10;
                close.OnMouseDown += new MouseEvent(OnClose);

                top.Append(close);
            }

            modal.Append(top);
            modal.OnScrollWheel += new ScrollWheelEvent(ScrollWheel);

            element.Top.Pixels = -scrollDistance;

            Add(element);

            AddScrollbar();

            Append(modal);

        }

        public void Add(UIElement element)
        {
            element.Top.Pixels += 20;
            element.Top.Percent = 0;
            element.Left.Pixels = 0;
            element.Left.Percent = 0;
            scrollbar?.SetView(0f, 300f);
            modal.Append(element);
        }

        private void AddScrollbar()
        {
            scrollbar = new UIScrollbar();
            scrollbar.Top.Percent = 0.1f;
            scrollbar.Left.Percent = 0.9f;
            scrollbar.Width.Percent = 0.05f;
            scrollbar.Height.Pixels = element.Height.Pixels - 20;
            scrollbar.SetView(element.Height.Pixels, 200f);
            scrollbar.OnScrollWheel += new ScrollWheelEvent(ScrollWheel);
            modal.Append(scrollbar);
        }

        private void OnClose(UIMouseEvent evt, UIElement listener)
        {
            Slackenhash.instance.HideModal();
        }

        public void ScrollWheel(UIScrollWheelEvent evt, UIElement listener)
        {
            if (evt.ScrollWheelValue >= 0)
            {
                // Up
                scrollDistance -= 5;

                if (scrollDistance <= 0)
                {
                    scrollDistance = 0;
                }
            } else
            {
                // Down
                scrollDistance += 5;

                if (scrollDistance >= element.Height.Pixels)
                {
                    scrollDistance = element.Height.Pixels;
                }
            }

            element.Top.Pixels = -scrollDistance + 20;
        }
    }
}
