using Exiled.Events.EventArgs;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;

namespace XyberC_plugin.Handlers
{
    class XyberC_plugin_ServerH
    {
        public void OnRestartingRound()
        {
            XyberC_plugin.HasAdminGun.Clear();
            XyberC_plugin.adminGun = false;
            XyberC_plugin.MissDamage = 0f;
            XyberC_plugin.HitDamage = 0f;
            XyberC_plugin.missDamage = false;
            XyberC_plugin.ReplaceSCP = RoleType.None;
            XyberC_plugin.ReplaceSCPHP = 0f;
            XyberC_plugin.ReplaceSCPAHP = 0f;
            XyberC_plugin.ReplaceSCPpos = UnityEngine.Vector3.zero;
            XyberC_plugin.HasPlayerStats.Clear();
            XyberC_plugin_Stuff.WriteToFile_Swap();
        }
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            if (XyberC_plugin.playerStats == true && XyberC_plugin.HasPlayerStats.Any())
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
            }
        }
        public void OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs ev)
        {
            if ((ev.Name.Equals("mute") || ev.Name.Equals("imute")) && ev.Success == true && ev.Arguments.Count != 0)
            {
                foreach (string arg in ev.Arguments[0].Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries))
                {
                    Player ply = Player.Get(arg);
                    XyberC_plugin_Stuff.WriteToFile_Mutes(ev.Sender.Nickname, ply != null ? ply.Nickname : "Server", ev.Name);
                }
            }
        }
        public void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            if ((ev.Name.Equals("mute") || ev.Name.Equals("imute")) && ev.Allow == true && ev.Arguments.Count != 0)
            {
                foreach (string arg in ev.Arguments[0].Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries))
                {
                    Player ply = Player.Get(arg);
                    XyberC_plugin_Stuff.WriteToFile_Mutes(ev.Player.Nickname, ply != null ? ply.Nickname : "Server", ev.Name);
                }
            }
        }
    }
}