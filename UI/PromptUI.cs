using System;
using System.Collections.Generic;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;

namespace Slackenhash.UI
{
    public class PromptUI : UIState
    {
        private UITextPanel<string> prompt;
        private string text;
        private List<(string, MouseEvent)> options;
        private int time = 0;
        private bool ticking = false;
        private UIText countdown;

        public PromptUI(string _text, List<(string, MouseEvent)> _options, int _timeInSeconds = 15)
        {
            text = _text;
            options = _options;
            time = _timeInSeconds * 60;
        }

        public override void OnInitialize()
        {
            int lineCount = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Length;

            prompt = new UITextPanel<string>(text, 1f);
            prompt.Height.Pixels = lineCount * 30;

            int optionIndex = 0;
            float optionSize = 0;
            foreach ((string, MouseEvent) option in options)
            {
                if (option.Item1 != "...")
                {
                    UITextPanel<string> optionButton = new UITextPanel<string>(option.Item1.ToUpper(), 0.67f);
                    optionButton.Top.Percent = 1.2f;
                    optionButton.HAlign = (1.0f / options.Count) * optionIndex;
                    optionButton.OnMouseDown += new MouseEvent(option.Item2);
                    optionButton.SetPadding(10f);

                    prompt.Append(optionButton);

                    optionSize += optionButton.Width.Pixels;

                    optionIndex++;
                }
            }

            countdown = new UIText(FramesToString(), 2f, true);

            Append(countdown);

            prompt.Width.Pixels = optionSize;
            prompt.HAlign = 0.5f;
            prompt.VAlign = 0.05f;

            countdown.HAlign = 0.95f;
            countdown.VAlign = 0.05f;

            Append(prompt);

            ticking = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (ticking)
            {
                time--;

                countdown.SetText(FramesToString());

                if (time == 0)
                {
                    countdown.TextColor = new Color(255, 0, 0);

                    ticking = false;

                    if (options.Count > 0)
                    {
                       // The default option is the first one.
                        options[0].Item2(null, null);
                    }
                }
            }
        }

        private string FramesToString()
        {
            double seconds = Math.Floor((double) time / 60);

            return seconds.ToString();
        }
    }
}
