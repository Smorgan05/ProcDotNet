// See https://aka.ms/new-console-template for more information


namespace ProcNet
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

        internal static void DictionaryPrinter(Dictionary<string, List<ProcMon>> sortedProcessBuckets)
        {
            foreach (var procMon in sortedProcessBuckets)
            {
                Console.WriteLine();
                Console.WriteLine("Process: " + procMon.Key);
                foreach (var process in procMon.Value)
                {
                    Console.WriteLine(process.ProcessName + ": " + process.ProcessID);
                }
            }
        }

        internal static void DictionaryPrinter(Dictionary<ProcMon, List<ProcMon>> processDictionary)
        {
            foreach (var procMon in processDictionary)
            {
                Console.WriteLine();
                Console.WriteLine(procMon.Key.ProcessName + ": " + procMon.Key.ParentPID + ": " + procMon.Key.ProcessID);
                foreach (var process in procMon.Value)
                {
                    Console.WriteLine(process.ProcessName + ": " + process.ParentPID + ": " + process.ProcessID);
                }
            }
        }

        internal static void Printer(List<ProcMon> linkedProc)
        {
            Console.WriteLine("Nested Process List:");
            foreach (ProcMon item in linkedProc)
            {
                Console.WriteLine(item.ProcessName);
            }
        }
    }
}