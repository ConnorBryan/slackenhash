using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Slackenhash
{
    public class History
    {
        public Queue<string> events = new Queue<string>();
        public Queue<string> toasts = new Queue<string>();

        private int framesRemaining = 0;

        public void Log(string entry, bool toast = true)
        {
            Slackenhash.instance.AddHistoryLog(entry);

            events.Enqueue(entry);

            if (toast)
            {
                toasts.Enqueue(entry);
                ShowToast();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (framesRemaining > 0)
            {
                framesRemaining--;

                if (framesRemaining == 0)
                {
                    DismissToast();
                }
            }
        }

        private void ShowToast()
        {
            framesRemaining = Slackenhash.FOUR_SECONDS;

            string mostRecentEntry = toasts.Peek();

            // Display UIToast.
        }

        private void DismissToast()
        {
            toasts.Dequeue();

            if (toasts.Count > 0)
            {
                // Replace toast content with next message.
            } else
            {
                // Remove toast.
            }
        }
    }
}
