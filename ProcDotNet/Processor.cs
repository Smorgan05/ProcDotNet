// See https://aka.ms/new-console-template for more information


using CsvHelper;
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
            var UniqueProcs = ProfileEvents.Select(x => x.ProcessName).Distinct().ToList();
            Dictionary<string, List<ProcMon>> ProcessBuckets = ProcessSorter(ProfileEvents, UniqueProcs);
            return ProcessBuckets;
        }

        internal static Dictionary<ProcMon, List<ProcMon>> GetProcessBucketGroups(Dictionary<string, List<ProcMon>> processBuckets)
        {
            Dictionary<ProcMon, List<ProcMon>> result = new Dictionary<ProcMon, List<ProcMon>>();

            // iterate through all Processes 
            foreach (var item in processBuckets)
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
        /// Map Launch Process (Specific) to Child Processes
        /// </summary>
        /// <param name="sortedProcessBuckets"></param>
        /// <param name="Process"></param>
        /// <returns>
        /// Dictionary Parent Process, Child Processes
        /// </returns>
        internal static Dictionary<ProcMon, List<ProcMon>> DictProcessMapper(Dictionary<string, List<ProcMon>> sortedProcessBuckets, string Process)
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