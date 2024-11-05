using ProcDotNet.Classes;

namespace ProcDotNet
{
    public class Support
    {


        internal static bool FileSystemCheck(ProcMon record)
        {
            if (FileSystemEvent.Events.Contains(record.Operation, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        internal static bool NetworkCheck(ProcMon record)
        {
            if (NetworkEvent.Events.Contains(record.Operation, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        internal static bool ProcessCheck(ProcMon record)
        {
            if (ProcessEvent.Events.Contains(record.Operation, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        internal static bool ProfileCheck(ProcMon record)
        {
            if (ProcessEvent.Events.Contains(record.Operation, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        internal static bool RegistryCheck(ProcMon record)
        {
            if (RegistryEvent.Events.Contains(record.Operation, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        internal static bool FileSystemCheckOld(ProcMon record)
        {
            if (record.EventClass.Equals(EventClass.FileSystem, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        internal static bool NetworkCheckOld(ProcMon record)
        {
            if (record.EventClass.Equals(EventClass.Network, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        internal static bool ProcessCheckOld(ProcMon record)
        {
            if (record.EventClass.Equals(EventClass.Process, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        internal static bool ProfileCheckOld(ProcMon record)
        {
            if (record.EventClass.Equals(EventClass.Profiling, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        internal static bool RegistryCheckOld(ProcMon record)
        {
            if (record.EventClass.Equals(EventClass.Registry, StringComparison.OrdinalIgnoreCase))
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

        internal static string ParseTime(ProcMon procMon)
        {
            string properTime = string.Empty;
            string rem = string.Empty;
            int partial = procMon.DateAndTime.Hour;
            if (procMon.TimeOfDay.Contains("PM"))
            {
                var fix = partial + ":" + procMon.TimeOfDay.Split(':', 2)[1];
                rem = fix.Substring(0, fix.Length - 3);
                //properTime = procMon.DateAndTime.ToShortDateString() + " " + rem;
            }
            if (procMon.TimeOfDay.Contains("AM"))
            {
                rem = procMon.TimeOfDay.Substring(0, procMon.TimeOfDay.Length - 3);
                //properTime = procMon.DateAndTime.ToShortDateString() + " " + rem;
            }

            //DateTime result = DateTime.MinValue;
            //string format = "HH:mm:ss.fffffff";
            //try
            //{
            //    result = DateTime.ParseExact(rem, format, CultureInfo.InvariantCulture);
            //    //Console.WriteLine("{0} converts to {1}.", properTime, result.ToString());
            //}
            //catch (FormatException)
            //{
            //    Console.WriteLine("{0} is not in the correct format.", properTime);
            //}

            return rem;
        }

    }
}