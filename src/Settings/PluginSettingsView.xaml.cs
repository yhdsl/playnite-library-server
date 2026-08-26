namespace PlayniteLibraryServer.Settings
{
    using System.Windows.Controls;

    public partial class PluginSettingsView : UserControl
    {
        public PluginSettingsView(PluginSettingsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
