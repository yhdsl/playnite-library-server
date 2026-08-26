using Playnite.SDK;
using Playnite.SDK.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace PlayniteLibraryServer
{
    public class LibraryHttpServer
    {
        private static readonly ILogger Logger = LogManager.GetLogger();

        private readonly int _port;
        private readonly Func<IEnumerable<Game>> _getGames;
        private readonly Func<IReadOnlyDictionary<Guid, string>> _getSourceNames;
        private HttpListener _listener;
        private Thread _thread;

        public LibraryHttpServer(int port, Func<IEnumerable<Game>> getGames, Func<IReadOnlyDictionary<Guid, string>> getSourceNames)
        {
            _port = port;
            _getGames = getGames;
            _getSourceNames = getSourceNames;
        }

        public void Start()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://localhost:{_port}/");
            _listener.Start();
            _thread = new Thread(Loop) { IsBackground = true };
            _thread.Start();
        }

        public void Stop()
        {
            try { _listener?.Stop(); _listener?.Close(); } catch { }
        }

        private void Loop()
        {
            while (_listener?.IsListening == true)
            {
                try
                {
                    var ctx = _listener.GetContext();
                    ThreadPool.QueueUserWorkItem(_ => HandleRequest(ctx));
                }
                catch (HttpListenerException) { break; }
                catch (ObjectDisposedException) { break; }
                catch (Exception ex) { Logger.Error(ex, "playnite-library-server request loop error"); }
            }
        }

        private void HandleRequest(HttpListenerContext ctx)
        {
            try
            {
                var (status, body) = RequestHandler.Handle(
                    ctx.Request.Url.AbsolutePath, ctx.Request.QueryString, _getGames(), _getSourceNames());
                ctx.Response.StatusCode = status;
                ctx.Response.ContentType = "application/json; charset=utf-8";
                var buffer = Encoding.UTF8.GetBytes(body);
                ctx.Response.ContentLength64 = buffer.Length;
                ctx.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "playnite-library-server request handling error");
                ctx.Response.StatusCode = 500;
            }
            finally
            {
                try { ctx.Response.OutputStream.Close(); } catch { }
            }
        }
    }
}
