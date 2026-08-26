using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Plugins;
using PlayniteLibraryServer.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PlayniteLibraryServer
{
    public class PlayniteLibraryServerPlugin : GenericPlugin
    {
        private static readonly ILogger Logger = LogManager.GetLogger();

        private readonly PluginSettingsViewModel _settingsViewModel;
        private LibraryHttpServer _server;

        public override Guid Id => Guid.Parse("9d3f5a2c-1e4b-4a6f-8c2d-7a1b9e0f3d55");

        public PlayniteLibraryServerPlugin(IPlayniteAPI api) : base(api)
        {
            _settingsViewModel = new PluginSettingsViewModel(this);
            Properties = new GenericPluginProperties { HasSettings = true };
        }

        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            var port = _settingsViewModel.Settings.Port;
            _server = new LibraryHttpServer(
                port,
                () => PlayniteApi.Database.Games,
                () => PlayniteApi.Database.Sources.ToDictionary(s => s.Id, s => s.Name));
            _server.Start();
            Logger.Info($"Playnite Library Server listening on http://localhost:{port}/");
        }

        public override void Dispose()
        {
            _server?.Stop();
            base.Dispose();
        }

        public override ISettings GetSettings(bool firstRunSettings) => _settingsViewModel;

        public override UserControl GetSettingsView(bool firstRunSettings) => new PluginSettingsView(_settingsViewModel);
    }
}
