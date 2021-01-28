using Terraria.UI;
using Terraria.GameContent.UI.Elements;

namespace Slackenhash.UI
{
    internal class ModalUI : UIState
    {
        public DragableUIPanel modal;
        public UIText modalTitle;
        public UIElement element;

        private bool dismissable;
        private UIPanel top;
        private bool hasShown = false;

        internal int TOP_HEIGHT = 20;
        internal int MODAL_HEIGHT = 300;

        public ModalUI(bool _dismissable = true)
        {
            dismissable = _dismissable;
        }

        public override void OnInitialize()
        {
            modal = new DragableUIPanel(MODAL_HEIGHT);
            modal.Top.Percent = 0.45f;
            modal.Left.Percent = 0.45f;
            modal.Width.Pixels = Slackenhash.CARD_WIDTH;
            modal.Height.Pixels = Slackenhash.CARD_WIDTH * Slackenhash.CARD_HEIGHT_TO_WIDTH_RATIO;
            modal.SetPadding(0);

            Append(modal);
        }

        public void SetTitle(string _title)
        {
            modalTitle = new UIText(_title.ToUpper(), 0.5f);
            modalTitle.VAlign = 0.5f;
            modalTitle.HAlign = 0.1f;
        }

        public void SetElement(UIElement _element)
        {
            element = _element;
            element.Top.Pixels += 20;
            element.Top.Percent = 0;
            element.Left.Pixels = 0;
            element.Left.Percent = 0;
            element.HAlign = 0.5f;
        }

        public void Build()
        {
            if (hasShown)
            {
                Clear();
            }

            hasShown = true;

            top = new UIPanel();
            top.Width.Pixels = Slackenhash.PLAYER_WIDTH;
            top.Height.Pixels = TOP_HEIGHT;
            top.SetPadding(0);

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

            top.Append(modalTitle);

            modal.Append(top);
            modal.Append(element);
        }

        public void Clear()
        {
            if (hasShown)
            {
                modal.RemoveAllChildren();
            }
        }

        private void OnClose(UIMouseEvent evt, UIElement listener)
        {
            Slackenhash.instance.HideModal();
        }
    }
}
