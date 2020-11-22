
using Exiled.API.Features;
using System;
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
        public static float MissDamage = 0;

        public override string Name { get; } = "XyberC_plugin";
        public override string Author { get; } = "Seeloewe";
        public override string Prefix { get; } = "xyz";
        public override void OnEnabled()
        {
            player = new Handlers.XyberC_plugin_PlayerH();
            server = new Handlers.XyberC_plugin_ServerH();
            Player.Shooting += player.OnShooting;
            Player.Shot += player.OnShot;
            Player.Dying += player.OnDying;
            Player.Left += player.OnLeft;
            Player.DroppingItem += player.OnDroppingItem;
            Player.Spawning += player.OnSpawning;
            Player.ChangingRole += player.OnChangingRole;
            Server.RoundEnded += server.OnRoundEnded;
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
            Server.RoundEnded -= server.OnRoundEnded;
            player = null;
            server = null;
        }
    }
    public class AdminGunClass
    {
        public string Userid;
        public List<Inventory.SyncItemInfo> ReplacedItems;
        public List<string> Commands;
        public int AgunType;
    }
}
