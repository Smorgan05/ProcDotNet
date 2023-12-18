// See https://aka.ms/new-console-template for more information


namespace ProcNet
{
    internal class Support
    {
        internal static bool FileSystemCheck(ProcMon record)
        {
            if (record.EventClass.Equals("File System", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        internal static bool NetworkCheck(ProcMon record)
        {
            if (record.EventClass.Equals("Network", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        internal static bool ProcessCheck(ProcMon record)
        {
            if (record.EventClass.Equals("Process", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        internal static bool ProfileCheck(ProcMon record)
        {
            if (record.EventClass.Equals("Profiling", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        internal static bool RegistryCheck(ProcMon record)
        {
            if (record.EventClass.Equals("Registry", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}