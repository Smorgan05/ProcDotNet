﻿using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

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
        /// SubProcs / Profiling
        /// </summary>
        public List<ProcMon> Children { get; set; } = new List<ProcMon>();
        public ProcMon Parent { get; set; }

        /// <summary>
        /// Various Operations
        /// </summary>
        public List<ProcMon> EventsRegistry { get; set; } = new List<ProcMon>();
        public List<ProcMon> EventsFileSystem { get; set; } = new List<ProcMon>();
        public List<ProcMon> EventsNetwork { get; set; } = new List<ProcMon>();
        public List<ProcMon> EventsProcess { get; set; } = new List<ProcMon>();
        public List<ProcMon> EventsProfiling { get; set; } = new List<ProcMon>();

        /// <summary>
        /// RAW Processed Attributes
        /// </summary>
        //Application Data
        [Name("Process Name"), Required]
        public string ProcessName { get; set; }
        [Name("Image Path")]
        public string ImagePath { get; set; }
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

        [Name("Event Class"), Required]
        public string EventClass { get; set; }

        [Name("Operation"), Required]
        public string Operation { get; set; }

        [Name("Date & Time"), Required]
        public DateTime DateAndTime { get; set; }

        [Name("Time of Day"), Required]
        public string TimeOfDay { get; set; }

        [Name("Category")]
        public string? Category { get; set; }

        [Name("Path")]
        public string? Path { get; set; }

        [Name("Detail"), Required]
        public string? Detail { get; set; }

        [Name("Result"), Required]
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
        public int? Session { get; set; }
        [Name("Authentication ID")]
        public string? AuthenticationID { get; set; }
        [Name("Integrity")]
        public string? Integrity { get; set; }
        [Name("PID")]
        public int ProcessID { get; set; }
        [Name("TID")]
        public int? ThreadID { get; set; }
        [Name("Parent PID"), Required]
        public int ParentPID { get; set; }
        [Name("Virtualized")]
        public string? Virtualized { get; set; }

    }


}
