// See https://aka.ms/new-console-template for more information


using CsvHelper;
using Microsoft.Win32;
using ProcDotNet.Classes;
using ProcDotNet.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Sources;
using System.Xml.Linq;
using System.Xml.XPath;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProcDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            //string parentProc = "explorer.exe";
            //string process = "brave.exe";
            //string testPath = @"C:\Users\Dark\Documents\ProcDotNet Local\Logfile.CSV";
            //string testPathNew = @"C:\Users\Dark\Documents\ProcDotNet Local\Logfile_12_26_2023.CSV";

            // Load ProcMon CSV (with Fixed Times)

            // Process Tree Testing
            //var ProcTreeCheck = Support.ProcessTreeMaker(testPath);

            // Event Classes
            //Dictionary<string, List<ProcMon>> ProcessDicts = Processor.LoadLists(testPath);
            //var Registry = ProcessDicts[EventClass.Registry];
            //var Network = ProcessDicts[EventClass.Network];
            //var FileSystem = ProcessDicts[EventClass.FileSystem];
            //var Process = ProcessDicts[EventClass.Process];
            //var AllEvents = ProcessDicts[EventClass.All];

            // Sorting
            //var procName = ProcMaps.OrderBy(x => x.Key.ProcessName).ToList();
            //var timeOfDay = AllEvents.OrderBy(x => x.TimeOfDay).ToList();
        }

    }

}
