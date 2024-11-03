using CsvHelper.Configuration.Attributes;
using System;

namespace ProcDotNet.Classes
{
    public class ProcMon
    {
        /// <summary>
        /// POST Process
        /// </summary>
        public bool isRegistry = false;
        public bool isFileSystem = false;
        public bool isNetwork = false;
        public bool isProcess = false;
        public bool isProfiling = false;


        /// <summary>
        /// RAW Processed Attributes
        /// </summary>
        //Application Data
        [Name("Process Name")]
        public required string ProcessName { get; set; }
        [Name("Image Path")]
        public required string ImagePath { get; set; }
        [Name("Command Line")]
        public string? CommandLine { get; set; }
        [Name("Company")]
        public string? Company { get; set; }
        [Name("Description")]
        public string? Description { get; set; }
        [Name("Version")]
        public string? Version { get; set; }
        [Name("Architecture")]
        public string? Architecture { get; set; }

        //Event Details
        [Name("Sequence")]
        public string? SequenceNumber { get; set; }

        [Name("Event Class")]
        public required string EventClass { get; set; }

        [Name("Operation")]
        public required string Operation { get; set; }

        [Name("Date & Time")]
        public required DateTime DateAndTime { get; set; }

        [Name("Time of Day")]
        public required string TimeOfDay { get; set; }

        [Name("Category")]
        public string? Category { get; set; }

        [Name("Path")]
        public string? Path { get; set; }

        [Name("Detail")]
        public string? Detail { get; set; }

        [Name("Result")]
        public string? Result { get; set; }

        [Name("Relative Time")]
        public DateTime? RelativeTime { get; set; }

        [Name("Duration")]
        public string? Duration { get; set; }

        [Name("Completion Time")]
        public DateTime? CompletionTime { get; set; }

        //Process Management
        [Name("User")]
        public string? UserName { get; set; }
        [Name("Session")]
        public int? SessionID { get; set; }
        [Name("Authentication ID")]
        public string? AuthenticationID { get; set; }
        [Name("Integrity")]
        public string? Integrity { get; set; }
        [Name("PID")]
        public required int ProcessID { get; set; }
        [Name("TID")]
        public int? ThreadID { get; set; }
        [Name("Parent PID")]
        public required int ParentPID { get; set; }
        [Name("Virtualized")]
        public string? Virtualized { get; set; }

    }


}
