#r ".\\debug\\System.Portable.Base.dll"
#r ".\\debug\\Harness.Net.dll"

using System.Portable.Runtime;
using System.Reflection;


public class DependencyClass : IDependency {}

var types = TypeProvider.Instance;

Console.WriteLine("Assemblies Loaded          			:" + AppDomain.CurrentDomain.GetAssemblies().Count());
Console.WriteLine("Types Loaded               			:" + AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic).SelectMany(x => x.ExportedTypes).Count());
Console.WriteLine("Type Provider Types Loaded (No Cache):" + types.GetTypes().Count());
Console.WriteLine("Type Provider Types Loaded (Cached)  :" + types.Types);

