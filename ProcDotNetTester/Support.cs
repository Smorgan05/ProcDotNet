using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcDotNet.Classes;
using ProcDotNet.Tree;
using System.Text.Json.Serialization;
using System.Text.Json;


namespace ProcDotNetTester
{
    internal class Support
    {
        internal static string JSONConv(List<TreeNode<ProcMon>> procTree, Dictionary<string, List<ProcMon>> ProcessDicts)
        {

            // Creating an object to hold all data for serialization
            var jsonObject = new
            {
                ProcessTree = procTree,  // Ensure your TreeNode class is serializable
                Events = new
                {
                    Registry = ProcessDicts[EventClass.Registry],
                    Network = ProcessDicts[EventClass.Network],
                    FileSystem = ProcessDicts[EventClass.FileSystem],
                    Process = ProcessDicts[EventClass.Process],
                    All = ProcessDicts[EventClass.All]
                }
            };

            // Serializing the object to JSON
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            return System.Text.Json.JsonSerializer.Serialize(jsonObject, options);
        }
    }
}
