using System;
using System.Collections.Generic;
using System.Composition;
using System.Dynamic;
using System.Events;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Harness.Http
{
    

    public interface ISessionState : IDictionary<string, object> {
        Guid Id { get; }
    }

    public class SessionStateDictionary : Dictionary<string, object> {
        public Guid Id { get; set; }
    }

    

}
