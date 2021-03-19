using CommandSystem;
using System;
using Exiled.API.Features;

namespace XyberC_plugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class DamageOnMiss : ParentCommand
    {
        public DamageOnMiss() => LoadGeneratedCommands();

        public override string Command { get; } = "damageonmiss";

        public override string[] Aliases { get; } = new string[] { "missdamage", "damagemiss" };

        public override string Description { get; } = "Sets damage taken by shooter when hitting/missing shots\n\"[amount]\" - Damage on missing\n\"[amount]\" - Damage on hitting";

        public override void LoadGeneratedCommands() { }

        public void BCOutput(string text1 = "", string text2 = "")
        {
            string text = $"{(text1 == "" ? "" : $"{text1}\n")}{(text2 == "" ? "" : $"{text2}")}";
            foreach (Player Ply in Player.List)
            {
                if (Ply.Team != Team.RIP && Ply.Team != Team.SCP && Ply.IsGodModeEnabled == false)
                {
                    Ply.Broadcast(8, text);
                }
            }
        }
        public string BCOutput_Col (string action, float oldnum, float newnum = 0)
        {
            if (newnum == 0)
            {
                if (oldnum == 0)
                {
                    return "";
                }
                else if (oldnum < 0)
                {
                    return $"<color=#40FF40A0>You will no longer be healed when you {action}</color>";
                }
                return $"<color=#40FF40A0>You will no longer take damage when you {action}</color>";
            }
            if (newnum < 0)
            {
                return $"<color=#40FF40A0>You will be healed by {-newnum} HP when you {action}!</color>";
            }
            return $"<color=#FF4040A0>You will take {newnum} HP damage when you {action}!</color>";
        }
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player ply = Player.Get(((CommandSender)sender).SenderId);
            if (!ply.ReferenceHub.serverRoles.RemoteAdmin)
            {
                response = "Permission denied.";
                return false;
            }
            if (arguments.Count > 2)
            {
                response = "Usage: \"damageonmiss [amount] [amount]\"";
                return false;
            }
            if (arguments.Count == 0)
            {
                BCOutput( BCOutput_Col("miss a shot", XyberC_plugin.MissDamage), BCOutput_Col("hit a shot", XyberC_plugin.HitDamage) );
                XyberC_plugin.MissDamage = 0;
                XyberC_plugin.HitDamage = 0;
                XyberC_plugin.missDamage = false;
                response = "Disabled damage on missing/hitting";
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
            if (arguments.Count == 2)
            {
                float damageonhit = 0;
                try
                {
                    damageonhit += float.Parse(arguments.At(1));
                }
                catch
                {
                    response = $"Not a valid number: {arguments.At(1)}";
                    return false;
                }

                BCOutput(BCOutput_Col("miss a shot", XyberC_plugin.MissDamage, damageonmiss), BCOutput_Col("hit a shot", XyberC_plugin.HitDamage, damageonhit));

                XyberC_plugin.MissDamage = damageonmiss;
                XyberC_plugin.HitDamage = damageonhit;
                if (damageonmiss == 0 && damageonhit == 0)
                {
                    XyberC_plugin.missDamage = false;
                }
                else
                {
                    XyberC_plugin.missDamage = true;
                }
                response = $"Set damage on missing to {damageonmiss}, damage on hitting to {damageonhit}";
                return true;
            }

            BCOutput(BCOutput_Col("miss a shot", XyberC_plugin.MissDamage, damageonmiss), "");

            XyberC_plugin.MissDamage = damageonmiss;
            if (damageonmiss == 0 && XyberC_plugin.HitDamage == 0)
            {
                XyberC_plugin.missDamage = false;
            }
            else
            {
                XyberC_plugin.missDamage = true;
            }
            response = $"Set damage on missing to {damageonmiss}";
            return true;
        }
    }
}