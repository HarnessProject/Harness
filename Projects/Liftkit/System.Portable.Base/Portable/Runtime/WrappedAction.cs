namespace System.Portable.Runtime
{
    public delegate void WrappedAction(object tObject);

    public delegate TY WrappedResult<out TY>();
    public delegate TY WrappedResult<in T, out TY>(T obj);
}