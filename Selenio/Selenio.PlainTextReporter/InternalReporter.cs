using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenio.PlainTextReporter
{
    internal class InternalReporter
    {
        public static string PreviousClassName { get; set; }
        public static string PreviousTimeStamp { get; set; }
        public static string PreviousTestStep { get; set; }
    }
}
