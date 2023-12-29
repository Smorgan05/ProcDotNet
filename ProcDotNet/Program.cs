// See https://aka.ms/new-console-template for more information


using CsvHelper;
using Microsoft.Win32;
using ProcDotNet;
using ProcDotNet.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Sources;
using System.Xml.Linq;
using System.Xml.XPath;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProcNet
{
    class Program
    {
        static void Main(string[] args)
        {
            string parentProc = "explorer.exe";
            string process = "brave.exe";
            string testPath = @"C:\Users\Dark\Documents\ProcDotNet Local\Logfile.CSV";
            string testPathNew = @"C:\Users\Dark\Documents\ProcDotNet Local\Logfile_12_26_2023.CSV";

            var ProcTreeCheck = ProcessTreeMaker(testPath);

            // Sorting
            //var procName = ProcMaps.OrderBy(x => x.Key.ProcessName).ToList();
            //var timeOfDay = ProcMaps.OrderBy(x => x.Key.TimeOfDay).ToList();

            //Test.DictionaryPrinter(timeOfDayBuckets);

            //Test Print Method
            Test.RecNodeListPrinter(ProcTreeCheck);
            //Test.RecNodePrinter(node.Children.First());
            //Test.RecNodePrinter(LinkProcessNodes[4]);
            //Test.BucketPrinter(ProcessBuckets);
            //Test.DictionaryPrinter(ProcessBucketGroups);
            //Test.KeyValuePrinter(ProcMaps);
            //Test.Printer(childProcs);


        }

        internal static List<TreeNode<ProcMon>> ProcessTreeMaker(string filePath)
        {
            // Load ProcMon CSV (with Fixed Times)
            var ProcessDicts = Processor.LoadLists(filePath);

            // Load Buckes
            var ProcessBuckets = Processor.ProcessSorter(ProcessDicts["ProfileEvents"]);

            //Get Process Buckets
            List<KeyValuePair<ProcMon, List<ProcMon>>> ProcessBucketGroups = Processor.GetProcessBucketGroups(ProcessBuckets);

            // Map Disparate Processes
            List<KeyValuePair<ProcMon, List<ProcMon>>> ProcMaps = Processor.GetInterProcMapping(ProcessBucketGroups);

            // Map KVPs to Process Nodes (Good)
            List<TreeNode<ProcMon>> ProcessNodes = NodeProcessor.GetTreeList(ProcMaps);

            // Inter Node Mapping (Good?)
            List<TreeNode<ProcMon>> LinkProcessNodes = NodeProcessor.MakeTreeList(ProcessNodes);
            return LinkProcessNodes;
        }

        
    }

}
