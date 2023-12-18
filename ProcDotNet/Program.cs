// See https://aka.ms/new-console-template for more information


using CsvHelper;
using Microsoft.Win32;
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

            // Load ProcMon CSV (with Fixed Times)
            var ProcessBuckets = Processor.LoadCSV(testPath);

            //Sort by number of processes
            Dictionary<string, List<ProcMon>> sortedProcessBuckets = ProcessBuckets.OrderByDescending(x => x.Value.Capacity).ToDictionary(x => x.Key, x => x.Value);
            Dictionary<ProcMon, List<ProcMon>> ProcessBucketGroups = Processor.GetProcessBucketGroups(ProcessBuckets);

            //Sort by Time of Day
            var timeOfDayBuckets = ProcessBucketGroups.OrderByDescending(x => x.Key.TimeOfDay).ToDictionary(x => x.Key, x => x.Value);

            // Linked List
            var test = PrepList(timeOfDayBuckets);

            //Test.DictionaryPrinter(timeOfDayBuckets);

            // Process Mapper Proper (Specific) explorer.exe -> Many
            List<ProcMon> ProcMaps = ProcessMapper(sortedProcessBuckets, parentProc);
            Dictionary<ProcMon, List<ProcMon>> DictProcMaps = Processor.DictProcessMapper(sortedProcessBuckets, parentProc);

            // Find Parent Procss from bucket brave.exe -> sub braves
            var temp = ParentChildMapper(sortedProcessBuckets, process);

            //Test Print Method
            Console.WriteLine("Unique Processes: " + sortedProcessBuckets.Count);
            //Test.DictionaryPrinter(sortedProcessBuckets);
            //Test.Printer(childProcs);
        }

        /// <summary>
        /// Get Process Mapping for Bucket Group <Brave.exe>
        /// (Specific) brave.exe -> sub braves
        /// </summary>
        /// <param name="sortedProcessBuckets"></param>
        /// <param name="subProcess"></param>
        /// <returns></returns>
        private static Dictionary<ProcMon, List<ProcMon>> ParentChildMapper(Dictionary<string, List<ProcMon>> sortedProcessBuckets, string subProcess)
        {
            // Get specific Processes
            List<ProcMon> testFilter = sortedProcessBuckets.First(x => x.Key.Equals(subProcess, StringComparison.OrdinalIgnoreCase)).Value;
            Dictionary<ProcMon, List<ProcMon>> result = new Dictionary<ProcMon, List<ProcMon>>();

            // Get Parents
            foreach (ProcMon currProc in testFilter)
            {
                //Console.WriteLine(currProc.ParentPID + " " + currProc.ProcessID);
                var temp = testFilter.Where(x => x.ProcessID == currProc.ParentPID).ToList();       
                if (temp.Count > 0)
                {
                    // Map Parents to Children
                    var parent = temp.First();
                    var proclist = new List<ProcMon>();
                    foreach(var item in temp)
                    {
                        proclist = testFilter.Where(x => x.ParentPID == item.ProcessID).ToList();
                    }

                    result.TryAdd(parent, proclist);
                }
            }

            return result;
        }

        /// <summary>
        /// Map Launch Process (specific) to Child Processes
        /// (Specific) explorer.exe -> Many
        /// </summary>
        /// <param name="sortedProcessBuckets"></param>
        /// <param name="Process"></param>
        /// <returns></returns>
        private static List<ProcMon> ProcessMapper(Dictionary<string, List<ProcMon>> processBuckets, string Process)
        {
            ProcMon? parent = processBuckets.First(x => x.Key.Equals(Process, StringComparison.OrdinalIgnoreCase)).Value.FirstOrDefault();
            List<ProcMon> result = new();
            if (parent != null)
            {
                foreach (var item in processBuckets.Values)
                {
                    foreach (var process in item)
                    {
                        if (process.ParentPID == parent.ProcessID)
                        {
                            result.Add(process);
                        }
                    }
                }

            }
            return result;
        }

        private static Dictionary<ProcMon, List<ProcMon>> ProcessMapperDict(Dictionary<ProcMon, List<ProcMon>> processBuckets, ProcMon Process)
        {
            ProcMon? parent = processBuckets.First(x => x.Key.ProcessName.Equals(Process.ProcessName, StringComparison.OrdinalIgnoreCase)).Value.FirstOrDefault();
            List<ProcMon> temp = new();
            if (parent != null)
            {
                foreach (var item in processBuckets.Values)
                {
                    foreach (var process in item)
                    {
                        if (process.ParentPID == parent.ProcessID)
                        {
                            temp.Add(process);
                        }
                    }
                }

            }

            // Return
            Dictionary<ProcMon, List<ProcMon>> result = new();
            if (parent != null)
            {
                // Cleanup
                var check = temp.Contains(parent);
                if (check == true)
                {
                    temp.Remove(parent);
                }

                result.Add(parent, temp);
            }
            return result;
        }

        private static Dictionary<string, List<ProcMon>> PrepList(Dictionary<ProcMon, List<ProcMon>> processBuckets)
        {
            Dictionary<string, List<ProcMon>> result = new();
            foreach (var proc in processBuckets.Keys)
            {
                // Check for Children <parent, children<List>> for specific process
                Dictionary<ProcMon, List<ProcMon>> temp = ProcessMapperDict(processBuckets, proc);
                if (temp.Values.Count > 0)
                {

                }

                // Has No Children
                else
                {

                }

            }
            return result;
        }
    }

}
