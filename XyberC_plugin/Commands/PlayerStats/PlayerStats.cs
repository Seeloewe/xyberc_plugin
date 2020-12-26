using CommandSystem;
using System;
using Exiled.API.Features;
using System.Linq;

namespace XyberC_plugin.PlayerStats
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class ActivatePlayerStats : ParentCommand
    {
        public ActivatePlayerStats() => LoadGeneratedCommands();

        public override string Command { get; } = "playerstats";

        public override string[] Aliases { get; } = new string[] { "pstats" };

        public override string Description { get; } = "Activates/deactivates player stat tracking\nadd \"?\" to check current state, add \"display\" to publicly show current standings";

        public override void LoadGeneratedCommands() { }
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(((CommandSender)sender).SenderId);
            if (!ply.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "Permission denied.";
                return false;
            }
            if (arguments.Count == 1 && arguments.At(0) == "?")
            {
                if (XyberC_plugin.playerStats == true)
                {
                    response = "Player stats are currently enabled";
                    return true;
                }
                else
                {
                    response = "Player stats are currently disabled";
                    return true;
                }
            }
            else if (arguments.Count == 1 && (arguments.At(0) == "display" || arguments.At(0) == "disp" || arguments.At(0) == "d"))
            {
                foreach (PlayerStatsClass P1 in XyberC_plugin.HasPlayerStats)
                {
                    P1.Percentage = P1.Hits / P1.Shots;
                }
                PlayerStatsClass bestPlayer = XyberC_plugin.HasPlayerStats.Find(s => s.Percentage == XyberC_plugin.HasPlayerStats.Max(p => p.Percentage) && s.Shots > 10);
                if (bestPlayer == null)
                {
                    foreach (Player P1 in Player.List)
                    {
                        try
                        {
                            var Ply = XyberC_plugin.HasPlayerStats.Find(s => s.Id == P1.Id);
                            P1.Broadcast(8, $"<color=#3F9DFCA0>You hit {Ply.Hits} out of your {Ply.Shots} shots, an accuracy of {Ply.Percentage:0.##\\%}.</color>");
                        }
                        catch (ArgumentException) { }
                    }
                }
                else
                {
                    string bestPlayerName = Player.Get(bestPlayer.Id).DisplayNickname ?? Player.Get(bestPlayer.Id).Nickname;
                    foreach (Player P1 in Player.List)
                    {
                        try
                        {
                            var Ply = XyberC_plugin.HasPlayerStats.Find(s => s.Id == P1.Id);
                            P1.Broadcast(14, $"<color=#3F9DFCA0>You hit {Ply.Hits} out of your {Ply.Shots} shots, an accuracy of {Ply.Percentage:0.##\\%}.\nThe best player ({bestPlayerName}) hit {bestPlayer.Percentage:0.##\\%} out of {bestPlayer.Shots} shots.</color>");
                        }
                        catch (ArgumentException)
                        {
                            P1.Broadcast(8, $"<color=#3F9DFCA0>The best player ({bestPlayerName}) hit {bestPlayer.Percentage:0.##\\%} out of {bestPlayer.Shots} shots.</color>");
                        }
                    }
                }
                response = "Displayed player stats";
                return true;
            }
            else if (XyberC_plugin.playerStats == true)
            {
                XyberC_plugin.playerStats = false;
                XyberC_plugin.HasPlayerStats.Clear();
                response = "Player stats disabled";
                return true;
            }
            else
            {
                XyberC_plugin.playerStats = true;
                response = "Player stats activated";
                return true;
            }
        }
    }
}