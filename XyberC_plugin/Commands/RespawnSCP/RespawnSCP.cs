using CommandSystem;
using System;
using Exiled.API.Features;
using System.Linq;
using System.Collections.Generic;

namespace XyberC_plugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class RespawnSCP : ParentCommand
    {
        public RespawnSCP() => LoadGeneratedCommands();

        public override string Command { get; } = "respawn";

        public override string[] Aliases { get; } = new string[] { "rs" };

        public override string Description { get; } = "Respawns the last SCP that left the game at their HP value\n\"[ID]\" - Respawn specific player ID\n\"spawn\" - Respawn at default location\n\"hp\" - Respawn at default HP\n\"bc\" - Respawn without broadcast";

        public override void LoadGeneratedCommands() { }
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(((CommandSender)sender).SenderId);
            if (!ply.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "Permission denied.";
                return false;
            }
            if (arguments.Count > 4)
            {
                response = "Too many arguments";
                return false;
            }
            if (arguments.Count == 1)
            {
                if (arguments.At(0) == "?")
                {
                    if (XyberC_plugin.replaceSCP == true)
                    {
                        response = "Respawning SCPs is enabled";
                        return true;
                    }
                    else
                    {
                        response = "Respawning SCPs is disabled";
                        return true;
                    }
                }
                if (arguments.At(0) == "t")
                {
                    if (XyberC_plugin.replaceSCP == true)
                    {
                        XyberC_plugin.replaceSCP = false;
                        response = "Disabled Respawning SCPs";
                        return true;
                    }
                    else
                    {
                        XyberC_plugin.replaceSCP = true;
                        response = "Enabled Respawning SCPs";
                        return true;
                    }
                }
            }
            if (XyberC_plugin.replaceSCP == false)
            {
                response = "Respawning SCPs is disabled";
                return false;
            }
            if (XyberC_plugin.ReplaceSCP == RoleType.None)
            {
                response = "No SCP player has left and can be revived";
                return false;
            }
            Player ReviveMe;
            bool keeppos = true;
            bool hp = true;
            bool bc = true;
            List<string> args = arguments.ToList();
            if (args.Any(x => x == "spawn"))
            {
                keeppos = false;
                args.Remove("spawn");
            }
            if (args.Any(x => x == "hp"))
            {
                hp = false;
                args.Remove("hp");
            }
            if (args.Any(x => x == "bc"))
            {
                bc = false;
                args.Remove("bc");
            }
            if (args.Count == 0)
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
                ReviveMe = Player.Get(args[0]);
                if (ReviveMe == null)
                {
                    response = $"Not a player: {args[0]}";
                    return false;
                }
            }
            if (keeppos == true)
            {
                ReviveMe.Position = XyberC_plugin.ReplaceSCPpos;
                ReviveMe.SetRole(XyberC_plugin.ReplaceSCP, true, false);
            }
            else
            {
                ReviveMe.SetRole(XyberC_plugin.ReplaceSCP, false, false);
            }
            if (hp == true)
            {
                ReviveMe.Health = XyberC_plugin.ReplaceSCPHP;
                ReviveMe.AdrenalineHealth = XyberC_plugin.ReplaceSCPAHP;
            }
            if (bc == true)
            {
                foreach (Player P1 in Player.List)
                {
                    P1.Broadcast(5, $"<color=FFFFFF90>Respawning {XyberC_plugin.ReplaceSCP} (disconnected)</color>");
                }
            }
            XyberC_plugin.ReplaceSCPpos = UnityEngine.Vector3.zero;
            XyberC_plugin.ReplaceSCP = RoleType.None;
            XyberC_plugin.ReplaceSCPHP = 0f;
            XyberC_plugin.ReplaceSCPAHP = 0f;
            response = $"Respawned {ReviveMe.Nickname} ({ReviveMe.Id}) as {ReviveMe.Role} with {ReviveMe.Health} HP, {ReviveMe.AdrenalineHealth} AHP";
            return true;
        }
    }
}