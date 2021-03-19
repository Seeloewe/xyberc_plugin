using Exiled.API.Interfaces;

namespace XyberC_plugin
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string WelcomeMessage { get; private set; } = "Insert welcome message here, \"%name\" will be replaced by the player's name";
        public ushort WelcomeMessageDur { get; private set; } = 8;
    }
}