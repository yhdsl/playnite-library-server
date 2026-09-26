namespace PlayniteLibraryServer.Settings
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class PluginSettingsView : UserControl
    {
        public PluginSettingsView(PluginSettingsViewModel viewModel)
        {
            DataContext = viewModel;

            var title = new TextBlock
            {
                Text = "Playnite Library Server",
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 12),
            };
            title.SetResourceReference(TextBlock.FontSizeProperty, "FontSizeLarger");

            var description = new TextBlock
            {
                Text = "在本地主机 (localhost) 上托管 Playnite 的游戏库，以便宇其他软件"
                    + "(例如 Flow Launcher 的 Playnite 插件) 无需直接读取 games.db 即可获取游戏信息"
                    + "在更改端口后，请重启 Playnite。",
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 12),
            };
            description.SetResourceReference(TextBlock.ForegroundProperty, "TextBrushDarker");

            var portLabel = new TextBlock
            {
                Text = "端口:",
                VerticalAlignment = VerticalAlignment.Center,
            };
            portLabel.SetResourceReference(TextBlock.ForegroundProperty, "TextBrush");

            var portBox = new TextBox { Padding = new Thickness(8, 6, 8, 6) };
            portBox.SetResourceReference(TextBox.ForegroundProperty, "TextBrush");
            portBox.SetBinding(TextBox.TextProperty, new Binding("Settings.Port") { Mode = BindingMode.TwoWay });

            var portRow = new Grid();
            portRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
            portRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            Grid.SetColumn(portLabel, 0);
            Grid.SetColumn(portBox, 1);
            portRow.Children.Add(portLabel);
            portRow.Children.Add(portBox);

            var panel = new StackPanel { Margin = new Thickness(16) };
            panel.Children.Add(title);
            panel.Children.Add(description);
            panel.Children.Add(portRow);
            Content = panel;
        }
    }
}
