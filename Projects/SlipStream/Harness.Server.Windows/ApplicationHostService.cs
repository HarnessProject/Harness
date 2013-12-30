using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Tasks;
using System.Text;
using System.Threading.Tasks;

namespace Harness.Server.Windows
{
    public partial class ApplicationHostService : ServiceBase
    {
        ApplicationtHost Host { get; set; }

        public ApplicationHostService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Host = new ApplicationtHost();
        }

        protected override async void OnStop() {
            await Host.Applications.EachAsync(x => x.StopService());
        }
    }
}
