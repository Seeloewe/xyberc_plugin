using CommandSystem;
using System;
using Exiled.API.Features;

namespace XyberC_plugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class AddCola: ParentCommand
    {
        public AddCola() => LoadGeneratedCommands();

        public override string Command { get; } = "addcola";

        public override string[] Aliases { get; } = new string[] { "acola" };

        public override string Description { get; } = "Adds Cola x4 effect to player. Argument: [player]";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(((CommandSender)sender).SenderId);
            if (!ply.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "Permission denied.";
                return false;
            }
            if (arguments.Count >= 2)
            {
                response = "Usage: \"addcola [player] [amount]\"";
                return false;
            }
            Player target;
            if (arguments.Count == 0)
            {
                target = ply;
            }
            else
            {
                target = Player.Get(arguments.At(0));
                if (target == null)
                {
                    response = $"Not a valid player: {arguments.At(0)}";
                    return false;
                }
            }
            target.EnableEffect<CustomPlayerEffects.Scp207>(999999f, true);
            target.ChangeEffectIntensity<CustomPlayerEffects.Scp207>(4);
            response = $"Gave {target.Nickname} 4x Cola";
            return true;
        }
    }
}