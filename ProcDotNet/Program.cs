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

            // Tree Testing
            //ProcDotNet.Tree.SampleIterating.MainTest();

            //TreeNode<string> treeRoot = SampleData.GetSet1();
            //TreeNode<string> found = treeRoot.FindTreeNode(node => node.Data != null && node.Data.Contains("210"));

            // Load ProcMon CSV (with Fixed Times)
            var ProcessBuckets = Processor.LoadCSV(testPath);

            //Get Process Buckets
            List<KeyValuePair<ProcMon, List<ProcMon>>> ProcessBucketGroups = Processor.GetProcessBucketGroups(ProcessBuckets);

            // Map Disparate Processes
            List<KeyValuePair<ProcMon, List<ProcMon>>> ProcMaps = Processor.GetInterProcMapping(ProcessBucketGroups);

            // Map KVPs to Process Nodes (Good)
            List<TreeNode<ProcMon>> ProcessNodes = NodeProcessor.GetTreeList(ProcMaps);

            // Inter Node Mapping (Good?)
            List<TreeNode<ProcMon>> LinkProcessNodes = NodeProcessor.MakeTreeList(ProcessNodes);

            //FinalNodes[4].Children = FinalNodes[4].Children.DistinctBy(x => x.Data.ProcessID).ToList();

            //TreeNode<ProcMon> node = LinkProcessNodes[4];

            // Sorting
            //var procName = ProcMaps.OrderBy(x => x.Key.ProcessName).ToList();
            //var timeOfDay = ProcMaps.OrderBy(x => x.Key.TimeOfDay).ToList();

            //Test.DictionaryPrinter(timeOfDayBuckets);

            //Test Print Method
            Test.RecNodeListPrinter(LinkProcessNodes);
            //Test.RecNodePrinter(node.Children.First());
            //Test.RecNodePrinter(LinkProcessNodes[4]);
            //Test.BucketPrinter(ProcessBuckets);
            //Test.DictionaryPrinter(ProcessBucketGroups);
            //Test.KeyValuePrinter(ProcMaps);
            //Test.Printer(childProcs);


        }

        private static List<KeyValuePair<ProcMon, List<ProcMon>>> ConvertToKVP(List<TreeNode<ProcMon>> processNodes)
        {
            List<KeyValuePair<ProcMon, List<ProcMon>>> result = new();
            foreach (var item in processNodes)
            {
                List<ProcMon> list = new List<ProcMon>();
                foreach (var child in item.Children)
                {
                    list.Add(child.Data);
                }
                var element = new KeyValuePair<ProcMon, List<ProcMon>>(item.Data, list);
                result.Add(element);
            }

            return result;
        }

        
    }

}
