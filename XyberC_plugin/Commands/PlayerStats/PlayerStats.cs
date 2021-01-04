using CommandSystem;
using System;
using System.Collections.Generic;
using Exiled.API.Features;
using System.Linq;

namespace XyberC_plugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class ActivatePlayerStats : ParentCommand
    {
        public ActivatePlayerStats() => LoadGeneratedCommands();

        public override string Command { get; } = "playerstats";

        public override string[] Aliases { get; } = new string[] { "pstats" };

        public override string Description { get; } = "Activates/deactivates player stat tracking\nArguments: \"?\": check if active, \"show\": show standings in console, \"display\": show standings publicly";

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
                response = "Too many arguments";
                return false;
            }
            if (arguments.Count == 0)
            {
                if (XyberC_plugin.playerStats == true)
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
            if (arguments.At(0) == "?")
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
            if (arguments.At(0) == "show" || arguments.At(0) == "s")
            {
                if (XyberC_plugin.HasPlayerStats.Any())
                {
                    string text = "<color=#3F9DFCA0>Current standings:</color>\n";
                    List<PlayerStatsClass> plyList = XyberC_plugin.HasPlayerStats.ToList();
                    foreach (PlayerStatsClass P1 in plyList)
                    {
                        P1.Percentage = 100.0f * P1.Hits / P1.Shots;
                    }
                    plyList.Sort((x, y) => y.Percentage.CompareTo(x.Percentage));
                    foreach (PlayerStatsClass Ply in plyList)
                    {
                        Player P1 = Player.Get(Ply.Id);
                        string playerName;
                        if (P1 == null)
                        {
                            playerName = Ply.Name;
                        }
                        else
                        {
                            playerName = Player.Get(P1.Id).DisplayNickname ?? Player.Get(P1.Id).Nickname;
                        }
                        text += $"{Ply.Percentage:0.##\\%}: {playerName} hit {Ply.Hits} out of {Ply.Shots}\n";
                    }
                    response = text;
                    return true;
                }
                else
                {
                    response = "No player stats have been recorded so far";
                    return true;
                }
            }
            if (arguments.At(0) == "display" || arguments.At(0) == "disp" || arguments.At(0) == "d")
            {
                if (XyberC_plugin.HasPlayerStats.Any())
                {
                    List<PlayerStatsClass> plyList = XyberC_plugin.HasPlayerStats.ToList();
                    foreach (PlayerStatsClass P1 in plyList)
                    {
                        P1.Percentage = 100.0f * P1.Hits / P1.Shots;
                    }
                    plyList.RemoveAll(x => x.Shots < 10);
                    if (plyList.Count == 0)
                    {
                        foreach (Player P1 in Player.List)
                        {
                            PlayerStatsClass Ply = XyberC_plugin.HasPlayerStats.Find(s => s.Id == P1.Id);
                            if (Ply != null)
                            {
                                P1.Broadcast(8, $"<color=#3F9DFCA0>You hit {Ply.Hits} out of your {Ply.Shots} shots, an accuracy of {Ply.Percentage:0.##\\%}.</color>");
                            }
                        }
                    }
                    else
                    {
                        plyList.Sort((x, y) => y.Percentage.CompareTo(x.Percentage));
                        PlayerStatsClass BestPlayer = plyList.First();
                        Player bestPlayer = Player.Get(BestPlayer.Id);
                        string bestPlayerName;
                        if (bestPlayer == null)
                        {
                            bestPlayerName = BestPlayer.Name;
                        }
                        else
                        {
                            bestPlayerName = bestPlayer.DisplayNickname ?? bestPlayer.Nickname;
                        }
                        foreach (Player P1 in Player.List)
                        {
                            PlayerStatsClass Ply = XyberC_plugin.HasPlayerStats.Find(s => s.Id == P1.Id);
                            if (Ply == null)
                            {
                                P1.Broadcast(8, $"<color=#3F9DFCA0>The best player ({bestPlayerName}) hit {BestPlayer.Percentage:0.##\\%} out of {BestPlayer.Shots} shots.</color>");
                            }
                            else if (Ply.Id == BestPlayer.Id)
                            {
                                P1.Broadcast(12, $"<color=#3F9DFCA0>You hit {Ply.Hits} out of your {Ply.Shots} shots, an accuracy of {Ply.Percentage:0.##\\%}.\nYou are the most accurate player, congratulations!</color>");
                            }
                            else
                            {
                                P1.Broadcast(12, $"<color=#3F9DFCA0>You hit {Ply.Hits} out of your {Ply.Shots} shots, an accuracy of {Ply.Percentage:0.##\\%}.\nThe best player ({bestPlayerName}) hit {BestPlayer.Percentage:0.##\\%} out of {BestPlayer.Shots} shots.</color>");
                            }
                        }
                    }
                    response = "Displayed player stats";
                    return true;
                }
                else
                {
                    response = "No player stats have been recorded so far";
                    return true;
                }
            }
            /* if (arguments.At(0) == "icom" || arguments.At(0) == "i")
            {
                if (XyberC_plugin.HasPlayerStats.Any())
                {
                    string text = "<color=#3F9DFCA0>Current standings:</color>\n";
                    System.Collections.Generic.List<PlayerStatsClass> plyList = XyberC_plugin.HasPlayerStats.ToList();
                    foreach (PlayerStatsClass P1 in plyList)
                    {
                        P1.Percentage = 100.0f * P1.Hits / P1.Shots;
                    }
                    plyList.Sort((x, y) => y.Percentage.CompareTo(x.Percentage));
                    foreach (PlayerStatsClass Ply in plyList)
                    {
                        Player P1 = Player.Get(Ply.Id);
                        string playerName;
                        if (P1 == null)
                        {
                            playerName = Ply.Name;
                        }
                        else
                        {
                            playerName = Player.Get(P1.Id).DisplayNickname ?? Player.Get(P1.Id).Nickname;
                        }
                        text += $"{Ply.Percentage:0.##\\%}: {playerName} hit {Ply.Hits} out of {Ply.Shots}\n";
                    }
                    var x = Unity.FindObjectOfType(Intercom)
                    response = "Displaying standings on intercom";
                    return true;
                }
                else
                {
                    response = "No player stats have been recorded so far";
                    return true;
                }
            } */
            response = $"Unrecognized argument: {arguments.At(0)}";
            return false;
        }
    }
}