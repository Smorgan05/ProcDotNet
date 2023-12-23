// See https://aka.ms/new-console-template for more information


using CsvHelper;
using Microsoft.Win32;
using ProcDotNet.Tree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

            // Load ProcMon CSV (with Fixed Times)
            var ProcessBuckets = Processor.LoadCSV(testPath);

            //Get Process Buckets (with dup indexes)
            List<KeyValuePair<ProcMon, List<ProcMon>>> ProcessBucketGroups = Processor.GetProcessBucketGroups(ProcessBuckets);

            // Map Disparate Processes
            List<KeyValuePair<ProcMon, List<ProcMon>>> ProcMaps = Processor.GetInterProcMapping(ProcessBucketGroups);

            //var tester = ProcMaps.OrderBy(x => x.Key.ProcessName).ToList();

            //var timeOfDayBuckets = ProcessBucketGroups.OrderByDescending(x => x.Key.TimeOfDay).ToDictionary(x => x.Key, x => x.Value);
            //var ProcessIDBuckets = ProcessBucketGroups.OrderByDescending(x => x.Key.ProcessID).ToDictionary(x => x.Key, x => x.Value);

            // Linked Tree Nodes List
            List<TreeNode<ProcMon>> ProcessNodes = GetTreeList(ProcMaps);

            TreeNode<ProcMon> one = ProcessNodes[5];
            TreeNode<ProcMon> temp = one.FindTreeNode(node => node.Data != null && node.Data.ProcessID == 5064);

            // Inter Node Mapping
            List<TreeNode<ProcMon>> LinkedProcessNodes = MakeTreeListNew(ProcessNodes);

            //Sort by number of processes
            //Dictionary<string, List<ProcMon>> sortedProcessBuckets = ProcessBuckets.OrderByDescending(x => x.Value.Capacity).ToDictionary(x => x.Key, x => x.Value);

            //Test.DictionaryPrinter(timeOfDayBuckets);

            //Test Print Method
            //Console.WriteLine("Unique Processes: " + sortedProcessBuckets.Count);
            //Test.NodeListPrinter(ProcessNodes);
            //Test.NodePrinter(one);
            //Test.NodePrinter(temp);
            //Test.BucketPrinter(ProcessBuckets);
            //Test.DictionaryPrinter(ProcessBucketGroups);
            //Test.KeyValuePrinter(ProcMaps);
            //Test.Printer(childProcs);
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

        private static List<TreeNode<ProcMon>> MakeTreeListNew(List<TreeNode<ProcMon>> processNodes)
        {
            List<TreeNode<ProcMon>> result = new();

            // Make Return Result
            result = TreeMaker(new List<TreeNode<ProcMon>>(), processNodes);

            static List<TreeNode<ProcMon>> TreeMaker(List<TreeNode<ProcMon>> TreeList, List<TreeNode<ProcMon>> Nodes)
            {
                // Prep
                List<TreeNode<ProcMon>> TreeListTemp = new(TreeList);
                TreeNode<ProcMon> currentRoot = Nodes.First();
                TreeListTemp.Add(currentRoot);

                // Check for Parent (ProcessID) => Child (ParentID) - Continue Here!!!
                var InterMapCheck = GetMapInterCheck(TreeListTemp, currentRoot);
                if (TreeListTemp.Count > 0 && InterMapCheck == true)
                {
                    // Adjust TreeList Temp
                    //TreeListTemp = ExecInterMap(TreeList);
                }

                // Pop Current Index From Buckets
                Nodes.Remove(currentRoot);

                // Check and Recurse
                if (Nodes.Count > 0)
                {
                    // Continue if Buckets are not Empty
                    TreeListTemp = TreeMaker(TreeListTemp, Nodes);
                }
                return TreeListTemp;
            }

            return result;
        }

        private static bool GetMapInterCheck(List<TreeNode<ProcMon>> treeList, TreeNode<ProcMon> singleTree)
        {
            // Pull Root of Single Tree (to map to child)
            int singleRoot = singleTree.Data.ParentPID;
            List<TreeNode<ProcMon>> tempTreeList = new List<TreeNode<ProcMon>>(treeList);

            // Iterate through Tree List
            foreach (TreeNode<ProcMon> item in tempTreeList)
            {
                // Find Process ID (Branch => Child of Branch) (Broken)
                TreeNode<ProcMon>? found = item.FindTreeNode(node => node.Data.ProcessID == singleRoot);
                if (found != null)
                {
                    Console.WriteLine("found one");
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        private static TreeNode<ProcMon>? FindChildNode(TreeNode<ProcMon> Node, int singleRoot)
        {
            TreeNode<ProcMon>? result = null;
            var childNode = Node.Children;
            
            // Child Node Count Check
            if (childNode.Count == 0)
            {
                return null;
            }

            //First Layer (get childNode Match)
            TreeNode<ProcMon> selectNode = childNode.Where(a => a.Data.ProcessID == singleRoot).First();
            //var select = test.Select(a => )
            if (selectNode != null)
            {
                result = (TreeNode<ProcMon>?)selectNode;
            }

            // Recurse
            else
            {
                // Iterate through children
                foreach (var item in childNode)
                {
                    // N Layer
                    var temp = FindChildNode(item, singleRoot);
                    if (temp != null)
                    {
                        result = temp;
                    }

                }
            }
            return result;

        }
    }

}
