﻿// See https://aka.ms/new-console-template for more information


using ProcDotNet.Classes;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace ProcDotNetTester
{
    internal class Test
    {
        internal static void BucketPrinter(Dictionary<string, List<ProcMon>> processBuckets)
        {
            foreach (var item in processBuckets)
            {
                Console.WriteLine(item.Key);
                foreach (var proc in item.Value)
                {
                    Console.WriteLine(proc.ProcessName + " " + proc.ProcessID);
                }
            }
        }

        internal static void DictionaryPrinter(Dictionary<string, List<ProcMon>> sortedProcessBuckets, string EventClass)
        {
            List<ProcMon> List = sortedProcessBuckets[EventClass];
            foreach (var item in List)
            {
                Console.WriteLine(item.SequenceNumber + " " + EventClass + ": " + item.ProcessName + " " + item.Operation + " " + item.Path);
            }

        }

        internal static void DictionaryPrinter(Dictionary<ProcMon, List<ProcMon>> processDictionary)
        {
            foreach (var procMon in processDictionary)
            {
                Console.WriteLine();
                Console.WriteLine("Parent: " + procMon.Key.ProcessName + ": " + procMon.Key.ParentPID + ": " + procMon.Key.ProcessID);
                foreach (var process in procMon.Value)
                {
                    Console.WriteLine(process.ProcessName + ": " + process.ParentPID + ": " + process.ProcessID);
                }
            }
        }

        internal static void KeyValuePrinter(List<KeyValuePair<ProcMon, List<ProcMon>>> processBucketGroups)
        {
            foreach (var process in processBucketGroups)
            {
                Console.WriteLine();
                Console.WriteLine(process.Key.ProcessName + " Parent ID: " + process.Key.ParentPID + ": Process ID: " + process.Key.ProcessID);
                foreach (var item in process.Value)
                {
                    Console.WriteLine("-" + item.ProcessName + " Parent ID: " + item.ParentPID + ": Process ID: " + item.ProcessID);
                }

            }
        }

        internal static void RecNodeListPrinter(List<ProcMon> processNodes)
        {
            foreach (var process in processNodes)
            {
                Console.WriteLine();
                RecNodePrinter(process);
            }

        }

        internal static void RecNodePrinter(ProcMon tree, string indent = "", bool last = false)
        {
            Console.WriteLine(indent + "+- " + tree.ProcessName + ": PPID: " + tree.ParentPID + ": PID: " + tree.ProcessID);
            indent += last ? "   " : "|  ";

            for (int i = 0; i < tree.Children.Count; i++)
            {
                RecNodePrinter(tree.Children.ToList()[i], indent, i == tree.Children.ToList().Count - 1);
            }
        }

        internal static void Printer(List<ProcMon> linkedProc)
        {
            Console.WriteLine("Process List:");
            foreach (ProcMon item in linkedProc)
            {
                Console.WriteLine(item.TimeOfDay + ": " + item.ProcessName);
            }
        }
    }
}