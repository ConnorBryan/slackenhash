using Terraria.UI;
using Terraria.GameContent.UI.Elements;

namespace Slackenhash.UI
{
    internal class ModalUI : UIState
    {
        public DragableUIPanel modal;

        private string title;
        private UIElement element;
        private bool dismissable;

        public ModalUI(string _title, UIElement _element, bool _dismissable = true)
        {
            title = _title;
            element = _element;
            dismissable = _dismissable;
        }

        public override void OnInitialize()
        {
            modal = new DragableUIPanel();
            modal.Top.Percent = 0.45f;
            modal.Left.Percent = 0.45f;
            modal.Width.Pixels = Slackenhash.CARD_WIDTH;
            modal.Height.Pixels = Slackenhash.CARD_WIDTH * Slackenhash.CARD_HEIGHT_TO_WIDTH_RATIO;
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

            Add(element);

            Append(modal);
        }

        public void Add(UIElement element)
        {
            element.Top.Pixels += 20;
            element.Top.Percent = 0;
            element.Left.Pixels = 0;
            element.Left.Percent = 0;

            modal.Append(element);
        }

        private void OnClose(UIMouseEvent evt, UIElement listener)
        {
            Slackenhash.instance.HideModal();
        }
    }
}
