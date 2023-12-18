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

            // Raw Processing
            Console.WriteLine("Processing CSV: " + testPath);
            using var procLog = new StreamReader(testPath);
            using var csv = new CsvReader(procLog, CultureInfo.InvariantCulture);
            List<ProcMon> records = csv.GetRecords<ProcMon>().ToList();

            //Get Types
            Console.WriteLine("Categorizing Event Classes");
            PostProcess(records);

            //Get EventClass Lists
            List<ProcMon> FileSystemEvents = records.Where(rec => rec.isFileSystem.Equals(true)).ToList();
            List<ProcMon> NetworkEvents = records.Where(rec => rec.isNetwork.Equals(true)).ToList();
            List<ProcMon> ProcessEvents = records.Where(rec => rec.isProcess.Equals(true)).ToList();
            List<ProcMon> ProfileEvents = records.Where(rec => rec.isProfiling.Equals(true)).ToList();
            List<ProcMon> RegistryEvents = records.Where(rec => rec.isRegistry.Equals(true)).ToList();

            //Gather Process Buckets
            Console.WriteLine("Gathering Unique Processes");
            var UniqueProcs = ProfileEvents.Select(x => x.ProcessName).Distinct().ToList();
            Dictionary<string, List<ProcMon>> ProcessBuckets = ProcessSorter(ProfileEvents, UniqueProcs);

            //Sort by number of processes
            Dictionary<string, List<ProcMon>> sortedProcessBuckets = ProcessBuckets.OrderByDescending(x => x.Value.Capacity).ToDictionary(x => x.Key, x => x.Value);

            //Sort by Time of Day <Next>
            var timeSortedProcessBuckets = GetTimeSortProcess(ProcessBuckets);

            //Get Process Tree (Linked List)
            //var ParentProcesses = GetParentProcesses(sortedProcessBuckets);

            // Process Mapper Proper (Specific) explorer.exe -> Many
            //List<ProcMon> ProcMaps = ProcessMapper(sortedProcessBuckets, parentProc);
            var DictProcMaps = DictProcessMapper(sortedProcessBuckets, parentProc);

            // Find Parent Procss from bucket brave.exe -> sub braves
            var temp = ParentChildMapper(sortedProcessBuckets, process);

            // Linked List
            var test = PrepList(sortedProcessBuckets);

            //Test Print Method
            Console.WriteLine("Unique Processes: " + sortedProcessBuckets.Count);
            //Test.DictionaryPrinter(sortedProcessBuckets);
            //Test.Printer(childProcs);
        }

        private static object GetTimeSortProcess(Dictionary<string, List<ProcMon>> processBuckets)
        {
            Dictionary<ProcMon, List<ProcMon>> result = new Dictionary<ProcMon, List<ProcMon>>();

            // iterate through all Processes
            foreach (var item in  processBuckets)
            {
                var temp = DictProcessMapper(processBuckets, item.Key);
                foreach (var procMap in temp)
                {
                    result.Add(procMap.Key, procMap.Value);
                }
            }

            return result;
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
        private static List<ProcMon> ProcessMapper(Dictionary<string, List<ProcMon>> sortedProcessBuckets, string Process)
        {
            ProcMon? parent = sortedProcessBuckets.First(x => x.Key.Equals(Process, StringComparison.OrdinalIgnoreCase)).Value.FirstOrDefault();
            List<ProcMon> result = new();
            if (parent != null)
            {
                foreach (var item in sortedProcessBuckets.Values)
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

        /// <summary>
        /// Map Launch Process (Specific) to Child Processes
        /// </summary>
        /// <param name="sortedProcessBuckets"></param>
        /// <param name="Process"></param>
        /// <returns>
        /// Dictionary Parent Process, Child Processes
        /// </returns>
        private static Dictionary<ProcMon, List<ProcMon>> DictProcessMapper(Dictionary<string, List<ProcMon>> sortedProcessBuckets, string Process)
        {
            ProcMon? parent = sortedProcessBuckets.First(x => x.Key.Equals(Process, StringComparison.OrdinalIgnoreCase)).Value.FirstOrDefault();
            List<ProcMon> temp = new();
            Dictionary<ProcMon, List<ProcMon>> result = new();
            if (parent != null)
            {
                foreach (var item in sortedProcessBuckets.Values)
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

            // Null Check then Return
            if (parent != null)
            {
                result.Add(parent, temp);
            }

            return result;
        }

        private static Dictionary<string, List<ProcMon>> PrepList(Dictionary<string, List<ProcMon>> processBuckets)
        {
            Dictionary<string, List<ProcMon>> result = new();
            foreach (var proc in processBuckets.Keys)
            {
                // Check for Children <parents, children<List>> for specific process
                List<ProcMon> temp = ProcessMapper(processBuckets, proc);

                if (temp.Capacity > 0)
                {

                }

                // Orphaned Process
                else
                {

                }

            }
            return result;
        }

        private static Dictionary<string, List<ProcMon>> ProcessSorter(List<ProcMon> records, List<string> uniqueProcs)
        {
            Dictionary<string, List<ProcMon>> result = new();
            foreach (var proc in uniqueProcs)
            {
                // List of Duplicates
                List<ProcMon> temp = records.FindAll(x => x.ProcessName.Equals(proc, StringComparison.OrdinalIgnoreCase)).ToList();
                
                // List of Unique Process IDs
                List<int> uniqueIDs = temp.Select(a => a.ProcessID).Distinct().ToList();
                List<ProcMon> uniqueTemp = new();

                // Filter by Process IDs
                foreach (int ID in uniqueIDs)
                {
                    var singleProcess = temp.FirstOrDefault(x => x.ProcessID == ID);
                    if (singleProcess != null)
                    {
                        uniqueTemp.Add(singleProcess);
                    }
                }
                result.Add(proc, uniqueTemp);
            }
            return result;
        }

        /// <summary>
        /// Set Event Types
        /// </summary>
        /// <param name="records"></param>
        /// <exception cref="NotImplementedException"></exception>
        private static void PostProcess(List<ProcMon> records)
        {
            foreach (ProcMon record in records)
            {
                record.isFileSystem = Support.FileSystemCheck(record);
                record.isNetwork = Support.NetworkCheck(record);
                record.isProcess = Support.ProcessCheck(record);
                record.isProfiling = Support.ProfileCheck(record);
                record.isRegistry = Support.RegistryCheck(record);
            }
        }
    }

}
