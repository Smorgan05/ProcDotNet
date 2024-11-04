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
            string JsonProcStr = JsonHelper.JSONConvProcTree(ProcTree);
            File.WriteAllText(@"C:\Users\Dark\Documents\ProcDotNet Local\ProcessTree.json", JsonProcStr);
            Console.WriteLine(JsonProcStr);

            // Event Classes
            Dictionary<string, List<ProcMon>> ProcessDicts = Processor.LoadLists(testPath);
            string JsonDicts = JsonHelper.JSONConvEvents(ProcessDicts);
            File.WriteAllText(@"C:\Users\Dark\Documents\ProcDotNet Local\All.json", JsonDicts);

            // Get Process List
            List<string> ProcList = Processor.GetUniqueProcessList(testPath);

            // Find Node from List (first)
            var procMon = NodeProcessor.FindNodeFromListByProcessName(ProcTree, process);

            //Console.WriteLine();
            //var Registry = ProcessDicts[EventClass.Registry];
            //var Network = ProcessDicts[EventClass.Network];
            //var FileSystem = ProcessDicts[EventClass.FileSystem];
            //var Process = ProcessDicts[EventClass.Process];
            //var Profiling = ProcessDicts[EventClass.Profiling];

            //Test Print Method
            //Test.RecNodeListPrinter(ProcTree);

            //Test.DictionaryPrinter(ProcessDicts, EventClass.Registry);
            //Test.RecNodePrinter(node.Children.First());
            //Test.RecNodePrinter(LinkProcessNodes[4]);
            //Test.BucketPrinter(ProcessBuckets);
            //Test.KeyValuePrinter(ProcMaps);
            //Test.Printer(timeOfDay);
        }
    }
}
