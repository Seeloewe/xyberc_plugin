using Exiled.Events.EventArgs;
using Exiled.API.Features;

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