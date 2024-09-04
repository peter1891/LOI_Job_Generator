using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOI_Job_Generator.Events.EventArg
{
    public class EventMessage : EventArgs
    {
        public string eventMessage { get; }

        public EventMessage(string message)
        {
            eventMessage = message;
        }
    }
}
