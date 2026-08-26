using NUnit.Framework;

namespace PlayniteLibraryServer.Tests
{
    public class PluginSettingsTests
    {
        [Test]
        public void DefaultsPortTo38217()
        {
            var settings = new Settings.PluginSettings();
            Assert.That(settings.Port, Is.EqualTo(38217));
        }

        [Test]
        public void PortIsSettable()
        {
            var settings = new Settings.PluginSettings { Port = 40000 };
            Assert.That(settings.Port, Is.EqualTo(40000));
        }
    }
}
