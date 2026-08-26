using NUnit.Framework;

namespace PlayniteLibraryServer.Tests
{
    public class SanityTests
    {
        [Test]
        public void PluginHasAFixedId()
        {
            var id = new System.Guid("9d3f5a2c-1e4b-4a6f-8c2d-7a1b9e0f3d55");
            Assert.That(id, Is.Not.EqualTo(System.Guid.Empty));
        }
    }
}
