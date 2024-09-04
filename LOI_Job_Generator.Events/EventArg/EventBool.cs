using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOI_Job_Generator.Events.EventArg
{
    public class EventBool : EventArgs
    {
        public bool eventBool { get; }

        public EventBool(bool value)
        {
            eventBool = value;
        }
    }
}
