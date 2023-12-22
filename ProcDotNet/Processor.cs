// See https://aka.ms/new-console-template for more information


using CsvHelper;
using System.Diagnostics;
using System.Globalization;

namespace ProcNet
{
    internal class Processor
    {
        internal static Dictionary<string, List<ProcMon>> LoadCSV(string testPath)
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
            PostProcess(recordsTimeFix);

            ////Get EventClass Lists
            List<ProcMon> FileSystemEvents = recordsTimeFix.Where(rec => rec.isFileSystem.Equals(true)).ToList();
            List<ProcMon> NetworkEvents = recordsTimeFix.Where(rec => rec.isNetwork.Equals(true)).ToList();
            List<ProcMon> ProcessEvents = recordsTimeFix.Where(rec => rec.isProcess.Equals(true)).ToList();
            List<ProcMon> ProfileEvents = recordsTimeFix.Where(rec => rec.isProfiling.Equals(true)).ToList();
            List<ProcMon> RegistryEvents = recordsTimeFix.Where(rec => rec.isRegistry.Equals(true)).ToList();

            //Gather Process Buckets
            Console.WriteLine("Gathering Unique Processes");
            List<string> UniqueProcs = ProfileEvents.Select(x => x.ProcessName).ToList();
            Dictionary<string, List<ProcMon>> ProcessBuckets = ProcessSorter(ProfileEvents, UniqueProcs);
            return ProcessBuckets;
        }

        internal static Dictionary<ProcMon, List<ProcMon>> GetProcessBucketGroups(Dictionary<string, List<ProcMon>> processBuckets)
        {
            Dictionary<ProcMon, List<ProcMon>> result = new Dictionary<ProcMon, List<ProcMon>>();

            // iterate through all Processes 
            foreach (var item in processBuckets)
            {
                Dictionary<ProcMon, List<ProcMon>> temp = DictProcessMapperNew(processBuckets, item.Key);
                foreach (var procMap in temp)
                {
                    result.Add(procMap.Key, procMap.Value);
                }
            }

            return result;
        }

        private static Dictionary<ProcMon, List<ProcMon>> DictProcessMapperNew(Dictionary<string, List<ProcMon>> sortedProcessBuckets, string key)
        {
            List<ProcMon> temp = new();
            Dictionary<ProcMon, List<ProcMon>> result = new();

            ProcMon? parent = sortedProcessBuckets.First(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Value.FirstOrDefault();



            throw new NotImplementedException();
        }

        /// <summary>
        /// Map Launch Process (Specific) to Child Processes
        /// </summary>
        /// <param name="sortedProcessBuckets"></param>
        /// <param name="Process"></param>
        /// <returns>
        /// Dictionary Parent Process, Child Processes
        /// </returns>
        internal static Dictionary<ProcMon, List<ProcMon>> DictProcessMapper(Dictionary<string, List<ProcMon>> sortedProcessBuckets, string Process)
        {
            List<ProcMon> temp = new();
            Dictionary<ProcMon, List<ProcMon>> result = new();

            ProcMon? parent = sortedProcessBuckets.First(x => x.Key.Equals(Process, StringComparison.OrdinalIgnoreCase)).Value.FirstOrDefault();
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

        private static Dictionary<string, List<ProcMon>> ProcessSorter(List<ProcMon> records, List<string> uniqueProcs)
        {
            Dictionary<string, List<ProcMon>> tempRes = new();
            Dictionary<string, List<ProcMon>> result = new();
            foreach (var proc in uniqueProcs)
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