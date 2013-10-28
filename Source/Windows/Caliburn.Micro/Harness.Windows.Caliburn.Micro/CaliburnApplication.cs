using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Autofac;
using Caliburn.Micro;
using Harness.Core;
using Newtonsoft.Json;

namespace Harness.WinRT.CaliburnMicro {
    public class Application : CaliburnApplication {
        protected bool TreatViewAsLoaded { get; set; }

        protected IContainer Container { get; private set; }
        protected bool EnforceNamespaceConvention { get; set; }
        protected bool AutoSubscribeEventAggegatorHandlers { get; set; }
        protected Type ViewModelBaseType { get; set; }
        protected bool ViewModelFirst { get; set; }

        //public Func<IWindowManager> CreateWindowManager { get; set; }
        protected Func<IEventAggregator> CreateEventAggregator { get; set; }
        protected Type DefaultViewType { get; set; }

        //public Frame RootFrame { get; protected set; }

        protected override void Configure() {
            base.Configure();

            //  allow base classes to change bootstrapper settings
            ConfigureApplication();

            var builder = new ContainerBuilder();
            Application<Environment>.New(new Environment(false, () => builder));

            Assembly[] assemblies = Harness.Application.EnvironmentAs<Environment>().Assemblies.ToArray();
            IEnumerable<Type> types = assemblies.SelectMany(a => a.ExportedTypes);

            foreach (
                Type t in 
                    types
                        .Where(
                            t =>
                                (
                                    t.Name.EndsWith("ViewModel") &&
                                    (!EnforceNamespaceConvention ||
                                     (!string.IsNullOrWhiteSpace(t.Namespace) && t.Namespace.EndsWith("ViewModels")))
                                    ) ||
                                (
                                    t.Name.EndsWith("View") &&
                                    (!EnforceNamespaceConvention ||
                                     (!string.IsNullOrWhiteSpace(t.Namespace) && t.Namespace.EndsWith("Views")))
                                    )
                        )
                ) {
                builder.RegisterType(t).AsSelf().InstancePerDependency();
            }

            builder.Register(c => CreateEventAggregator()).SingleInstance();

            //  should we install the auto-subscribe event aggregation handler module?
            if (AutoSubscribeEventAggegatorHandlers)
                builder.RegisterModule<EventAggregationAutoSubscriptionModule>();
            //  allow derived classes to add to the container
            ConfigureContainer(builder);

            Container = builder.Build();
            Harness.Application.EnvironmentAs<Environment>().SetContainer(Container);
        }

        protected override object GetInstance(Type service, string key) {
            object instance;
            if (string.IsNullOrWhiteSpace(key)) {
                if (Container.TryResolve(service, out instance))
                    return instance;
            }

            else {
                if (Container.TryResolveNamed(key, service, out instance))
                    return instance;
            }

            //If we haven't returned by now, let's provide a softer landing...
            try {
                instance = Activator.CreateInstance(service);
                return instance;
            }
            catch (Exception ex) {
                //Hard landing!
                throw new Exception(
                    string.Format("Could not locate any instances of contract {0}.", key ?? service.Name),
                    ex);
            }
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            object instances;
            Type type = typeof (IEnumerable<>).MakeGenericType(service);

            if (Container.TryResolve(type, out instances))
                return instances as IEnumerable<object>;

            //If we haven't returned by now, let's provide a softer landing...
            try {
                instances = Activator.CreateInstance(type);
                return instances as IEnumerable<object>;
            }
            catch (Exception ex) {
                //Hard landing!
                throw new Exception(string.Format("Could not locate any instances of contract {0}.", service.Name), ex);
            }
        }

        protected override void BuildUp(object instance) {
            Container.InjectProperties(instance);
        }

        protected override async void PrepareViewFirst(Frame rootFrame) {
            if (!ViewModelFirst) {
                var builder = new ContainerBuilder();
                builder.RegisterInstance<INavigationService>(new FrameAdapter(rootFrame, TreatViewAsLoaded)).
                    SingleInstance();
                builder.Update(Container);
            }
        }

        protected virtual void ConfigureContainer(ContainerBuilder builder) {
            //Your code here.....
        }

        protected virtual void ConfigureApplication() {
            ViewModelFirst = false;
            EnforceNamespaceConvention = true;
            AutoSubscribeEventAggegatorHandlers = true;
            ViewModelBaseType = typeof (INotifyPropertyChanged);
            CreateEventAggregator = () => new EventAggregator();
        }

        //protected override async void OnLaunched(LaunchActivatedEventArgs args)
        //{
        //    base.OnLaunched(args);
        //    if (args.PreviousExecutionState != ApplicationExecutionState.Terminated) return;

        //    var storage = ApplicationData.Current.RoamingFolder;
        //    StorageFile history = null;
        //    IRandomAccessStream historyStream = null;
        //    DataReader historyReader = null;
        //    try
        //    {
        //        history = await storage.GetFileAsync("history.state");
        //        if (history == null) return;
        //        historyStream = await history.OpenAsync(FileAccessMode.Read);
        //        historyReader = new DataReader(historyStream.GetInputStreamAt(0));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    if (historyReader == null || historyStream == null) return;

        //    RootFrame.SetNavigationState(historyReader.ReadString(historyReader.UnconsumedBufferLength));

        //}

        //protected override async void OnSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        //{
        //    var deferal = e.SuspendingOperation.GetDeferral();

        //    var storage = Windows.Storage.ApplicationData.Current.RoamingFolder;
        //    var file = await storage.CreateFileAsync("history.state", CreationCollisionOption.ReplaceExisting);
        //    var history = await file.OpenAsync(FileAccessMode.ReadWrite);
        //    var historyWriter = new DataWriter(history.GetOutputStreamAt(0));
        //    var state = RootFrame.GetNavigationState();
        //    historyWriter.WriteString(state);
        //    await historyWriter.StoreAsync();
        //    await historyWriter.FlushAsync();
        //    historyWriter.DetachStream();
        //    file = null;
        //    deferal.Complete();

        //}

        //protected override async void OnResuming(object sender, object e)
        //{
        //    //IsResuming(); //FIX: NOT FOR RESUMING, FOR RESTARTING
        //}

        //protected bool IsRestarting { get; set; }
    }

    public interface ISuspend {
        void OnSuspend(SuspendingOperation operation);
    }

    public interface IResume {
        void OnResume();
    }

    public interface IExit {
        void OnExit();
    }

    public class Screen<T> : Screen {
        private string _parameter;
        public T Context { get; set; }

        public string Parameter {
            get { return _parameter; }
            set {
                _parameter = value;
                if (_parameter == null) return;

                Context = new JsonSerializer().Deserialize<T>(new JsonTextReader(new StringReader(_parameter)));
                NotifyOfPropertyChange(() => Context);
            }
        }
    }
}