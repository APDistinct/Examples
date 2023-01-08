using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.ReportDB
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("usage: Reports.exe <MessageId>");
                return;
            }
            if (!Guid.TryParse(args[0], out Guid messId))
            {
                Console.WriteLine("Invalid essage Id: " + args[0]);
                return;
            }

            //MakeReport(messId);

            var reporter = new DataPerformer();
            reporter.Perform(messId);
        }
    }
}
