using ProcDotNet;
using ProcDotNet.Classes;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using ProcDotNet.Tree;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

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
            List<TreeNode<ProcMon>> ProcTree = Processor.ProcessTreeMaker(testPath);

            // Event Classes
            Dictionary<string, List<ProcMon>> ProcessDicts = Processor.LoadLists(testPath);
            //var Registry = ProcessDicts[EventClass.Registry];
            //var Network = ProcessDicts[EventClass.Network];
            //var FileSystem = ProcessDicts[EventClass.FileSystem];
            //var Process = ProcessDicts[EventClass.Process];
            //var AllEvents = ProcessDicts[EventClass.All];

            string json = Support.JSONConv(ProcTree, ProcessDicts);

            // Print or use the JSON string as needed
            //Console.WriteLine(json);

            File.WriteAllText(@"C:\Users\Dark\Documents\ProcDotNet Local\Test.json", json);

            // Sorting
            //var procName = ProcMaps.OrderBy(x => x.Key.ProcessName).ToList();
            //var timeOfDay = AllEvents.OrderBy(x => x.TimeOfDay).ToList();

            //Test.DictionaryPrinter(timeOfDayBuckets);

            //Test Print Method
            //Test.RecNodeListPrinter(ProcTree);
            //Test.DictionaryPrinter(ProcessDicts, EventClass.Registry);

            //var test = Support.JSONConv(ProcTree[0]);


            //Test.RecNodePrinter(node.Children.First());
            //Test.RecNodePrinter(LinkProcessNodes[4]);
            //Test.BucketPrinter(ProcessBuckets);
            //Test.KeyValuePrinter(ProcMaps);
            //Test.Printer(timeOfDay);
        }
    }
}
