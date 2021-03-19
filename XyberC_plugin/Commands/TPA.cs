using CommandSystem;
using System;
using Exiled.API.Features;

namespace XyberC_plugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class TPA: ParentCommand
    {
        public TPA() => LoadGeneratedCommands();

        public override string Command { get; } = "tpa";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Teleports player to a room ID. Arguments: \"save\" to save current location or [player / \'all\']";

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
                response = "Usage: \"tpa \'save\'\" or \"tpa [player / \'all\']\"";
                return false;
            }
            if (arguments.Count == 0)
            {
                if (XyberC_plugin.savedPositions.TryGetValue(ply.Id, out UnityEngine.Vector3 pos1) == true)
                {
                    ply.Position = pos1;
                    response = $"Teleported you to {pos1}";
                    return true;
                }
                else
                {
                    response = "You have not saved a position yet.";
                    return false;
                }
            }
            if (arguments.At(0) == "save" || arguments.At(0) == "s")
            {
                XyberC_plugin.savedPositions.Add(ply.Id, ply.Position);
                response = $"Saved tpa position at {ply.Position}";
                return true;
            }
            if (XyberC_plugin.savedPositions.TryGetValue(ply.Id, out UnityEngine.Vector3 pos2) == true)
            {
                if (arguments.At(0) == "all")
                {
                    foreach (Player player in Player.List)
                    {
                        player.Position = pos2;
                    }
                    response = $"Teleported everyone to {pos2}";
                    return true;
                }
                Player target = Player.Get(arguments.At(0));
                if (target == null)
                {
                    response = $"Not a valid player: {arguments.At(0)}";
                    return false;
                }
                target.Position = pos2;
                response = $"Teleported {target.Nickname} to {pos2}";
                return true;
            }
            else
            {
                response = "You have not saved a position yet.";
                return false;
            }
        }
    }
}