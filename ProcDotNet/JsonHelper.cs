using ProcDotNet.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace ProcDotNet
{
    public class JsonHelper
    {
        public static string JSONConvEvents(Dictionary<string, List<ProcMon>> ProcessDicts)
        {

            // Creating an object to hold all data for serialization
            var jsonObject = new
            {
                Events = new
                {
                    Registry = ProcessDicts[EventClass.Registry],
                    Network = ProcessDicts[EventClass.Network],
                    FileSystem = ProcessDicts[EventClass.FileSystem],
                    Process = ProcessDicts[EventClass.Process],
                    Profiling = ProcessDicts[EventClass.Profiling]
                }
            };

            // Serializing the object to JSON
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            return JsonSerializer.Serialize(jsonObject, options);
        }

        public class CustomJsonConverter<T> : JsonConverter<T> where T : ProcMon, new()
        {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                JsonNode node = JsonNode.Parse(ref reader);
                return (T)RecursiveDeserialize(node);
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                JsonNode node = RecursiveSerialize(value);
                node.WriteTo(writer);
            }

            private JsonNode RecursiveSerialize(ProcMon prog)
            {
                if (prog == null)
                {
                    return null;
                }

                JsonObject obj = new JsonObject
                {
                    //Application Data
                    ["ProcessName"] = JsonValue.Create(prog.ProcessName),
                    ["ImagePath"] = JsonValue.Create(prog.ImagePath),
                    ["CommandLine"] = JsonValue.Create(prog.CommandLine),
                    ["Company"] = JsonValue.Create(prog.Company),
                    ["Description"] = JsonValue.Create(prog.Description),
                    ["Version"] = JsonValue.Create(prog.Version),
                    ["Architecture"] = JsonValue.Create(prog.Architecture),

                    //Event Details
                    ["SequenceNumber"] = JsonValue.Create(prog.SequenceNumber),
                    ["EventClass"] = JsonValue.Create(prog.EventClass),
                    ["Operation"] = JsonValue.Create(prog.Operation),
                    ["DateAndTime"] = JsonValue.Create(prog.DateAndTime),
                    ["TimeOfDay"] = JsonValue.Create(prog.TimeOfDay),
                    ["Category"] = JsonValue.Create(prog.Category),
                    ["Path"] = JsonValue.Create(prog.Path),
                    ["Detail"] = JsonValue.Create(prog.Detail),
                    ["Result"] = JsonValue.Create(prog.Result),
                    ["RelativeTime"] = JsonValue.Create(prog.RelativeTime),
                    ["Duration"] = JsonValue.Create(prog.Duration),
                    ["CompletionTime"] = JsonValue.Create(prog.CompletionTime),

                    //Process Mgmt
                    ["UserName"] = JsonValue.Create(prog.UserName),
                    ["Session"] = JsonValue.Create(prog.Session),
                    ["AuthenticationID"] = JsonValue.Create(prog.AuthenticationID),
                    ["Integrity"] = JsonValue.Create(prog.Integrity),
                    ["ProcessID"] = JsonValue.Create(prog.ProcessID),
                    ["ThreadID"] = JsonValue.Create(prog.ThreadID),
                    ["ParentPID"] = JsonValue.Create(prog.ParentPID),
                    ["Virtualized"] = JsonValue.Create(prog.Virtualized)

                };

                JsonArray ChildrenArray = new JsonArray();
                foreach (var friend in prog.Children)
                {
                    ChildrenArray.Add(RecursiveSerialize(friend));
                }
                obj["Children"] = ChildrenArray;

                return obj;
            }

            private ProcMon RecursiveDeserialize(JsonNode node)
            {
                if (node == null)
                {
                    return null;
                }

                var prog = new ProcMon
                {
                    // Children
                    Children = node["Children"].AsArray().Select(childNode => RecursiveDeserialize(childNode)).ToList(),

                    //Application Data
                    ProcessName = node["ProcessName"].GetValue<string>(),
                    ImagePath = node["ImagePath"].GetValue<string>(),
                    CommandLine = node["CommandLine"].GetValue<string>(),
                    Company = node["Company"].GetValue<string>(),
                    Description = node["Description"].GetValue<string>(),
                    Version = node["Version"].GetValue<string>(),
                    Architecture = node["Architecture"].GetValue<string>(),

                    // Event Details
                    SequenceNumber = node["SequenceNumber"].GetValue<string>(),
                    EventClass = node["EventClass"].GetValue<string>(),
                    Operation = node["Operation"].GetValue<string>(),
                    DateAndTime = node["DateAndTime"].GetValue<DateTime>(),
                    TimeOfDay = node["TimeOfDay"].GetValue<string>(),
                    Category = node["Category"].GetValue<string>(),
                    Path = node["Path"].GetValue<string>(),
                    Detail = node["Detail"].GetValue<string>(),
                    Result = node["Result"].GetValue<string>(),
                    RelativeTime = node["RelativeTime"].GetValue<DateTime>(),
                    Duration = node["Duration"].GetValue<string>(),
                    CompletionTime = node["CompletionTime"].GetValue<DateTime>(),

                    //Process Mgmt
                    UserName = node["UserName"].GetValue<string>(),
                    Session = node["Session"].GetValue<int>(),
                    AuthenticationID = node["AuthenticationID"].GetValue<string>(),
                    Integrity = node["Integrity"].GetValue<string>(),
                    ProcessID = node["ProcessID"].GetValue<int>(),
                    ThreadID = node["ThreadID"].GetValue<int>(),
                    ParentPID = node["ParentPID"].GetValue<int>()

                };

                return prog;

                throw new NotImplementedException();
            }
        }

        public static string JSONConvProcTree(List<ProcMon> ProcTree)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                Converters = { new CustomJsonConverter<ProcMon>() },
                WriteIndented = true
            };

            return JsonSerializer.Serialize(ProcTree, options);
        }

        
    }
}
