#Harness#

A framework for Composable applications everywhere .net runs.
Targets .Net 4.5, SL5, Windows 8, Windows Phone 8, iOS, and Android.
All under the Apache 2.0 License.

###WARNING: This project requires the Roslyn End User Preview###
Because C# 6 is fantastic.

(Also requires some NuGet packages: Microsoft Async, Rx, JSON.Net)

Picking up the declarative method style that started with LINQ, Harness is a framework for composing applications from rich expressions. 

Why not Try/Catch/Finally in a single expression:

```csharp
this.Try(x => {
  Some.Statements;
  return true;
}).Catch<InvalidOperationException>((x,ex) => true)
.Catch<Exception>((x,ex) => false)
.Act()
```
How about a cast that reads like one:

```csharp
this.AsType<IInterface>();
```
Making Strings more fun:

```csharp
"{0} World".WithParams("Hello");
"Some String".Match("REGEX");
"Some Other String".Matches("Regex").Each( ... );
```

Easily create Tasks:

```csharp
await this.AsTask(x => Some.LongRunning.Operation());
await SomeCollection.EachAsync( ... );
```
Compare assignable Types:
```csharp
this.Is<SomeType>(); // true or false
typeof(SomeType).Is<IInterface>();
```

How about a rich IOC Provider (implementing a variation of the CSLP):
```csharp
Provider.Get<IDependency>();
Provider.GetAll<IService>();
Provider.Get(someType).AsType<SomeType>()
```
(Currently we support Autofac, but there is a rich abstraction around IOC so we could support any IOC Container)

And a DomainProvider, which provides Type services even in PCLs:
```csharp
Provider.Domain.Types;
Provider.Domain.Assemblies;
Provider.Domain.GetDefault<T>();
Provider.Domain.GetDefault(typeof(T));
Provider.Domain.Cast<TY>(object)
```
**And So Much More!**

Do you Caliburn.Micro? We do to! Harness has a bootstrapper for the upcoming Caliburn.Micro 2.0.

**Whats Next?**

Working to bring back some of the platforms Harness has supported in previous iterations:

* Windows Runtime
* Asp.Net MVC
* Web Api
* OWIN

More IOC Container:

* TinyIOC
* MEF
* StructureMap

AND a whole bunch of source cleanup. :)



