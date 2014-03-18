namespace System.Web.Owin.Server.Http
{
    public interface IHttpService {
        int ConnectionCount { get; }
        int CurrentMaxConnectionCount { get; }
        void Start();
    }
}
