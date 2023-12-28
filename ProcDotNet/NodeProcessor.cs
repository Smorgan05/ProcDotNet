using ProcDotNet.Tree;
using ProcNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcDotNet
{
    public class NodeProcessor
    {
        /// <summary>
        /// Perform Layer 1 Node Mapping
        /// </summary>
        /// <param name="processBuckets"></param>
        /// <returns></returns>
        internal static List<TreeNode<ProcMon>> GetTreeList(List<KeyValuePair<ProcMon, List<ProcMon>>> processBuckets)
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

        internal static List<TreeNode<ProcMon>> MakeTreeList(List<TreeNode<ProcMon>> processNodes)
        {
            List<TreeNode<ProcMon>> result = new();

            foreach (TreeNode<ProcMon> branch in processNodes)
            {
                var MapResult = Mapper(processNodes, branch);

                // Top Check
                if (MapResult != null && !result.Contains(branch))
                {
                    // Check for Duplicates
                    result = CheckResult(result, MapResult);
                    //result.Add(branch);
                }
            }

            return result;
        }

        private static List<TreeNode<ProcMon>> SingleDedup(List<TreeNode<ProcMon>> linkProcessNodes)
        {
            //List<TreeNode<ProcMon>> temp = new List<TreeNode<ProcMon>>(linkProcessNodes);
            List<TreeNode<ProcMon>> result = new List<TreeNode<ProcMon>>(linkProcessNodes);

            if (linkProcessNodes.Count == 0)
            {
                return result;
            }

            // Layer One dup (prioritize keeping nested over orphan)
            foreach (var item in linkProcessNodes)
            {
                foreach (var single in linkProcessNodes)
                {
                    var tempNode = RecFind(single, item.Data.ProcessID);
                    if (tempNode != null && item != single)
                    {
                        result.Remove(tempNode);
                    }
                }
            }

            return result;
        }

        private static List<TreeNode<ProcMon>> CheckResult(List<TreeNode<ProcMon>> Nodes, TreeNode<ProcMon> mapResult)
        {
            var Result = new List<TreeNode<ProcMon>>(Nodes);
            var temp = FindNode(Nodes, mapResult.Data.ProcessID);
            var parent = FindNode(Nodes, mapResult.Data.ParentPID);

            // Dup Check
            if (temp == null)
            {
                Result.Add(mapResult);
            }

            //else if (!Nodes.Contains(mapResult) && parent == null)
            //{
            //    Result.Add(mapResult);
            //}

            else if (mapResult != null && parent != null)
            {
                var check = parent.Children.Where(x => x.Data.ProcessID == mapResult.Data.ProcessID);
                if (check.Count() > 1)
                {
                    TreeNode<ProcMon> remove = check.OrderByDescending(x => x.Children.Count).ToList().Skip(1).First();
                    parent.Children.Remove(remove);
                }
            }
            else if (parent != null & mapResult != null && !parent.Children.Contains(mapResult))
            {
                parent.AddChild(mapResult);
            }

            Result = SingleDedup(Result);
            return Result;
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
                //currentNode.Children = currentNode.Children.DistinctBy(x => x.Data.ProcessID).ToList();
                return currentNode;
            }

            // Ensure distinct children
            //ParentNode.Children = ParentNode.Children.DistinctBy(x => x.Data.ProcessID).ToList();
            return ParentNode;
        }

        internal static TreeNode<ProcMon>? FindNode(List<TreeNode<ProcMon>> Nodes, int processID)
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
        internal static TreeNode<ProcMon> RecFind(TreeNode<ProcMon> Node, int ProcessID)
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
