using CommandSystem;
using System;
using Exiled.API.Features;

namespace XyberC_plugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class SetNick : ParentCommand
    {
        public SetNick() => LoadGeneratedCommands();

        public override string Command { get; } = "setnick";

        public override string[] Aliases { get; } = new string[] { "snick", "setnickname" };

        public override string Description { get; } = "Sets nickname of player\n\"[ID]\" - ID of player to set nickname of\n\"[text]\" - Text to set nickname to";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(((CommandSender)sender).SenderId);
            if (!ply.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "Permission denied.";
                return false;
            }
            if (arguments.Count < 2)
            {
                response = Description;
                return false;
            }
            Player Ply = Player.Get(arguments.At(0));
            if (Ply == null)
            {
                response = $"Player not found: {arguments.At(0)}";
                return false;
            }
            Ply.DisplayNickname = String.Join(" ", arguments.Segment(1, arguments.Count - 1));
            if (Ply.DisplayNickname == "")
            {
                response = $"Reset {Ply.Nickname}'s name";
            }
            else
            {
                response = $"Set {Ply.Nickname}'s display name to: {Ply.DisplayNickname}";
            }
            return true;
        }
    }
}