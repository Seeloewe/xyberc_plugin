using Exiled.Events.EventArgs;
using Exiled.API.Features;
using System.Linq;
using System;
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
            if (XyberC_plugin.playerStats == true || XyberC_plugin.missDamage == true)
            {
                Player player = Player.Get(ev.Target);
                if (player == null || (player.Team == ev.Shooter.Team && ev.Shooter.IsFriendlyFireEnabled == false) || player.IsGodModeEnabled == true)
                {
                    if (XyberC_plugin.MissDamage != 0 && ev.Shooter.IsGodModeEnabled == false)
                    {
                        if (XyberC_plugin.MissDamage > 0)
                        {
                            ev.Shooter.Hurt(XyberC_plugin.MissDamage, ev.Shooter);
                        }
                        else
                        {
                            float hp = ev.Shooter.Health;
                            hp -= XyberC_plugin.MissDamage;
                            ev.Shooter.Health = hp;
                        }
                    }
                    if (XyberC_plugin.playerStats == true)
                    {
                        int index = XyberC_plugin.HasPlayerStats.FindIndex(p => p.Id == ev.Shooter.Id);
                        if (index == -1)
                        {
                            XyberC_plugin.HasPlayerStats.Add(new PlayerStatsClass
                            {
                                Id = ev.Shooter.Id,
                                Name = Player.Get(ev.Shooter.Id).Nickname,
                                Shots = 1,
                                Hits = 0
                            });
                        }
                        else
                        {
                            XyberC_plugin.HasPlayerStats[index].Shots += 1;
                        }
                    }
                }
                else if (XyberC_plugin.playerStats == true)
                {
                    int index = XyberC_plugin.HasPlayerStats.FindIndex(p => p.Id == ev.Shooter.Id);
                    if (index == -1)
                    {
                        XyberC_plugin.HasPlayerStats.Add(new PlayerStatsClass
                        {
                            Id = ev.Shooter.Id,
                            Shots = 1,
                            Hits = 1
                        });
                    }
                    else
                    {
                        XyberC_plugin.HasPlayerStats[index].Shots += 1;
                        XyberC_plugin.HasPlayerStats[index].Hits += 1;
                    }
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
            if (ev.Player.Team == Team.SCP)
            {
                if (ev.Player.Role == RoleType.Scp0492 )
                {
                    foreach (Player Pl in Player.List)
                    {
                        if (Pl.ReferenceHub.serverRoles.RemoteAdmin)
                        {
                            Pl.Broadcast(8, $"{ev.Player.Nickname} ({ev.Player.Id}) left as SCP-049-2 (cannot revive)", Broadcast.BroadcastFlags.AdminChat);
                        }
                    }
                }
                else
                {
                    XyberC_plugin.ReplaceSCP = ev.Player.Role;
                    XyberC_plugin.ReplaceSCPHP = ev.Player.Health;
                    XyberC_plugin.ReplaceSCPAHP = ev.Player.AdrenalineHealth;
                    foreach (Player Pl in Player.List)
                    {
                        if (Pl.ReferenceHub.serverRoles.RemoteAdmin)
                        {
                            Pl.Broadcast(8, $"{ev.Player.Nickname} ({ev.Player.Id}) left as {ev.Player.Role}, revive using \"respawn\" command", Broadcast.BroadcastFlags.AdminChat);
                        }
                    }
                }
            }
            if (XyberC_plugin.HasAdminGun.Any(s => s.Id == ev.Player.Id))
            {
                if (Server.FriendlyFire == false)
                {
                    ev.Player.IsFriendlyFireEnabled = false;
                }
                XyberC_plugin.HasAdminGun.Remove(XyberC_plugin.HasAdminGun.Find(p => p.Id == ev.Player.Id));
            }
        }
        public void OnShot(ShotEventArgs ev)
        {
            if (XyberC_plugin.adminGun == true && ev.Shooter.CurrentItem.id == ItemType.GunCOM15 && XyberC_plugin.HasAdminGun.Any(s => s.Id == ev.Shooter.Id))
            {
                Exiled.API.Extensions.Item.SetWeaponAmmo(ev.Shooter, ev.Shooter.CurrentItem, 12);
                ev.CanHurt = false;
                Player player = Player.Get(ev.Target);
                if (player != null)
                {
                    AdminGunClass gun = XyberC_plugin.HasAdminGun.Find(s => s.Id == ev.Shooter.Id);
                    foreach (string s in gun.Commands)
                    {
                        string command = s.Replace("#", $"{player.Id}");
                        command = command.Replace("$", $"{player.Health:0}");
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
            if (XyberC_plugin.missDamage == true && XyberC_plugin.HitDamage != 0 && ev.CanHurt == true && ev.Shooter.IsGodModeEnabled == false)
            {
                if (XyberC_plugin.HitDamage > 0)
                {
                    ev.Shooter.Hurt(XyberC_plugin.HitDamage, ev.Shooter);
                }
                else
                {
                    float hp = ev.Shooter.Health;
                    hp -= XyberC_plugin.HitDamage;
                    ev.Shooter.Health = hp;
                }
            }
        }
        public void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (XyberC_plugin.adminGun == true && ev.Item.id == ItemType.GunCOM15 && XyberC_plugin.HasAdminGun.Any(s => s.Id == ev.Player.Id))
            {
                ev.IsAllowed = false;
            }
        }
        public void OnSpawning(SpawningEventArgs ev)
        {
            if (XyberC_plugin.missDamage == true && ev.Player.Team != Team.RIP && ev.Player.Team != Team.SCP && ev.Player.IsGodModeEnabled == false)
            {
                string text1 = "";
                string text2 = "";
                if (XyberC_plugin.MissDamage < 0)
                {
                    text1 = $"<color=#40FF40A0>You will be healed by {-XyberC_plugin.MissDamage} HP when you miss a shot!</color>";
                }
                else if (XyberC_plugin.MissDamage > 0)
                {
                    text1 = $"<color=#FF4040A0>You will take {XyberC_plugin.MissDamage} HP damage when you miss a shot!</color>";
                }
                if (XyberC_plugin.HitDamage < 0)
                {
                    text2 = $"<color=#40FF40A0>You will be healed by {-XyberC_plugin.HitDamage} HP when you hit a shot!</color>";
                }
                else if (XyberC_plugin.HitDamage > 0)
                {
                    text2 = $"<color=#FF4040A0>You will take {XyberC_plugin.HitDamage} HP damage when you hit a shot!</color>";
                }
                ev.Player.Broadcast(8, $"{(text1 == "" ? "" : $"{text1}\n")}{(text2 == "" ? "" : $"{text2}")}");
            }
        }
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player.IsOverwatchEnabled == true)
            {
                ev.NewRole = RoleType.Spectator;
            }
            if (XyberC_plugin.adminGun == true && ev.NewRole.GetTeam() != Team.RIP && ev.NewRole.GetTeam() != Team.SCP && XyberC_plugin.HasAdminGun.Any(s => s.Id == ev.Player.Id))
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
        public void OnKicking(KickingEventArgs ev)
        {
            XyberC_plugin_Stuff.WriteToFile($"{ev.Issuer.Nickname} kicked {ev.Target.Nickname} for {ev.Reason}");
        }
        public void OnBanning(BanningEventArgs ev)
        {
            TimeSpan duration = TimeSpan.FromSeconds(ev.Duration);
            DateTime until = DateTime.Now + duration;
            XyberC_plugin_Stuff.WriteToFile($"{ev.Issuer.Nickname} banned {ev.Target.Nickname} for {duration} (until {until}) for {ev.Reason}");
        }
        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            if (ev.Cuffer.Team == ev.Target.Team)
            {
                ev.IsAllowed = false;
            }
        }
    }
}
