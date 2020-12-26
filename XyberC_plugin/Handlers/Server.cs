using Exiled.Events.EventArgs;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XyberC_plugin.Handlers
{
    class XyberC_plugin_ServerH
    {
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            XyberC_plugin.HasAdminGun.Clear();
            XyberC_plugin.adminGun = false;
            XyberC_plugin.MissDamage = 0;
            XyberC_plugin.HitDamage = 0;
            XyberC_plugin.missDamage = false;
            XyberC_plugin_Write.WriteToFile_Swap();
            if (XyberC_plugin.playerStats == true)
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
            }
            XyberC_plugin.HasPlayerStats.Clear();
        }
        public void OnSendingRemoteAdminCommand(SendingRemoteAdminCommandEventArgs ev)
        {
            if ((ev.Name.Equals("mute") || ev.Name.Equals("imute")) && ev.Success == true && ev.Arguments.Count != 0)
            {
                foreach (string arg in ev.Arguments[0].Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries))
                {
                    Player ply = Player.Get(arg);
                    XyberC_plugin_Write.WriteToFile_Mutes(ev.Sender.Nickname, ply != null ? ply.Nickname : "Server", ev.Name);
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
                    XyberC_plugin_Write.WriteToFile_Mutes(ev.Player.Nickname, ply != null ? ply.Nickname : "Server", ev.Name);
                }
            }
        }
    }
}