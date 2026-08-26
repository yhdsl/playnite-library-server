using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;

namespace PlayniteLibraryServer
{
    public static class RequestHandler
    {
        private static readonly JavaScriptSerializer Json = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };

        public static (int StatusCode, string Body) Handle(
            string path, NameValueCollection query, IEnumerable<Game> games, IReadOnlyDictionary<Guid, string> sourceNames)
        {
            if (path == "/games")
            {
                var includeHidden = query?["hidden"] == "true";
                var payload = GamesProjection.ToJsonArray(games, sourceNames, includeHidden);
                return (200, Json.Serialize(payload));
            }

            return (404, Json.Serialize(new Dictionary<string, string> { ["error"] = "Not found" }));
        }
    }
}
