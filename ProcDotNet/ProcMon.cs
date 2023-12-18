using System;

public class Data
{
    public ProcMon()
    {
        public AppData appData { get; set; }
        public EventDetails eventDetails { get; set; }
        public ProcessManage processManage { get; set; }
    }

	public AppData()
	{
        [Name("Process Name")]
        public string ProcessName { get; set; }
        [Name("Image Path")]
        public string ImagePath { get; set; }
        [Name("Command Line")]
        public string CommandLine { get; set; }
        [Name("Company")]
        public string Company { get; set; }
        [Name("Description")]
        public string Description { get; set; }
        [Name("Version")]
        public string Version { get; set; }
        [Name("Architecture")]
        public string Architecture { get; set; }
    }

	public EventDetails()
	{
        [Name("Sequence")]
        public int SequenceNumber { get; set; }
        [Name("Event Class")]
        public int EventClass { get; set; }
        [Name("Operation")]
        public int Operation { get; set; }
        [Name("Date & Time")]
        public int DateAndTime { get; set; }
        [Name("Time of Day")]
        public string Time { get; set; }
        [Name("Category")]
        public string Category { get; set; }
        [Name("Path")]
        public int Path { get; set; }
        [Name("Detail")]
        public int Detail { get; set; }
        [Name("Result")]
        public int Result { get; set; }
        [Name("Relative Time")]
        public int RelativeTime { get; set; }
        [Name("Duration")]
        public int Duration { get; set; }
        [Name("Completion Time")]
        public int CompletionTime { get; set; }
    }

	public ProcessManage()
	{
        [Name("User")]
        public int UserName { get; set; }
        [Name("Session")]
        public int SessionID { get; set; }
        [Name("Authentication ID")]
        public int AuthenticationID { get; set; }
        [Name("Integrity")]
        public int Integrity { get; set; }
        [Name("PID")]
        public int ProcessID { get; set; }
        [Name("TID")]
        public int ThreadID { get; set; }
        [Name("Parent PID")]
        public int ParentPID { get; set; }
        [Name("Virtualized")]
        public int Virtualized { get; set; }

    }
}
