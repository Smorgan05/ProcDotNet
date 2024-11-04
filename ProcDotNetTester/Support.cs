using ProcDotNet.Classes;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Collections;


namespace ProcDotNetTester
{
    internal class Support
    {
        internal static string JSONConvEvents(Dictionary<string, List<ProcMon>> ProcessDicts)
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

        public class CustomJsonConverter<T> : System.Text.Json.Serialization.JsonConverter<T>
        {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                JsonNode node = JsonNode.Parse(ref reader);
                return (T)RecursiveDeserialize(node, typeToConvert);
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                JsonNode node = RecursiveSerialize(value); node.WriteTo(writer);
            }

            private JsonNode RecursiveSerialize(object value)
            {
                if (value == null)
                {
                    return null;
                }
                Type type = value.GetType();
                if (type.IsPrimitive || type == typeof(string))
                {
                    return JsonValue.Create(value);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    JsonArray array = new JsonArray();
                    foreach (var item in (IEnumerable)value)
                    {
                        array.Add(RecursiveSerialize(item));
                    }
                    return array;
                }
                else
                {
                    JsonObject obj = new JsonObject();
                    foreach (var property in type.GetProperties())
                    {
                        obj[property.Name] = RecursiveSerialize(property.GetValue(value));
                    }
                    return obj;
                }
            }
            private object RecursiveDeserialize(JsonNode node, Type type)
            {
                if (node == null)
                {
                    return null;
                }
                if (type.IsPrimitive || type == typeof(string))
                {
                    return Convert.ChangeType(node.GetValue<object>(), type);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    var listType = typeof(List<>).MakeGenericType(type.GetGenericArguments()[0]);
                    var list = (IList)Activator.CreateInstance(listType);
                    foreach (var childNode in node.AsArray())
                    {
                        list.Add(RecursiveDeserialize(childNode, type.GetGenericArguments()[0]));
                    }
                    return list;
                }
                else
                {
                    var obj = Activator.CreateInstance(type);
                    foreach (var property in type.GetProperties())
                    {
                        var childNode = node[property.Name];
                        var value = RecursiveDeserialize(childNode, property.PropertyType);
                        property.SetValue(obj, value);
                    }
                    return obj;
                }
            }
        }
    }
}
