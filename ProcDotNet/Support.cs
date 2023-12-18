// See https://aka.ms/new-console-template for more information


using System.Globalization;

namespace ProcNet
{
    internal class Support
    {
        internal static DateTime ParseTime(ProcMon procMon)
        {
            string properTime = string.Empty;
            int partial = procMon.DateAndTime.Hour;
            if (procMon.TimeOfDay.Contains("PM"))
            {
                var fix = partial + procMon.TimeOfDay.Remove(0, 1);
                var rem = fix.Substring(0, fix.Length-3);
                properTime = procMon.DateAndTime.ToShortDateString() + " " + rem;
            }
            if (procMon.TimeOfDay.Contains("AM"))
            {
                var rem = procMon.TimeOfDay.Substring(0, procMon.TimeOfDay.Length - 3);
                properTime = procMon.DateAndTime.ToShortDateString() + " " + rem;
            }

            DateTime result = DateTime.MinValue;
            string format = "MM/dd/yyyy HH:mm:ss.fffffff";
            try
            {
                result = DateTime.ParseExact(properTime, format, CultureInfo.InvariantCulture);
                //Console.WriteLine("{0} converts to {1}.", properTime, result.ToString());
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not in the correct format.", properTime);
            }

            return result;
        }


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

        internal static List<ProcMon> FixTime(List<ProcMon> records)
        {
            List<ProcMon> result = new(records);
            foreach (var item in result)
            {
                item.TimeOfDay = ParseTime(item).ToString();
            }
            return result;
        }
    }
}