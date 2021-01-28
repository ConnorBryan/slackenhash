using Terraria.ModLoader;

namespace Slackenhash
{
    public class StartCommand : ModCommand
    {
        public override CommandType Type => CommandType.World;

        public override string Command => "start";

        public override string Usage => "/start";

        public override string Description => "Start a new game of Slackenhash.";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            try
            {
                Slackenhash.instance.StartGame();
            } catch (System.Exception e)
            {
                Slackenhash.instance.Logger.Debug(e);
            }
        }
    }
}
