using System.Collections.Generic;
using System.Threading;


namespace Harness
{
        

    public delegate void MessageHandler<in T>(T message);

    public delegate void MessageHandler(object message);
}
