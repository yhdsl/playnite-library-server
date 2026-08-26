using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Plugins;
using System;

namespace PlayniteLibraryServer
{
    public class PlayniteLibraryServerPlugin : GenericPlugin
    {
        private static readonly ILogger Logger = LogManager.GetLogger();

        public override Guid Id => Guid.Parse("9d3f5a2c-1e4b-4a6f-8c2d-7a1b9e0f3d55");

        public PlayniteLibraryServerPlugin(IPlayniteAPI api) : base(api)
        {
            Properties = new GenericPluginProperties { HasSettings = false };
        }

        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            Logger.Info("Playnite Library Server starting.");
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
