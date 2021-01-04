using CommandSystem;
using System;
using Exiled.API.Features;

namespace XyberC_plugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class ChangeAHP : ParentCommand
    {
        public ChangeAHP() => LoadGeneratedCommands();

        public override string Command { get; } = "changeahp";

        public override string[] Aliases { get; } = new string[] { "cahp" };

        public override string Description { get; } = "Changes AHP of player";

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
                response = "Usage: \"cahp [ID] [amount]\"";
                return false;
            }
            Player Ply = Player.Get(arguments.At(0));
            if (Ply == null)
            {
                response = $"Player not found: {arguments.At(0)}";
                return false;
            }
            float curHP = Ply.AdrenalineHealth;
            try
            {
                curHP += float.Parse(arguments.At(1));
            }
            catch
            {
                response = $"Not a valid number: {arguments.At(1)}";
                return false;
            }
            Ply.AdrenalineHealth = curHP;
            response = $"AHP of {Ply.Nickname} changed to {curHP}";
            return true;
        }
    }
}