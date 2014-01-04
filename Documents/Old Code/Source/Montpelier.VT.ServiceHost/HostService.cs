using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Builder;
using Montpelier.VT.Services.Host.Owin;
using Nowin;

namespace Montpelier.VT.Services.Host
{
    partial class HostService: ServiceBase
    {
        INowinServer Server { get; set; }
        public HostService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
