using ProcDotNet.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProcDotNet
{
    public class Core
    {
        public static string NodeFilter(string filePath, string process, string Events)
        {
            // Single Node with Children (not multi yet)
            string result = string.Empty;
            var ProcTree = Processor.ProcessTreeMaker(filePath);
            var EventDict = Processor.LoadLists(filePath);

            //var ProcTreeEvents = Processor.ProcessTreeMakerNew(filePath);
            //Dictionary<string, List<ProcMon>>? EventDict = ProcTreeEvents.Keys.FirstOrDefault();
            //List<ProcMon>? ProcTree = ProcTreeEvents.Values.FirstOrDefault();

            var procMon = NodeProcessor.FindNodeFromListByProcessName(ProcTree, process);

            if (!EventClass.Types.Contains(Events))
            {
                throw new Exception("Invalid Event");
            }

            if (procMon != null)
            {
                var procMonEve = new List<ProcMon>();
                if (Events.Equals(EventClass.All))
                {
                    procMonEve = EventFilterAndAttachNew(EventDict, procMon);
                    
                    result = JsonHelper.JSONConvProcTree(procMonEve);
                }
                else
                {
                }
            }
            else
            {
                return result;
            }

            return result;
        }

        private static List<ProcMon> EventFilterAndAttachNew(Dictionary<string, List<ProcMon>> eventDict, ProcMon procMon)
        {
            // Get list of all Nodes (iterate)
            static IEnumerable<ProcMon> FlattenRecursive(ProcMon node)
            {
                yield return node;
                foreach (var child in node.Children)
                {
                    foreach (var flattenedNode in FlattenRecursive(child))
                    {
                        yield return flattenedNode;
                    }
                }
            }

            // Adjust Nodes recursives to include events
            static void recAdjustNodes(ProcMon node, ProcMon finder, Dictionary<string, List<ProcMon>> eventDict)
            {
                if (node.ProcessID == finder.ProcessID)
                {
                    // Adjust inplace
                    node.EventsFileSystem.AddRange(eventDict[EventClass.FileSystem].Where(x => x.ProcessID == node.ProcessID).AsParallel().ToList());
                    node.EventsNetwork.AddRange(eventDict[EventClass.Network].Where(x => x.ProcessID == node.ProcessID).AsParallel().ToList());
                    node.EventsProcess.AddRange(eventDict[EventClass.Process].Where(x => x.ProcessID == node.ProcessID).AsParallel().ToList());
                    node.EventsProfiling.AddRange(eventDict[EventClass.Profiling].Where(x => x.ProcessID == node.ProcessID).AsParallel().ToList());
                    node.EventsRegistry.AddRange(eventDict[EventClass.Registry].Where(x => x.ProcessID == node.ProcessID).AsParallel().ToList());

                    return;
                }

                foreach (var child in node.Children)
                {
                    recAdjustNodes(child, finder, eventDict);
                }

            }

            var keys = FlattenRecursive(procMon).ToList();
            foreach (var item in keys)
            {
                recAdjustNodes(procMon, item, eventDict);
            }

            List<ProcMon> temp = [procMon];
            return temp;
        }
    }
}
