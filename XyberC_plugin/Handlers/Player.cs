using Exiled.Events.EventArgs;
using Exiled.API.Features;

namespace XyberC_plugin.Handlers
{
    class XyberC_plugin_H
    {
        public void OnShooting(ShootingEventArgs ev)
        {
            if (ev.Shooter.IsGodModeEnabled == true && ev.Shooter.Role != RoleType.Tutorial)
            {
                ev.Shooter.IsGodModeEnabled = false;
                Log.Info($"{ev.Shooter.Nickname} got godmode removed");
            }
        }
        public void OnDying(DyingEventArgs ev)
        {
            Log.Info($"Player died");
            if (ev.Target.IsCuffed == true)
            {
                Log.Info($"Player that died was cuffed");
            }
            if (ev.Killer.Side == Exiled.API.Enums.Side.Mtf)
            {
                Log.Info($"Player that killed was MTF or Scientist");
            }
            if (ev.Target.IsCuffed == true && (ev.Killer.Side == Exiled.API.Enums.Side.Mtf || ev.Killer.Side == Exiled.API.Enums.Side.ChaosInsurgency))
            {
                Log.Info($"Success");
                foreach (Player Pl in Player.List)
                {
                    Log.Info($"This should be here once per player");
                    if (Pl.ReferenceHub.serverRoles.RemoteAdmin)
                    {
                        Log.Info($"This should be here once per admin");
                        Pl.Broadcast(10, $"{ev.Killer.Nickname} ({ev.Killer.Id}) killed {ev.Target.Nickname} ({ev.Target.Id}) while cuffed", Broadcast.BroadcastFlags.AdminChat);
                    }
                }
            }
        }
    }
}
