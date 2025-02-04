﻿using CsvHelper;
using ProcDotNet.Classes;
using System.Globalization;

namespace ProcDotNet
{
    public class Processor
    {
        /// <summary>
        /// Process Tree Maker that takes ProcMon CSV File to parse into Process Tree Structure
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<ProcMon> ProcessTreeMaker(string filePath)
        {
            // Load ProcMon CSV (with Fixed Times)
            Dictionary<string, List<ProcMon>> ProcessDicts = LoadLists(filePath);
            //Dictionary<string, List<ProcMon>> ProcessDicts = LoadListsNew(filePath);

            // Load Buckets
            Dictionary<string, List<ProcMon>> ProcessBuckets = ProcessSorter(ProcessDicts[EventClass.Profiling]);

            //Get Process Buckets
            List<KeyValuePair<ProcMon, List<ProcMon>>> ProcessBucketGroups = GetProcessBucketGroups(ProcessBuckets);

            // Map Disparate Processes
            List<KeyValuePair<ProcMon, List<ProcMon>>> ProcMaps = GetInterProcMapping(ProcessBucketGroups);

            // Map KVPs to Process Nodes (Good)
            List<ProcMon> ProcessNodes = NodeProcessor.GetTreeList(ProcMaps);

            // Inter Node Mapping (Good)
            return NodeProcessor.MakeTreeList(ProcessNodes);
        }

        public static List<string> GetUniqueProcessList(string filePath)
        {
            // Load ProcMon CSV (with Fixed Times)
            Dictionary<string, List<ProcMon>> ProcessDicts = LoadLists(filePath);

            // Load Buckets
            Dictionary<string, List<ProcMon>> ProcessBuckets = ProcessSorter(ProcessDicts[EventClass.Profiling]);

            return ProcessBuckets.Keys.ToList();
        }

        /// <summary>
        /// Process and Categorize CSV into Event Types
        /// </summary>
        /// <param name="testPath"></param>
        /// <returns></returns>
        public static Dictionary<string, List<ProcMon>> LoadLists(string testPath)
        {
            // Raw Processing
            Console.WriteLine("Processing CSV: " + testPath);
            using var procLog = new StreamReader(testPath);
            using var csv = new CsvReader(procLog, CultureInfo.InvariantCulture);
            List<ProcMon> records = csv.GetRecords<ProcMon>().ToList();

            // Time of Date Parsing Fix
            List<ProcMon> recordsTimeFix = Support.FixTime(records);

            //Get Types
            Console.WriteLine("Categorizing Event Classes");
            List<ProcMon> recordsFixed = PostProcessOld(records);

            //Get EventClass Lists
            List<ProcMon> FileSystemEvents = recordsFixed.Where(rec => rec.isFileSystem.Equals(true)).ToList();
            List<ProcMon> NetworkEvents = recordsFixed.Where(rec => rec.isNetwork.Equals(true)).ToList();
            List<ProcMon> ProcessEvents = recordsFixed.Where(rec => rec.isProcess.Equals(true)).ToList();
            List<ProcMon> ProfileEvents = recordsFixed.Where(rec => rec.isProfiling.Equals(true)).ToList();
            List<ProcMon> RegistryEvents = recordsFixed.Where(rec => rec.isRegistry.Equals(true)).ToList();

            // Build Return
            Dictionary<string, List<ProcMon>> result = new Dictionary<string, List<ProcMon>>
            {
                { EventClass.FileSystem, FileSystemEvents },
                { EventClass.Network, NetworkEvents },
                { EventClass.Process, ProcessEvents },
                { EventClass.Profiling, ProfileEvents }, // Process Tree
                { EventClass.Registry, RegistryEvents },
            };

            return result;
        }

        /// <summary>
        /// Process and Categorize CSV into Event Types
        /// </summary>
        /// <param name="testPath"></param>
        /// <returns></returns>
        public static Dictionary<string, List<ProcMon>> LoadListsNew(string testPath)
        {
            // Raw Processing
            Console.WriteLine("Processing CSV: " + testPath);
            using var procLog = new StreamReader(testPath);
            using var csv = new CsvReader(procLog, CultureInfo.InvariantCulture);
            List<ProcMon> records = csv.GetRecords<ProcMon>().ToList();

            // Time of Date Parsing Fix
            List<ProcMon> recordsTimeFix = Support.FixTime(records);

            //Get Types
            Console.WriteLine("Categorizing Event Classes");
            List<ProcMon> recordsFixed = PostProcess(records);

            //Get EventClass Lists
            List<ProcMon> FileSystemEvents = recordsFixed.Where(rec => rec.isFileSystem.Equals(true)).ToList();
            List<ProcMon> NetworkEvents = recordsFixed.Where(rec => rec.isNetwork.Equals(true)).ToList();
            List<ProcMon> ProcessEvents = recordsFixed.Where(rec => rec.isProcess.Equals(true)).ToList();
            List<ProcMon> ProfileEvents = recordsFixed.Where(rec => rec.isProfiling.Equals(true)).ToList();
            List<ProcMon> RegistryEvents = recordsFixed.Where(rec => rec.isRegistry.Equals(true)).ToList();

            // Build Return
            Dictionary<string, List<ProcMon>> result = new Dictionary<string, List<ProcMon>>
            {
                { EventClass.FileSystem, FileSystemEvents },
                { EventClass.Network, NetworkEvents },
                { EventClass.Process, ProcessEvents },
                { EventClass.Profiling, ProfileEvents }, // Process Tree
                { EventClass.Registry, RegistryEvents },
            };

            return result;
        }

        /// <summary>
        /// Get Process Buckes for initial sorting
        /// </summary>
        /// <param name="processBuckets"></param>
        /// <returns></returns>
        internal static List<KeyValuePair<ProcMon, List<ProcMon>>> GetProcessBucketGroups(Dictionary<string, List<ProcMon>> processBuckets)
        {
            List<KeyValuePair<ProcMon, List<ProcMon>>> result = new();

            // iterate through all Processes 
            foreach (var item in processBuckets)
            {
                List<KeyValuePair<ProcMon, List<ProcMon>>> temp = DictProcessMapper(processBuckets, item.Key);
                foreach (var procMap in temp)
                {
                    result.Add(procMap);
                }
            }

            return result;
        }

        /// <summary>
        /// Map Processes to each other
        /// </summary>
        /// <param name="processBucketGroups"></param>
        /// <returns></returns>
        internal static List<KeyValuePair<ProcMon, List<ProcMon>>> GetInterProcMapping(List<KeyValuePair<ProcMon, List<ProcMon>>> processBucketGroups)
        {

            List<KeyValuePair<ProcMon, List<ProcMon>>> result = new();

            // Conversions
            List<KeyValuePair<ProcMon, List<ProcMon>>> singlesKeyVal = processBucketGroups.Where(x => x.Value.Count == 0).ToList();
            //List<ProcMon> singlesList = singlesKeyVal.ToDictionary(x => x.Key, x => x.Value).Keys.ToList();
            List<ProcMon> singlesList = singlesKeyVal.Select(x => x.Key).ToList();

            // Parent
            foreach (var item in singlesList)
            {
                List<ProcMon> select = singlesList.Where(x => x.ParentPID == item.ProcessID).DistinctBy(x => x.ProcessID).ToList();
                var element = new KeyValuePair<ProcMon, List<ProcMon>>(item, select);
                if (!result.Contains(element))
                {
                    result.Add(element);
                }

            }

            // Sloppy Merge
            var groups = result.Where(x => x.Value.Count > 0).ToList();
            var singles = result.Where(x => x.Value.Count == 0).ToList();
            var Merge = GetMerge(groups, singles);

            // Merge with Groups
            result = processBucketGroups.Where(x => x.Value.Count != 0).ToList();
            result.AddRange(Merge);

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
        internal static List<KeyValuePair<ProcMon, List<ProcMon>>> DictProcessMapper(Dictionary<string, List<ProcMon>> sortedProcessBuckets, string Process)
        {
            List<KeyValuePair<ProcMon, List<ProcMon>>> result = new();
            List<ProcMon> tempRes = new List<ProcMon>();

            var processes = sortedProcessBuckets.First(x => x.Key.Equals(Process)).Value;
            List<ProcMon> parents = processes.DistinctBy(x => x.ProcessID).ToList();

            foreach (var par in parents)
            {
                tempRes = Childfinder(processes, par);

                //Console.WriteLine(par.ProcessName + " " + par.ProcessID + " " + par.ParentPID);
                var element = new KeyValuePair<ProcMon, List<ProcMon>>(par, tempRes);
                result.Add(element);
            }

            // Sloppy Merge
            var groups = result.Where(x => x.Value.Count > 0).ToList();
            var singles = result.Where(x => x.Value.Count == 0).ToList();
            var Merge = GetMerge(groups, singles);

            return Merge;
        }

        /// <summary>
        /// Merge Value Pairs and ensure that are no duplicates
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="singles"></param>
        /// <returns></returns>
        internal static List<KeyValuePair<ProcMon, List<ProcMon>>> GetMerge(List<KeyValuePair<ProcMon, List<ProcMon>>> groups, List<KeyValuePair<ProcMon, List<ProcMon>>> singles)
        {
            List<KeyValuePair<ProcMon, List<ProcMon>>> result = new(groups);
            List<KeyValuePair<ProcMon, List<ProcMon>>> newSingles = new(singles);

            // Single Group
            foreach (var proc in groups)
            {
                // Check
                List<ProcMon> temp = proc.Value;
                foreach (var single in singles)
                {
                    // Check single for Match (remove)
                    if (temp.Contains(single.Key))
                    {
                        newSingles.Remove(single);
                    }
                }

            }

            result.AddRange(newSingles);
            return result;
        }

        /// <summary>
        /// Find ProcMon Object in Process List for mapping
        /// </summary>
        /// <param name="processes"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        private static List<ProcMon> Childfinder(List<ProcMon> processes, ProcMon par)
        {
            var result = new List<ProcMon>();
            foreach (var item in processes)
            {
                if (par.ProcessID == item.ParentPID)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Remove Duplicate ProcMon entries for Sorting
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        internal static Dictionary<string, List<ProcMon>> ProcessSorter(List<ProcMon> records)
        {
            //Gather Process Buckets
            Console.WriteLine("Gathering Unique Processes");
            List<string> UniqueProcs = records.Select(x => x.ProcessName).ToList();
            Dictionary<string, List<ProcMon>> tempRes = new();
            Dictionary<string, List<ProcMon>> result = new();
            foreach (var proc in UniqueProcs)
            {
                // List of Duplicates
                List<ProcMon> temp = records.FindAll(x => x.ProcessName.Equals(proc, StringComparison.OrdinalIgnoreCase)).ToList();
                var check = temp.DistinctBy(x => x.ProcessID).ToList();

                if (tempRes.ContainsKey(proc))
                {
                    tempRes[proc].AddRange(check);
                }
                else
                {
                    tempRes.Add(proc, check);
                }
            }

            //var test = tempRes["msedgewebview2.exe"].DistinctBy(x => x.ProcessID).ToList();

            foreach (var item in tempRes)
            {
                var list = item.Value.DistinctBy(x => x.ProcessID).ToList();
                result.Add(item.Key, list);
            }

            // Filter Result
            return result;
        }

        /// <summary>
        /// Set Event Types
        /// </summary>
        /// <param name="records"></param>
        /// <exception cref="NotImplementedException"></exception>
        private static List<ProcMon> PostProcessOld(List<ProcMon> records)
        {
            List<ProcMon> result = new List<ProcMon>(records);

            foreach (ProcMon record in result)
            {
                record.isFileSystem = Support.FileSystemCheckOld(record);
                record.isNetwork = Support.NetworkCheckOld(record);
                record.isProcess = Support.ProcessCheckOld(record);
                record.isProfiling = Support.ProfileCheckOld(record);
                record.isRegistry = Support.RegistryCheckOld(record);
            }

            return result;
        }

        /// <summary>
        /// Set Event Types
        /// </summary>
        /// <param name="records"></param>
        /// <exception cref="NotImplementedException"></exception>
        private static List<ProcMon> PostProcess(List<ProcMon> records)
        {
            List<ProcMon> result = new List<ProcMon>(records);

            foreach (ProcMon record in result)
            {
                record.isFileSystem = Support.FileSystemCheck(record);
                record.isNetwork = Support.NetworkCheck(record);
                record.isProcess = Support.ProcessCheck(record);
                record.isProfiling = Support.ProfileCheck(record);
                record.isRegistry = Support.RegistryCheck(record);
            }

            return result;
        }
    }
}