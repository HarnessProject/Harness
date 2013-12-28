using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Composition;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Timesheets.Services;

namespace Timesheets.Windows.Service
{
    public partial class SchedulerService : ServiceBase
    {
        IScheduler Scheduler { get; set; }
        public SchedulerService()
        {
            InitializeComponent();
        }

        protected override async void OnStart(string[] args) {
            await Application.Timesheets.Start(true);
            Scheduler = Application.Global.Container.GetInstance<IScheduler>();
            Scheduler.Start();
        }

        protected override void OnStop()
        {
            Application.Global.Dispose();
        }
    }
}
