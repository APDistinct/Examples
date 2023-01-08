using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Viber.Client.Args
{
    //
    // Summary:
    //     Represents the base class for classes that contain event data, and provides a
    //     value to use for events that do not include event data.
    public class EventArgs
    {
        //
        // Summary:
        //     Provides a value to use with events that do not have event data.
        public static readonly EventArgs Empty;

        //
        // Summary:
        //     Initializes a new instance of the System.EventArgs class.
        public EventArgs() { }
    }
}
