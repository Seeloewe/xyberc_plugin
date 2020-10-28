using CommandSystem;
using System;
using Exiled.API.Features;
using System.Linq;

namespace XyberC_plugin.AdminGun
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class AdminGun : ParentCommand
    {
        public AdminGun() => LoadGeneratedCommands();

        public override string Command { get; } = "admingun";

        public override string[] Aliases { get; } = new string[] { "agun" };

        public override string Description { get; } = "Gives/removes the Admin Gun with the given command";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            ///EventHandlers.LogCommandUsed((CommandSender)sender, EventHandlers.FormatArguments(arguments, 0));
            Player ply = Player.Get(((CommandSender)sender).SenderId);
            if (!ply.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "Permission denied.";
                return false;
            }

            if (arguments.Count < 1)
            {
                if (XyberC_plugin.HasAdminGun.Any(s => s.Userid == ply.UserId))
                {
                    XyberC_plugin.HasAdminGun.Remove(XyberC_plugin.HasAdminGun.Find(p => p.Userid == ply.UserId));
                    response = "Admin Gun disabled";
                    return true;
                }
                else
                {
                    response = "Usage: agun [command] [arguments]; \"%\" is replaced by the target's ID";
                    return false;
                }
            }

            else
            {
                if (XyberC_plugin.HasAdminGun.Any(s => s.Userid == ply.UserId))
                {
                    XyberC_plugin.HasAdminGun.Remove(XyberC_plugin.HasAdminGun.Find(p => p.Userid == ply.UserId));
                }

                string command = string.Join(" ", arguments);
                XyberC_plugin.HasAdminGun.Add(new AdminGunClass
                {
                    Userid = ply.UserId,
                    /* ReplacedItem = "null",*/
                    Command = command,
                });
                response = $"You have selected the \"{command}\" command";
                return true;
            }
        }
    }
}