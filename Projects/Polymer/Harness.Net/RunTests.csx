#r "System.Portable.Base.dll"
#r "Harness.Net.dll"

var typeProvider = new TypeProvider();

Console.WriteLn("Assemblies Loaded (AppDomain)		:"  + AppDomain.CurrentDomain.GetAssemblies().Count());
Console
