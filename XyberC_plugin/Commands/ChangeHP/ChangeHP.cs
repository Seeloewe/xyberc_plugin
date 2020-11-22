using CommandSystem;
using System;
using Exiled.API.Features;

namespace XyberC_plugin.AdminGun
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class ChangeHP : ParentCommand
    {
        public ChangeHP() => LoadGeneratedCommands();

        public override string Command { get; } = "changehp";

        public override string[] Aliases { get; } = new string[] { "chp" };

        public override string Description { get; } = "Changes HP of player";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(((CommandSender)sender).SenderId);
            if (!ply.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "Permission denied.";
                return false;
            }
            if (arguments.Count != 2)
            {
                response = "Usage: \"chp [ID] [amount]\"";
                return false;
            }
            Player Ply = Player.Get(arguments.At(0));
            if (Ply == null)
            {
                response = $"Player not found: {arguments.At(0)}";
                return false;
            }
            float curHP = Ply.Health;
            try
            {
                curHP += float.Parse(arguments.At(1));
            }
            catch
            {
                response = $"Not a valid number: {arguments.At(1)}";
                return false;
            }
            Ply.Health = curHP;
            response = $"HP of {Ply.Nickname} increased to {curHP}";
            return true;
        }
    }
}