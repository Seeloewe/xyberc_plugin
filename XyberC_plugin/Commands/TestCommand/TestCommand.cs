using CommandSystem;
using System;
using Exiled.API.Features;

namespace XyberC_plugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class DoTestCommand : ParentCommand
    {
        public DoTestCommand() => LoadGeneratedCommands();

        public override string Command { get; } = "dotest";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Does a test";

        public override void LoadGeneratedCommands() { }
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "DISABLED";
            return false;
            Player ply = Player.Get(((CommandSender)sender).SenderId);
            try
            {

                XyberC_plugin.HasPlayerStats.Add(new PlayerStatsClass
                {
                    Id = 3,
                    Name = "Test3",
                    Hits = 15,
                    Shots = 25
                });
                XyberC_plugin.HasPlayerStats.Add(new PlayerStatsClass
                {
                    Id = 4,
                    Name = "Test4",
                    Hits = 5,
                    Shots = 36
                });
                XyberC_plugin.HasPlayerStats.Add(new PlayerStatsClass
                {
                    Id = 5,
                    Name = "Test5",
                    Hits = 10,
                    Shots = 30
                });

                PlayerStatsClass Ply = XyberC_plugin.HasPlayerStats.Find(s => s.Id == ply.Id);
                Ply.Shots += 50;
                Ply.Hits += 40;

                XyberC_plugin.ReplaceSCP = RoleType.ClassD;
                XyberC_plugin.ReplaceSCPHP = 5.0f;
                XyberC_plugin.ReplaceSCPAHP = 50.0f;
                XyberC_plugin.ReplaceSCPpos = ply.Position;
                XyberC_plugin.ReplaceMeSCP = ply.Id;

                response = "success";
                return true;
            }
            catch (Exception e)
            {
                Log.Info($"TestCommand:\n{e}");
                response = $"Exception:\n{e}";
                return false;
            }
        }
    }
}