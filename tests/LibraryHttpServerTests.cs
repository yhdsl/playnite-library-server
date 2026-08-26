using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Playnite.SDK.Models;

namespace PlayniteLibraryServer.Tests
{
    public class LibraryHttpServerTests
    {
        [Test]
        public async Task ServesGamesOverHttp()
        {
            var port = 48217; // distinct from the real default, avoids clashing with a running instance
            var noSources = new Dictionary<Guid, string>();
            var server = new LibraryHttpServer(
                port,
                () => new[] { new Game { Id = Guid.NewGuid(), Name = "Doom" } },
                () => noSources);
            server.Start();
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"http://localhost:{port}/games");
                    Assert.That((int)response.StatusCode, Is.EqualTo(200));
                    var body = await response.Content.ReadAsStringAsync();
                    StringAssert.Contains("Doom", body);
                }
            }
            finally
            {
                server.Stop();
            }
        }
    }
}
