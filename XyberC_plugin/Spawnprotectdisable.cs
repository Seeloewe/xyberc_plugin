
using Exiled.API.Features;
using Exiled.Events;
using System;

using Player = Exiled.Events.Handlers.Player;

namespace Spawnprotectdisable
{
    public class Spawnprotectdisable : Plugin<Config>
    {
        private static readonly Lazy<Spawnprotectdisable> LazyInstance = new Lazy<Spawnprotectdisable>(valueFactory: () => new Spawnprotectdisable());
        public Spawnprotectdisable Instance => LazyInstance.Value;

        private Handlers.Spawnprotectdisable_H player;

        public override string Name { get; } = "Spawnprotectdisable";
        public override string Author { get; } = "Seeloewe";

        public override void OnEnabled()
        {
            player = new Handlers.Spawnprotectdisable_H();
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
}
