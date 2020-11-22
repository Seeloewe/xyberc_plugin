using Exiled.Events.EventArgs;
using Exiled.API.Features;
using System.Linq;
using Exiled.API.Extensions;

namespace XyberC_plugin.Handlers
{
    class XyberC_plugin_PlayerH
    {
        public void OnShooting(ShootingEventArgs ev)
        {
            if (ev.Shooter.IsGodModeEnabled && ev.Shooter.Role != RoleType.Tutorial)
            {
                ev.Shooter.IsGodModeEnabled = false;
            }
            if (XyberC_plugin.MissDamage != 0)
            {
                Player player = Player.Get(ev.Target);
                if ((player == null || player.IsGodModeEnabled == true) && ev.Shooter.IsGodModeEnabled == false)
                {
                    ev.Shooter.Hurt(XyberC_plugin.MissDamage, ev.Shooter);
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
                        Pl.Broadcast(8, $"{ev.Killer.Nickname} ({ev.Killer.Id}) killed {ev.Target.Nickname} ({ev.Target.Id}) while cuffed", Broadcast.BroadcastFlags.AdminChat);
                    }
                }
            }
        }
        public void OnLeft(LeftEventArgs ev)
        {
            if (XyberC_plugin.HasAdminGun.Any(s => s.Userid == ev.Player.UserId))
            {
                if (Server.FriendlyFire == false)
                {
                    ev.Player.IsFriendlyFireEnabled = false;
                }
                XyberC_plugin.HasAdminGun.Remove(XyberC_plugin.HasAdminGun.Find(p => p.Userid == ev.Player.UserId));
            }
        }
        public void OnShot(ShotEventArgs ev)
        {
            if (ev.Shooter.CurrentItem.id == ItemType.GunCOM15 && XyberC_plugin.HasAdminGun.Any(s => s.Userid == ev.Shooter.UserId))
            {
                Exiled.API.Extensions.Item.SetWeaponAmmo(ev.Shooter, ev.Shooter.CurrentItem, 1 + (int)ev.Shooter.CurrentItem.GetWeaponAmmo());
                ev.CanHurt = false;
                Player player = Player.Get(ev.Target);
                if (player != null)
                {
                    AdminGunClass gun = XyberC_plugin.HasAdminGun.Find(s => s.Userid == ev.Shooter.UserId);
                    foreach (string s in gun.Commands)
                    {
                        string command = s.Replace("#", $"{player.Id}");
                        command = command.Replace("$", $"{player.Health}");
                        if (player.DisplayNickname == null)
                        {
                            command = command.Replace("@", $"{player.Nickname}");
                        }
                        else
                        {
                            command = command.Replace("@", $"{player.DisplayNickname}");
                        }
                        GameCore.Console.singleton.TypeCommand($"{command}", ev.Shooter.Sender);
                    }
                }
            }
        }
        public void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (ev.Item.id == ItemType.GunCOM15 && XyberC_plugin.HasAdminGun.Any(s => s.Userid == ev.Player.UserId))
            {
                ev.IsAllowed = false;
            }
        }
        public void OnSpawning(SpawningEventArgs ev)
        {
            if (XyberC_plugin.MissDamage != 0 && ev.Player.Team != Team.RIP && ev.Player.Team != Team.SCP && ev.Player.IsGodModeEnabled == false)
            {
                ev.Player.Broadcast(8, $"<color=#FF404070>You will take {XyberC_plugin.MissDamage} damage every time you miss a shot!</color>");
            }
        }
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player.IsOverwatchEnabled == true)
            {
                ev.NewRole = RoleType.Spectator;
            }
            if (ev.NewRole.GetTeam() != Team.RIP && ev.NewRole.GetTeam() != Team.SCP && XyberC_plugin.HasAdminGun.Any(s => s.Userid == ev.Player.UserId))
            {
                ev.Items.Clear();
                ev.Items.Add(ItemType.GunCOM15);
                ev.Items.Add(ItemType.GunCOM15);
                ev.Items.Add(ItemType.GunCOM15);
                ev.Items.Add(ItemType.GunCOM15);
                ev.Items.Add(ItemType.GunCOM15);
                ev.Items.Add(ItemType.GunCOM15);
                ev.Items.Add(ItemType.GunCOM15);
                ev.Items.Add(ItemType.GunCOM15);
            }
        }
    }
}
