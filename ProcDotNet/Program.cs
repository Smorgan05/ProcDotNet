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

            //TreeNode<string> treeRoot = SampleData.GetSet1();
            //TreeNode<string> found = treeRoot.FindTreeNode(node => node.Data != null && node.Data.Contains("210"));

            // Load ProcMon CSV (with Fixed Times)
            var ProcessBuckets = Processor.LoadCSV(testPath);

            //Get Process Buckets (with dup indexes)
            List<KeyValuePair<ProcMon, List<ProcMon>>> ProcessBucketGroups = Processor.GetProcessBucketGroups(ProcessBuckets);

            // Map Disparate Processes
            List<KeyValuePair<ProcMon, List<ProcMon>>> ProcMaps = Processor.GetInterProcMapping(ProcessBucketGroups);

            // Linked Tree Nodes List
            List<TreeNode<ProcMon>> ProcessNodes = GetTreeList(ProcMaps);

            // Check for match
            List<KeyValuePair<ProcMon, List<ProcMon>>> ProcMapsCheck = ConvertToKVP(ProcessNodes);

            // Inter Node Mapping
            //List<TreeNode<ProcMon>> LinkedProcessNodes = MakeTreeListNew(ProcessNodes);

            // Sorting
            //var procName = ProcMaps.OrderBy(x => x.Key.ProcessName).ToList();
            //var timeOfDay = ProcMaps.OrderBy(x => x.Key.TimeOfDay).ToList();

            TreeNode<ProcMon> one = ProcessNodes[5];
            TreeNode<ProcMon> temp = one.FindTreeNode(node => node.Data != null && node.Data.ProcessID == 5064);

            //Test.DictionaryPrinter(timeOfDayBuckets);

            //Test Print Method
            //Test.NodeListPrinter(ProcessNodes);
            //Test.NodePrinter(one);
            //Test.NodePrinter(temp);
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
                //Nodes.Remove(currentRoot);

                //// Check and Recurse
                //if (Nodes.Count > 0)
                //{
                //    // Continue if Buckets are not Empty
                //    TreeListTemp = TreeMaker(TreeListTemp, Nodes);
                //}
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
                TreeNode<ProcMon> found = item.FindTreeNode(node => node.Data != null && node.Data.ProcessID == singleRoot);
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
    }

}
