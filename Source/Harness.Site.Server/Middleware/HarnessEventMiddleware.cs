using System;
using System.Threading.Tasks;
using Harness.Events;
using Harness.Framework;

using Harness.OWIN;
using Harness.Site.Server.Events;
using Microsoft.Owin;

namespace Harness.Site.Server.Middleware
{
    public class HarnessEventMiddleware : BaseMiddleWare
    {
        
        public override async Task Invoke(IOwinContext context) {
            //
            await 
                this
                .AsTask()
                .FuncAsync(x => x)
                .ActionAsync(
                    x => x.Trigger(new OwinPipelineEvent {Timestamp = DateTime.Now, Context = context})
                );

        }
    }
}