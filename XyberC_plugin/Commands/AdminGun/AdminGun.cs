using CommandSystem;
using System;
using Exiled.API.Features;
using System.Linq;
using System.Collections.Generic;

namespace XyberC_plugin.AdminGun
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class AdminGun : ParentCommand
    {
        public AdminGun() => LoadGeneratedCommands();

        public override string Command { get; } = "admingun";

        public override string[] Aliases { get; } = new string[] { "agun" };

        public override string Description { get; } = "Admin Gun that does command(s)";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(((CommandSender)sender).SenderId);
            if (!ply.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "Permission denied.";
                return false;
            }
            if (ply.Team == Team.RIP || ply.Team == Team.SCP || ply.IsCuffed == true)
            {
                response = "You cannot use the admin gun in this state.";
                return false;
            }
            if (arguments.Count < 1)
            {
                if (XyberC_plugin.HasAdminGun.Any(s => s.Id == ply.Id))
                {
                    if (Server.FriendlyFire == false)
                    {
                        ply.IsFriendlyFireEnabled = false;
                    }
                    ply.ResetInventory(XyberC_plugin.HasAdminGun.Find(s => s.Id == ply.Id).ReplacedItems);
                    XyberC_plugin.HasAdminGun.RemoveAll(p => p.Id == ply.Id);

                    if (!XyberC_plugin.HasAdminGun.Any())
                    {
                        XyberC_plugin.adminGun = false;
                    }
                    response = "Admin Gun disabled";
                    return true;
                }
                else
                {
                    /*response = "Usage: \"agun [id] [command] [arguments]\"; replacing: \"#\" > ID, \"@\" > Name, \"$\" > Health; use \"&\" to separate multiple commands, [id] can be left out";*/
                    response = "Usage: \"agun [command] [arguments]\"; replacing: \"#\" > ID, \"@\" > Name, \"$\" > Health; use \"&\" to separate multiple commands";
                    return false;
                }
            }
            /*int aguntype = 0;*/
            string[] argument;
            /*if (arguments.At(0) == "all")
            {
                aguntype = -1;
                argument = arguments.Skip(1).ToArray();
            }
            else if (int.TryParse(arguments.At(0), out int p) == true && p > 0)
            {
                aguntype = p;
                argument = arguments.Skip(1).ToArray();
            }
            else
            {
                argument = arguments.ToArray();
            }*/
            argument = arguments.ToArray();

            string command = string.Join(" ", argument);
            command = command.Replace(" & ", "&");
            List<string> commands = new List<string>(command.Split('&'));
            command = command.Replace("&", " & ");
            if (XyberC_plugin.HasAdminGun.Any(s => s.Id == ply.Id))
            {
                int index = XyberC_plugin.HasAdminGun.FindIndex(p => p.Id == ply.Id);
                XyberC_plugin.HasAdminGun[index].Commands = commands;
            }
            else
            {
                List<Inventory.SyncItemInfo> items = new List<Inventory.SyncItemInfo>();
                foreach (Inventory.SyncItemInfo item in ply.Inventory.items)
                {
                    items.Add(item);
                }
                XyberC_plugin.HasAdminGun.Add(new AdminGunClass
                {
                    Id = ply.Id,
                    ReplacedItems = items,
                    Commands = commands,
                    /*AgunType = aguntype,*/
                });
                ply.Inventory.Clear();
                if (Server.FriendlyFire == false)
                {
                    ply.IsFriendlyFireEnabled = true;
                }
                ply.Inventory.AddNewItem(ItemType.GunCOM15);
                ply.Inventory.AddNewItem(ItemType.GunCOM15);
                ply.Inventory.AddNewItem(ItemType.GunCOM15);
                ply.Inventory.AddNewItem(ItemType.GunCOM15);
                ply.Inventory.AddNewItem(ItemType.GunCOM15);
                ply.Inventory.AddNewItem(ItemType.GunCOM15);
                ply.Inventory.AddNewItem(ItemType.GunCOM15);
                ply.Inventory.AddNewItem(ItemType.GunCOM15);
            }
            XyberC_plugin.adminGun = true;
            response = $"You have selected: \"{command}\"";
            return true;
        }
    }
}