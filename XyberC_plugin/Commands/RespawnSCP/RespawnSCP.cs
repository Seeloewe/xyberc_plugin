using CommandSystem;
using System;
using Exiled.API.Features;
using System.Linq;

namespace XyberC_plugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class RespawnSCP : ParentCommand
    {
        public RespawnSCP() => LoadGeneratedCommands();

        public override string Command { get; } = "respawn";

        public override string[] Aliases { get; } = new string[] { "revive", "rs", "rv" };

        public override string Description { get; } = "Respawns the last SCP that left the game at their HP value, optional argument player ID to respawn";

        public override void LoadGeneratedCommands() { }
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(((CommandSender)sender).SenderId);
            if (!ply.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "Permission denied.";
                return false;
            }
            if (arguments.Count > 1)
            {
                response = "Too many arguments.";
                return false;
            }
            if (XyberC_plugin.ReplaceSCP == RoleType.None)
            {
                response = "No SCP player has left so far";
                return false;
            }

            Player ReviveMe;
            if (arguments.Count == 0)
            {
                ReviveMe = Player.List.FirstOrDefault(p => p.Role == RoleType.Spectator);
                if (ReviveMe == null)
                {
                    response = "There is no spectator available, please specify player ID.";
                    return false;
                }
            }
            else
            {
                ReviveMe = Player.Get(arguments.At(0));
                if (ReviveMe == null)
                {
                    response = $"Not a player: {arguments.At(0)}";
                    return false;
                }
            }
            XyberC_plugin.ReplaceMeSCP = ReviveMe.Id;
            ReviveMe.Position = XyberC_plugin.ReplaceSCPpos;
            ReviveMe.SetRole(XyberC_plugin.ReplaceSCP);
            ReviveMe.Health = XyberC_plugin.ReplaceSCPHP;
            ReviveMe.AdrenalineHealth = XyberC_plugin.ReplaceSCPAHP;
            foreach (Player P1 in Player.List)
            {
                P1.Broadcast(5, $"<color=FFFFFF90>Respawning {XyberC_plugin.ReplaceSCP} (disconnected)</color>");
            }
            response = $"Respawned {ReviveMe.Nickname} ({ReviveMe.Id}) as {ReviveMe.Role} with {ReviveMe.Health} HP, {ReviveMe.AdrenalineHealth} AHP";
            return true;
        }
    }
}