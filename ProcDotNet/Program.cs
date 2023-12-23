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

            var tester = ProcMaps.OrderBy(x => x.Key.ProcessName).ToList();

            //var timeOfDayBuckets = ProcessBucketGroups.OrderByDescending(x => x.Key.TimeOfDay).ToDictionary(x => x.Key, x => x.Value);
            //var ProcessIDBuckets = ProcessBucketGroups.OrderByDescending(x => x.Key.ProcessID).ToDictionary(x => x.Key, x => x.Value);

            // Linked Tree Nodes List
            //List<TreeNode<ProcMon>> ProcessNodes = GetTreeList(ProcessIDBuckets);

            // Inter Node Mapping
            //List<TreeNode<ProcMon>> LinkedProcessNodes = GetLinkedNodes(ProcessNodes);

            //Sort by number of processes
            //Dictionary<string, List<ProcMon>> sortedProcessBuckets = ProcessBuckets.OrderByDescending(x => x.Value.Capacity).ToDictionary(x => x.Key, x => x.Value);

            //Test.DictionaryPrinter(timeOfDayBuckets);

            // Process Mapper Proper (Specific) explorer.exe -> Many
            //List<ProcMon> ProcMaps = ProcessMapper(sortedProcessBuckets, parentProc);
            //Dictionary<ProcMon, List<ProcMon>> DictProcMaps = Processor.DictProcessMapper(sortedProcessBuckets, parentProc);

            // Find Parent Procss from bucket brave.exe -> sub braves
            //var temp = ParentChildMapper(sortedProcessBuckets, process);

            //Test Print Method
            //Console.WriteLine("Unique Processes: " + sortedProcessBuckets.Count);
            //Test.BucketPrinter(ProcessBuckets);
            //Test.DictionaryPrinter(ProcessBucketGroups);
            Test.KeyValuePrinter(tester);
            //Test.Printer(childProcs);
        }

        private static List<TreeNode<ProcMon>> GetLinkedNodes(List<TreeNode<ProcMon>> processNodesList)
        {
            List<TreeNode<ProcMon>> result = new List<TreeNode<ProcMon>>();
            //List<TreeNode<ProcMon>> temp = new List<TreeNode<ProcMon>>(processNodesList);

            IEnumerable<TreeNode<ProcMon>> testParent = processNodesList.Where(x => x.Data.ProcessID == 20084);
            IEnumerable<TreeNode<ProcMon>> testChildren = processNodesList.Where(x => x.Data.ParentPID == 20084);
            //Console.WriteLine(testParent.First().Data.ProcessName);

            // Linker
            foreach (var item in processNodesList)
            {
                Console.WriteLine();
                Console.WriteLine(item.Data.ProcessName + ": children : " + item.Children.Count);
                foreach (var child in item.Children)
                {
                    Console.WriteLine(child.Data.ProcessName + " " + child.Data.ProcessID);
                }


                //// Current process
                //int Parent = item.Data.ProcessID;
                //var currentNode = item;

                //// Find Child from other Nodes
                //List<TreeNode<ProcMon>> temp = processNodesList.Where(x => x.Data.ParentPID == Parent).ToList();

                //if (temp.Count > 0)
                //{
                //    //var child = temp.First().Data;
                //    Console.WriteLine();
                //    Console.WriteLine(item.Data.ProcessName + " " + item.Data.ProcessID + " " + item.Data.ParentPID);
                //    Console.WriteLine("Children count: " + item.Children.Count);
                //    //Console.WriteLine(child.ProcessName + ": " + child.ProcessID + " " + child.ParentPID);

                //    foreach (var node in temp)
                //    {
                //        Console.WriteLine(node.Data.ProcessName);
                //    }
                //}

                //Console.WriteLine(temp.Count);
            }




            return result;
        }

        private static List<TreeNode<ProcMon>> GetTreeList(Dictionary<ProcMon, List<ProcMon>> processBuckets)
        {
            //Setup and Pass
            Dictionary<ProcMon, List<ProcMon>> orgBuckets = new Dictionary<ProcMon, List<ProcMon>>(processBuckets);

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
                    singleRoot.AddChild(child);
                }

                TreeListRes.Add(singleRoot);
            }

            return TreeListRes;
        }


        private static List<TreeNode<ProcMon>> MakeTreeListBroken(Dictionary<ProcMon, List<ProcMon>> processBuckets)
        {
            //Setup and Pass
            Dictionary<ProcMon, List<ProcMon>> orgBuckets = new Dictionary<ProcMon, List<ProcMon>>(processBuckets);
            ProcMon Root = orgBuckets.Keys.First();

            // Make Return Result
            List<TreeNode<ProcMon>> TreeListRes = TreeMaker(new List<TreeNode<ProcMon>>(), orgBuckets);

            // Recurse Funky Town
            static List<TreeNode<ProcMon>> TreeMaker(List<TreeNode<ProcMon>> TreeList, Dictionary<ProcMon, List<ProcMon>> currentBuckets)
            {
                //Prep
                ProcMon current = currentBuckets.Keys.First();
                TreeNode<ProcMon> SingleTree = new TreeNode<ProcMon>(current);
                List<TreeNode<ProcMon>> TreeListTemp = new(TreeList);

                //Mapping (Layer 1 Parent)
                Dictionary<ProcMon, List<ProcMon>> temp = ProcessMapperDict(currentBuckets, current);
                
                // Mapping (Layer 1 Children)
                List<ProcMon> tempList = temp.First().Value;
                foreach (ProcMon item in tempList)
                {
                    SingleTree.AddChild(item);
                }

                // Add to TreeList
                TreeListTemp.Add(SingleTree);

                // Perform InterMap Check between Parent tree and (Child node of another Parent tree)
                var InterMapCheck = GetMapInterCheck(TreeListTemp, SingleTree);
                if (TreeListTemp.Count > 0 && InterMapCheck == true)
                {
                    // Adjust TreeList Temp
                    //TreeListTemp = ExecInterMap(TreeList);
                }

                // Pop Current Index From Buckets
                currentBuckets.Remove(current);

                // Check and Recurse
                if (currentBuckets.Count > 0 )
                {
                    // Continue if Buckets are not Empty
                    TreeListTemp = TreeMaker(TreeListTemp, currentBuckets);
                }
                return TreeListTemp;
            }

            return TreeListRes;
        }

        private static bool GetMapInterCheck(List<TreeNode<ProcMon>> treeList, TreeNode<ProcMon> singleTree)
        {
            // Pull Root of Single Tree (to map to child)
            int singleRoot = singleTree.Data.ParentPID;

            // Modify to Skip SignleTree from NodeList
            List<TreeNode<ProcMon>> tempTreeList = new List<TreeNode<ProcMon>>(treeList);
            tempTreeList.Remove(singleTree);

            // Iterate through Tree List
            foreach (TreeNode<ProcMon> item in tempTreeList)
            {
                // Find Process ID (Branch => Child of Branch) (Broken)
                TreeNode<ProcMon>? found = FindChildNode(item, singleRoot);
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
            TreeNode<ProcMon> result = null;
            var childNode = Node.Children;
            
            // Child Node Count Check
            if (childNode.Count == 0)
            {
                return null;
            }

            //First Layer (get childNode Match)
            TreeNode<ProcMon>? selectNode = childNode.Where(a => a.Data.ProcessID == singleRoot).FirstOrDefault();
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
