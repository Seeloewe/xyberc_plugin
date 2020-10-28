using Exiled.Events.EventArgs;
using Exiled.API.Features;
using System.Linq;

namespace XyberC_plugin.Handlers
{
    class XyberC_plugin_H
    {
        public void OnShooting(ShootingEventArgs ev)
        {
            if (ev.Shooter.IsGodModeEnabled && ev.Shooter.Role != RoleType.Tutorial)
            {
                ev.Shooter.IsGodModeEnabled = false;
                Log.Info($"{ev.Shooter.Nickname} got godmode removed");
            }
            if (XyberC_plugin.HasAdminGun.Any(s => s.Userid == ev.Shooter.UserId))
            {
                Log.Info("Admin Gun has been used");
                Player player = Player.Get(ev.Target);
                if(player != null)
                {
                    Log.Info($"on {player.Nickname}");
                    AdminGunClass gun = XyberC_plugin.HasAdminGun.Find(s => s.Userid == ev.Shooter.UserId);
                    Log.Info($"doing \"{gun.Command.Replace("%", $"{player.Id}")}\"");
                    Log.Info($"doing \"{gun.Command.Replace("%", $"{player.Id}")}\"");
                    GameCore.Console.singleton.TypeCommand($"{gun.Command.Replace("%", $"{player.Id}")}", player.Sender);
                }
            }
        }
        public void OnDying(DyingEventArgs ev)
        {
            if (ev.Target.IsCuffed && (ev.Killer.Side == Exiled.API.Enums.Side.Mtf || ev.Killer.Side == Exiled.API.Enums.Side.ChaosInsurgency))
            {
                foreach (Player Pl in Player.List)
                {
                    if (Pl.ReferenceHub.serverRoles.RemoteAdmin)
                    {
                        Pl.Broadcast(10, $"{ev.Killer.Nickname} ({ev.Killer.Id}) killed {ev.Target.Nickname} ({ev.Target.Id}) while cuffed", Broadcast.BroadcastFlags.AdminChat);
                    }
                }
            }
        }
    }
}
