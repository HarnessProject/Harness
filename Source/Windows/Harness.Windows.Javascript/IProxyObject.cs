namespace Harness.Windows {
    public interface IProxyObject {
        string[] Properties { get; }
        string[] Fields { get; }
        string[] Methods { get; set; }
        string GetField(string name);
        bool SetField(string name, string newValue);
        string GetProperty(string name);
        bool SetProperty(string name, string newValue);
        string InvokeMethod(string name, string parameters);
    }
}