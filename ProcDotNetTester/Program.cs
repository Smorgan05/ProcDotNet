using ProcDotNet;
using ProcDotNet.Classes;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace ProcDotNetTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string parentProc = "explorer.exe";
            string process = "brave.exe";
            string testPath = @"C:\Users\Dark\Documents\ProcDotNet Local\Logfile.CSV";
            string testPathNew = @"C:\Users\Dark\Documents\ProcDotNet Local\Logfile_12_26_2023.CSV";

            // Load ProcMon CSV (with Fixed Times)

            // Process Tree Testing
            var ProcTreeCheck = Support.ProcessTreeMaker(testPath);

            // Event Classes
            Dictionary<string, List<ProcMon>> ProcessDicts = Processor.LoadLists(testPath);
            var Registry = ProcessDicts[EventClass.Registry];
            var Network = ProcessDicts[EventClass.Network];
            var FileSystem = ProcessDicts[EventClass.FileSystem];
            var Process = ProcessDicts[EventClass.Process];
            var AllEvents = ProcessDicts[EventClass.All];

            // Sorting
            //var procName = ProcMaps.OrderBy(x => x.Key.ProcessName).ToList();
            var timeOfDay = AllEvents.OrderBy(x => x.TimeOfDay).ToList();

            //Test.DictionaryPrinter(timeOfDayBuckets);

            //Test Print Method
            //Test.RecNodeListPrinter(ProcTreeCheck);
            //Test.DictionaryPrinter(ProcessDicts, EventClass.Registry);

            //Test.RecNodePrinter(node.Children.First());
            //Test.RecNodePrinter(LinkProcessNodes[4]);
            //Test.BucketPrinter(ProcessBuckets);
            //Test.KeyValuePrinter(ProcMaps);
            Test.Printer(timeOfDay);
        }
    }
}
