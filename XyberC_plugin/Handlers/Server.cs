using Exiled.Events.EventArgs;
using Exiled.API.Features;
using System.Linq;

namespace XyberC_plugin.Handlers
{
    class XyberC_plugin_ServerH
    {
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            XyberC_plugin.HasAdminGun.Clear();
        }
    }
}