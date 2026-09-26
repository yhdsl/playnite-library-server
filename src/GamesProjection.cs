using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayniteLibraryServer
{
    public static class GamesProjection
    {
        public static List<Dictionary<string, object>> ToJsonArray(
            IEnumerable<Game> games, IReadOnlyDictionary<Guid, string> sourceNames, bool includeHidden)
        {
            return games
                .Where(g => includeHidden || !g.Hidden)
                .Select(g => ToJson(g, sourceNames))
                .ToList();
        }

        private static Dictionary<string, object> ToJson(Game game, IReadOnlyDictionary<Guid, string> sourceNames)
        {
            sourceNames.TryGetValue(game.SourceId, out var sourceName);
            return new Dictionary<string, object>
            {
                ["id"] = game.Id.ToString(),
                ["name"] = game.Name,
                ["sortingName"] = game.SortingName,
                ["isInstalled"] = game.IsInstalled,
                ["hidden"] = game.Hidden,
                ["installDirectory"] = game.InstallDirectory,
                ["icon"] = game.Icon,
                ["coverImage"] = game.CoverImage,
                ["playtime"] = game.Playtime,
                ["lastActivity"] = game.LastActivity?.ToUniversalTime().ToString("o"),
                ["releaseDate"] = game.ReleaseDate,
                ["completionStatus"] = game.CompletionStatus,
                ["links"] = (game.Links ?? new System.Collections.ObjectModel.ObservableCollection<Link>())
                    .Select(l => new Dictionary<string, string> { ["name"] = l.Name, ["url"] = l.Url })
                    .ToList(),
                ["platforms"] = game.Platforms,
                ["source"] = sourceName,
                ["version"] = game.Version,
            };
        }
    }
}
