using CommandSystem;
using System;
using Exiled.API.Features;

namespace XyberC_plugin.AdminGun
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class DamageOnMiss : ParentCommand
    {
        public DamageOnMiss() => LoadGeneratedCommands();

        public override string Command { get; } = "damageonmiss";

        public override string[] Aliases { get; } = new string[] { "missdamage", "damagemiss" };

        public override string Description { get; } = "Sets the damage dealt when missing shots to [amount]";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(((CommandSender)sender).SenderId);
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.RoundEvents))
            {
                response = "Permission denied.";
                return false;
            }
            if (arguments.Count != 1)
            {
                response = "Usage: \"damageonmiss [amount]\"";
                return false;
            }
            if (arguments.At(0) == "" || arguments.At(0) == "0")
            {
                foreach (Player Ply in Player.List)
                {
                    if (Ply.Team != Team.RIP && Ply.Team != Team.SCP && Ply.IsGodModeEnabled == false)
                    {
                        Ply.Broadcast(8, $"<color=#40FF4070>You will no longer take damage when you miss a shot.</color>");
                    }
                }
                response = "Disabled damage on missing";
                return true;
            }
            float damageonmiss = 0;
            try
            {
                damageonmiss += float.Parse(arguments.At(0));
            }
            catch
            {
                response = $"Not a valid number: {arguments.At(0)}";
                return false;
            }
            XyberC_plugin.MissDamage = damageonmiss;
            foreach (Player Ply in Player.List)
            {
                if (Ply.Team != Team.RIP && Ply.Team != Team.SCP && Ply.IsGodModeEnabled == false)
                {
                    Ply.Broadcast(8, $"<color=#FF404070>You will take {XyberC_plugin.MissDamage} damage every time you miss a shot!</color>");
                }
            }
            response = $"Set damage on missing to {damageonmiss}";
            return true;
        }
    }
}