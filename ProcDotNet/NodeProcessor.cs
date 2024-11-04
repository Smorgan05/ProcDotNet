using ProcDotNet.Classes;
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
        internal static List<JsonNode<ProcMon>> GetTreeList(List<KeyValuePair<ProcMon, List<ProcMon>>> processBuckets)
        {
            //Setup and Pass
            List<KeyValuePair<ProcMon, List<ProcMon>>> orgBuckets = new(processBuckets);

            // Make Return Result
            List<JsonNode<ProcMon>> TreeListRes = new List<JsonNode<ProcMon>>();

            // Perform One Layer Mapping
            foreach (var process in orgBuckets)
            {
                // First Node in Tree List
                JsonNode<ProcMon> singleRoot = new JsonNode<ProcMon>(process.Key);

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

        internal static List<JsonNode<ProcMon>> MakeTreeList(List<JsonNode<ProcMon>> processNodes)
        {
            List<JsonNode<ProcMon>> result = new();

            foreach (JsonNode<ProcMon> branch in processNodes)
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

        private static List<JsonNode<ProcMon>> SingleDedup(List<JsonNode<ProcMon>> linkProcessNodes)
        {
            //List<TreeNode<ProcMon>> temp = new List<TreeNode<ProcMon>>(linkProcessNodes);
            List<JsonNode<ProcMon>> result = new List<JsonNode<ProcMon>>(linkProcessNodes);

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

        private static List<JsonNode<ProcMon>> CheckResult(List<JsonNode<ProcMon>> Nodes, JsonNode<ProcMon> mapResult)
        {
            var Result = new List<JsonNode<ProcMon>>(Nodes);
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
                    JsonNode<ProcMon> remove = check.OrderByDescending(x => x.Children.Count).ToList().Skip(1).First();
                    parent.Children.Remove(remove);
                }
            }

            //else if (parent != null & mapResult != null && !parent.Children.Contains(mapResult))
            //{
            //    parent.AddChild(mapResult);
            //}

            return SingleDedup(Result);
        }

        internal static JsonNode<ProcMon> Mapper(List<JsonNode<ProcMon>> Nodes, JsonNode<ProcMon> currentNode)
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

        internal static JsonNode<ProcMon>? FindNode(List<JsonNode<ProcMon>> Nodes, int processID)
        {
            foreach (var item in Nodes)
            {
                JsonNode<ProcMon> found = RecFind(item, processID);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }

        // Search for a ProcessID in the specified node and all of its children
        internal static JsonNode<ProcMon> RecFind(JsonNode<ProcMon> Node, int ProcessID)
        {
            // find the string, starting with the current instance
            return RecFindNode(Node, ProcessID);

            static JsonNode<ProcMon> RecFindNode(JsonNode<ProcMon> node, int ProcessID)
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
