using ProcDotNet;
using ProcDotNet.Classes;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using static ProcDotNetTester.Support;

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
            List<ProcMon> ProcTree = Processor.ProcessTreeMaker(testPath);

            // Serialize Process Tree to JSON
            var JsonProcStr = JsonHelper.JSONConvProcTree(ProcTree);
            Console.WriteLine(JsonProcStr);

            // Get Process List
            List<string> ProcList = Processor.GetUniqueProcessList(testPath);

            // Find Node from List (first)
            var procMon = NodeProcessor.FindNodeFromListByProcessName(ProcTree, process);

            Console.WriteLine();

            // Event Classes
            //Dictionary<string, List<ProcMon>> ProcessDicts = Processor.LoadLists(testPath);
            //var Registry = ProcessDicts[EventClass.Registry];
            //var Network = ProcessDicts[EventClass.Network];
            //var FileSystem = ProcessDicts[EventClass.FileSystem];
            //var Process = ProcessDicts[EventClass.Process];
            //var AllEvents = ProcessDicts[EventClass.All];

            //string json = JsonHelper.JSONConvEvents(ProcessDicts);
            //string ProcTreeJson = JsonSerializer.Serialize(ProcTree, options);

            //File.WriteAllText(@"C:\Users\Dark\Documents\ProcDotNet Local\Test.json", json);
            //File.WriteAllText(@"C:\Users\Dark\Documents\ProcDotNet Local\ProcessTree.json", ProcTreeJson);

            // Sorting
            //var procName = ProcMaps.OrderBy(x => x.Key.ProcessName).ToList();
            //var timeOfDay = AllEvents.OrderBy(x => x.TimeOfDay).ToList();

            //Test.DictionaryPrinter(timeOfDayBuckets);

            //Test Print Method
            Test.RecNodeListPrinter(ProcTree);
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
