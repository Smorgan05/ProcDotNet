// See https://aka.ms/new-console-template for more information


using CsvHelper;
using Microsoft.Win32;
using ProcDotNet.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Sources;
using System.Xml.Linq;
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

            // Map KVPs to Process Nodes
            List<TreeNode<ProcMon>> ProcessNodes = GetTreeList(ProcMaps);

            // Inter Node Mapping
            List<TreeNode<ProcMon>> LinkProcessNodes = MakeTreeList(ProcessNodes);

            // MultiLayer Dedup (check)
            List<TreeNode<ProcMon>> FinalNodes = SingleDedup(LinkProcessNodes);

            //FinalNodes[4].Children = FinalNodes[4].Children.DistinctBy(x => x.Data.ProcessID).ToList();

            // Sorting
            //var procName = ProcMaps.OrderBy(x => x.Key.ProcessName).ToList();
            //var timeOfDay = ProcMaps.OrderBy(x => x.Key.TimeOfDay).ToList();

            //Test.DictionaryPrinter(timeOfDayBuckets);

            //Test Print Method
            Test.RecNodeListPrinter(FinalNodes);
            //Test.RecNodePrinter(FinalNodes[4]);
            //Test.RecNodePrinter(LinkProcessNodes[2]);
            //Test.BucketPrinter(ProcessBuckets);
            //Test.DictionaryPrinter(ProcessBucketGroups);
            //Test.KeyValuePrinter(ProcMaps);
            //Test.Printer(childProcs);
        }


        public static List<TreeNode<ProcMon>> SingleDedup(List<TreeNode<ProcMon>> linkProcessNodes)
        {
            //List<TreeNode<ProcMon>> temp = new List<TreeNode<ProcMon>>(linkProcessNodes);
            List<TreeNode<ProcMon>> result = new List<TreeNode<ProcMon>>(linkProcessNodes);

            // Layer One dup (prioritize keeping nested over orphan)
            foreach (var item in linkProcessNodes)
            {
                foreach (var proc in linkProcessNodes)
                {
                    if (proc.Children.Contains(item))
                    {
                        result.Remove(item);
                    }
                }
            }
            return result;
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

        /// <summary>
        /// Perform Layer 1 Node Mapping
        /// </summary>
        /// <param name="processBuckets"></param>
        /// <returns></returns>
        private static List<TreeNode<ProcMon>> GetTreeList(List<KeyValuePair<ProcMon, List<ProcMon>>> processBuckets)
        {
            //Setup and Pass
            List<KeyValuePair<ProcMon, List<ProcMon>>> orgBuckets = new(processBuckets);

            // Make Return Result
            List<TreeNode<ProcMon>> TreeListRes = new List<TreeNode<ProcMon>>();

            // Perform One Layer Mapping
            foreach (var process in orgBuckets)
            {
                // First Node in Tree List
                TreeNode<ProcMon> singleRoot = new TreeNode<ProcMon>(process.Key);

                // Add Children
                foreach (var child in process.Value)
                {
                    if (process.Key != child)
                    {
                        singleRoot.AddChild(child);
                    }
                }
                TreeListRes.Add(singleRoot);
            }

            return TreeListRes;
        }

        private static List<TreeNode<ProcMon>> MakeTreeList(List<TreeNode<ProcMon>> processNodes)
        {
            List<TreeNode<ProcMon>> result = new();

            foreach (TreeNode<ProcMon> branch in processNodes)
            {
                var MapResult = Mapper(processNodes, branch);

                // Top Check
                if (MapResult != null && !result.Contains(branch))
                {
                    // Adds Duplicates
                    result = CheckResult(result, MapResult);
                    //result.Add(MapResult);
                }
            }

            return result;
        }

        internal static TreeNode<ProcMon> Mapper(List<TreeNode<ProcMon>> Nodes, TreeNode<ProcMon> currentNode)
        { 
            var ParentNode = FindNode(Nodes, currentNode.Data.ParentPID);
            if (ParentNode != null && ParentNode != currentNode && !ParentNode.Children.Contains(currentNode))
            {
                ParentNode.AddChild(currentNode);
                Mapper(Nodes, ParentNode);
            }
            else if (ParentNode == null && ParentNode != currentNode) 
            {
                // Ensure distinct children
                currentNode.Children = currentNode.Children.DistinctBy(x => x.Data.ProcessID).ToList();
                return currentNode;
            }

            // Ensure distinct children
            ParentNode.Children = ParentNode.Children.DistinctBy(x => x.Data.ProcessID).ToList();
            return ParentNode;
        }

        private static List<TreeNode<ProcMon>> CheckResult(List<TreeNode<ProcMon>> Nodes, TreeNode<ProcMon> mapResult)
        {
            var Result = new List<TreeNode<ProcMon>>(Nodes);
            var temp = FindNode(Nodes, mapResult.Data.ProcessID);
            if (temp == null)
            {
                Result.Add(mapResult);
            }

            return Result;
        }

        private static TreeNode<ProcMon>? FindNode(List<TreeNode<ProcMon>> Nodes, int processID)
        {
            foreach (var item in Nodes)
            {
                TreeNode<ProcMon> found = RecFind(item, processID);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }

        // Search for a ProcessID in the specified node and all of its children
        public static TreeNode<ProcMon> RecFind(TreeNode<ProcMon> Node, int ProcessID)
        { 
            // find the string, starting with the current instance
            return RecFindNode(Node, ProcessID);

            static TreeNode<ProcMon> RecFindNode(TreeNode<ProcMon> node, int ProcessID)
            {
                if (node.Data.ProcessID == ProcessID)
                    return node;

                foreach (var child in node.Children)
                {
                    var result = RecFindNode(child, ProcessID);
                    if (result != null)
                        return result;
                }

                return null;
            }

        }

    }

}
