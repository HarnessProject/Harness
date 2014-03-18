namespace System.Web.Owin.Server.Http
{
    public interface IHttpService : IDisposable {
        int ConnectionCount { get; }
        int CurrentMaxConnectionCount { get; }
        void Start();
        void Stop();
    }
}
