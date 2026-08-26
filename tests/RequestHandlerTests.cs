using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using NUnit.Framework;
using Playnite.SDK.Models;

namespace PlayniteLibraryServer.Tests
{
    public class RequestHandlerTests
    {
        private static readonly Dictionary<Guid, string> NoSources = new Dictionary<Guid, string>();

        private static Game MakeGame(string name) => new Game { Id = Guid.NewGuid(), Name = name };

        [Test]
        public void GetGamesReturns200WithJsonArray()
        {
            var games = new[] { MakeGame("Doom") };
            var (status, body) = RequestHandler.Handle("/games", new NameValueCollection(), games, NoSources);

            Assert.That(status, Is.EqualTo(200));
            var parsed = (object[])new JavaScriptSerializer().DeserializeObject(body);
            Assert.That(parsed, Has.Length.EqualTo(1));
        }

        [Test]
        public void UnknownPathReturns404()
        {
            var (status, body) = RequestHandler.Handle("/nope", new NameValueCollection(), new Game[0], NoSources);

            Assert.That(status, Is.EqualTo(404));
            StringAssert.Contains("Not found", body);
        }

        [Test]
        public void HiddenQueryParamIncludesHiddenGames()
        {
            var games = new[] { new Game { Id = Guid.NewGuid(), Name = "Secret", Hidden = true } };
            var query = new NameValueCollection { { "hidden", "true" } };

            var (status, body) = RequestHandler.Handle("/games", query, games, NoSources);

            Assert.That(status, Is.EqualTo(200));
            var parsed = (object[])new JavaScriptSerializer().DeserializeObject(body);
            Assert.That(parsed, Has.Length.EqualTo(1));
        }
    }
}
