namespace PlayniteLibraryServer.Settings
{
    using System.Collections.Generic;
    using Playnite.SDK;

    public class PluginSettings : ObservableObject
    {
        private int _port = 38217;

        public int Port
        {
            get => _port;
            set => SetValue(ref _port, value);
        }
    }

    public class PluginSettingsViewModel : ObservableObject, ISettings
    {
        private readonly PlayniteLibraryServerPlugin _plugin;
        private PluginSettings _settings;
        private PluginSettings _editingClone;

        public PluginSettings Settings
        {
            get => _settings;
            set => SetValue(ref _settings, value);
        }

        public PluginSettingsViewModel(PlayniteLibraryServerPlugin plugin)
        {
            _plugin = plugin;
            var saved = plugin.LoadPluginSettings<PluginSettings>();
            Settings = saved ?? new PluginSettings();
        }

        public void BeginEdit()
        {
            _editingClone = Playnite.SDK.Data.Serialization.GetClone(Settings);
        }

        public void CancelEdit()
        {
            Settings = _editingClone;
        }

        public void EndEdit()
        {
            _plugin.SavePluginSettings(Settings);
        }

        public bool VerifySettings(out List<string> errors)
        {
            errors = new List<string>();
            if (Settings.Port < 1024 || Settings.Port > 65535)
            {
                errors.Add("Port must be between 1024 and 65535.");
            }
            return errors.Count == 0;
        }
    }
}
