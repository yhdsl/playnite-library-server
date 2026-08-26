using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;
using Playnite.SDK.Models;

namespace PlayniteLibraryServer.Tests
{
    public class GamesProjectionTests
    {
        private static readonly Guid SteamId = Guid.NewGuid();

        private static readonly Dictionary<Guid, string> SourceNames = new Dictionary<Guid, string>
        {
            [SteamId] = "Steam",
        };

        private static Game MakeGame(string name, bool hidden = false)
        {
            return new Game
            {
                Id = Guid.NewGuid(),
                Name = name,
                Hidden = hidden,
                Icon = "033b6530\\icon.ico",
                CoverImage = "033b6530\\cover.jpg",
                InstallDirectory = @"F:\Games\Doom",
                LastActivity = new DateTime(2026, 3, 20, 15, 30, 0, DateTimeKind.Utc),
                Links = new ObservableCollection<Link> { new Link("Steam", "https://store.steampowered.com/app/2280") },
                IsInstalled = true,
                Playtime = 3600,
                SourceId = SteamId,
            };
        }

        [Test]
        public void ProjectsAllExpectedFields()
        {
            var game = MakeGame("Doom");
            var result = GamesProjection.ToJsonArray(new[] { game }, SourceNames, includeHidden: false);

            Assert.That(result, Has.Count.EqualTo(1));
            var doc = result[0];
            Assert.That(doc["id"], Is.EqualTo(game.Id.ToString()));
            Assert.That(doc["name"], Is.EqualTo("Doom"));
            Assert.That(doc["isInstalled"], Is.EqualTo(true));
            Assert.That(doc["hidden"], Is.EqualTo(false));
            Assert.That(doc["installDirectory"], Is.EqualTo(@"F:\Games\Doom"));
            Assert.That(doc["icon"], Is.EqualTo("033b6530\\icon.ico"));
            Assert.That(doc["coverImage"], Is.EqualTo("033b6530\\cover.jpg"));
            Assert.That(doc["playtime"], Is.EqualTo(3600UL));
            Assert.That(doc["lastActivity"], Is.EqualTo("2026-03-20T15:30:00.0000000Z"));
            Assert.That(doc["source"], Is.EqualTo("Steam"));

            var links = (List<Dictionary<string, string>>)doc["links"];
            Assert.That(links, Has.Count.EqualTo(1));
            Assert.That(links[0]["name"], Is.EqualTo("Steam"));
            Assert.That(links[0]["url"], Is.EqualTo("https://store.steampowered.com/app/2280"));
        }

        [Test]
        public void OmitsHiddenGamesByDefault()
        {
            var visible = MakeGame("Doom");
            var hidden = MakeGame("Secret Game", hidden: true);

            var result = GamesProjection.ToJsonArray(new[] { visible, hidden }, SourceNames, includeHidden: false);

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0]["name"], Is.EqualTo("Doom"));
        }

        [Test]
        public void IncludesHiddenGamesWhenRequested()
        {
            var hidden = MakeGame("Secret Game", hidden: true);

            var result = GamesProjection.ToJsonArray(new[] { hidden }, SourceNames, includeHidden: true);

            Assert.That(result, Has.Count.EqualTo(1));
        }

        [Test]
        public void UnknownSourceIdAndNullLastActivityBecomeNull()
        {
            var game = MakeGame("Doom");
            game.SourceId = Guid.NewGuid(); // not present in SourceNames
            game.LastActivity = null;

            var result = GamesProjection.ToJsonArray(new[] { game }, SourceNames, includeHidden: false);

            Assert.That(result[0]["source"], Is.Null);
            Assert.That(result[0]["lastActivity"], Is.Null);
        }
    }
}
