using CommandSystem;
using System;
using Exiled.API.Features;

namespace XyberC_plugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class ChangeClass : ParentCommand
    {
        public ChangeClass() => LoadGeneratedCommands();

        public override string Command { get; } = "changeclass";

        public override string[] Aliases { get; } = new string[] { "cclass", "cc" };

        public override string Description { get; } = "Like Forceclass, but Admingun compatible. Arguments: []";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "DISABLED";
            return false;
            Player ply = Player.Get(((CommandSender)sender).SenderId);
            if (!ply.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "Permission denied.";
                return false;
            }
            if (arguments.Count >= 2)
            {
                response = "Usage: \"changeclass\"";
                return false;
            }

            response = "";
            return false;
        }
    }
}