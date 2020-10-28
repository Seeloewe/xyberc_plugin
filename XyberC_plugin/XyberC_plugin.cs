
using Exiled.API.Features;
using Exiled.Events;
using System;
using System.Collections.Generic;
using Player = Exiled.Events.Handlers.Player;

namespace XyberC_plugin
{
    public class XyberC_plugin : Plugin<Config>
    {
        private static readonly Lazy<XyberC_plugin> LazyInstance = new Lazy<XyberC_plugin>(valueFactory: () => new XyberC_plugin());
        public XyberC_plugin Instance => LazyInstance.Value;

        private Handlers.XyberC_plugin_H player;
        public static List<AdminGunClass> HasAdminGun = new List<AdminGunClass>();

        public override string Name { get; } = "XyberC_plugin";
        public override string Author { get; } = "Seeloewe";
        public override void OnEnabled()
        {
            player = new Handlers.XyberC_plugin_H();
            Player.Shooting += player.OnShooting;
            Player.Dying += player.OnDying;
        }

        public override void OnDisabled()
        {
            Player.Shooting -= player.OnShooting;
            Player.Dying -= player.OnDying;
            player = null;
        }
    }
    public class AdminGunClass
    {
        public string Userid;
        public string ReplacedItem;
        public string Command;
    }
}
