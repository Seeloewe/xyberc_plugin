
using Exiled.API.Features;
using System;
using System.IO;
using System.Collections.Generic;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace XyberC_plugin
{
    public class XyberC_plugin : Plugin<Config>
    {
        private static readonly Lazy<XyberC_plugin> LazyInstance = new Lazy<XyberC_plugin>(valueFactory: () => new XyberC_plugin());
        public XyberC_plugin Instance => LazyInstance.Value;

        private Handlers.XyberC_plugin_PlayerH player;
        private Handlers.XyberC_plugin_ServerH server;

        public static List<AdminGunClass> HasAdminGun = new List<AdminGunClass>();
        public static float MissDamage = 0f;
        public static float HitDamage = 0f;
        public static string LogFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED", "Plugins", "XyberC_Logs");
        public static string LogFile1 = Path.Combine(LogFileLocation, "Log1.txt");
        public static string LogFile2 = Path.Combine(LogFileLocation, "Log2.txt");
        public static bool LogOther = false;
        public static List<PlayerStatsClass> HasPlayerStats = new List<PlayerStatsClass>();
        public static int ReplaceMeSCP = -1;
        public static RoleType ReplaceSCP = RoleType.None;
        public static float ReplaceSCPHP = 0f;
        public static float ReplaceSCPAHP = 0f;
        public static UnityEngine.Vector3 ReplaceSCPpos = UnityEngine.Vector3.zero;

        public static bool adminGun = false;
        public static bool missDamage = false;
        public static bool playerStats = false;

        public override string Name { get; } = "XyberC_plugin";
        public override string Author { get; } = "Seeloewe";
        public override string Prefix { get; } = "xyz";
        public override void OnEnabled()
        {
            Directory.CreateDirectory(LogFileLocation);

            player = new Handlers.XyberC_plugin_PlayerH();
            server = new Handlers.XyberC_plugin_ServerH();

            Player.Shooting += player.OnShooting;
            Player.Shot += player.OnShot;
            Player.Dying += player.OnDying;
            Player.Left += player.OnLeft;
            Player.DroppingItem += player.OnDroppingItem;
            Player.Spawning += player.OnSpawning;
            Player.ChangingRole += player.OnChangingRole;
            Player.Kicking += player.OnKicking;
            Player.Banning += player.OnBanning;
            Player.Handcuffing += player.OnHandcuffing;
            Server.RoundEnded += server.OnRoundEnded;
            Server.RestartingRound += server.OnRestartingRound;
            Server.SendingRemoteAdminCommand += server.OnSendingRemoteAdminCommand;
        }

        public override void OnDisabled()
        {
            Player.Shooting -= player.OnShooting;
            Player.Shot -= player.OnShot;
            Player.Dying -= player.OnDying;
            Player.Left -= player.OnLeft;
            Player.DroppingItem -= player.OnDroppingItem;
            Player.Spawning -= player.OnSpawning;
            Player.ChangingRole -= player.OnChangingRole;
            Player.Kicking -= player.OnKicking;
            Player.Banning -= player.OnBanning;
            Player.Handcuffing -= player.OnHandcuffing;
            Server.RoundEnded -= server.OnRoundEnded;
            Server.RestartingRound -= server.OnRestartingRound;
            Server.SendingRemoteAdminCommand -= server.OnSendingRemoteAdminCommand;

            player = null;
            server = null;
        }
    }
    public class XyberC_plugin_Stuff
    {

        public static void WriteToFile(string text)
        {
            if (XyberC_plugin.LogOther == false)
            {
                using (StreamWriter file = new StreamWriter(XyberC_plugin.LogFile1, true))
                {
                    file.WriteLine(text);
                }
            }
            else
            {
                using (StreamWriter file = new StreamWriter(XyberC_plugin.LogFile2, true))
                {
                    file.WriteLine(text);
                }
            }
        }
        public static void WriteToFile_Swap()
        {
            if (XyberC_plugin.LogOther == false)
            {
                XyberC_plugin.LogOther = true;
                using (StreamWriter file = new StreamWriter(XyberC_plugin.LogFile1, true))
                {
                    file.WriteLine("End");
                }
                File.Create(XyberC_plugin.LogFile2).Dispose();
            }
            else
            {
                XyberC_plugin.LogOther = false;
                using (StreamWriter file = new StreamWriter(XyberC_plugin.LogFile2, true))
                {
                    file.WriteLine("End");
                }
                File.Create(XyberC_plugin.LogFile1).Dispose();
            }
        }
        public static void WriteToFile_Mutes(string sender, string target, string name)
        {
            if (name == "mute")
            {
                WriteToFile($"{sender} muted {target}");
            }
            else if (name == "imute")
            {
                WriteToFile($"{sender} icom-muted {target}");
            }
        }
    }
    public class AdminGunClass
    {
        public int Id;
        public List<Inventory.SyncItemInfo> ReplacedItems;
        public List<string> Commands;
        /*public int AgunType;*/
    }
    public class PlayerStatsClass
    {
        public int Id;
        public string Name;
        public int Hits;
        public int Shots;
        public float Percentage;
    }
}
