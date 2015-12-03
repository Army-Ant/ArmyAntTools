using System.IO;
using Newtonsoft.Json;

namespace ArmyAnt
{
    public class JsonFile : AConfigFile
    {
        public JsonFile(string filename = null) : base(filename)
        {
        }
        override public bool LoadString(string text)
        {
            root = new ConfigElement[1];
            root[0] = JsonToElement(text);
            return root != null;
        }
        override public string ToString()
        {
            var str = new StringWriter();
            var jsonWriter = new JsonTextWriter(str);

            jsonWriter.WriteStartArray();
            for(int i = 0; root != null && i < root.Length; i++)
            {
                jsonWriter.WriteValue(root[i].GetText(EConfigType.Json));
            }
            jsonWriter.WriteEndArray();
            return str.GetStringBuilder().ToString();
        }
        internal enum RootType : byte
        {
            Unsolved,
            Object,
            Array,
            Constructor,
            Value
        }

        public static ConfigElement JsonToElement(string text)
        {
            var jsonReader = new JsonTextReader(new StringReader(text));
            ConfigElement single = new ConfigElement("", null);
            string end = jsonReader.ReadAsString();
            RootType innerType = RootType.Unsolved;
            string innerValue = null;
            string innerPropName = null;
            string baseValue = null;
            RootType type = RootType.Unsolved;
            while(jsonReader.Read())
            {
                switch(jsonReader.TokenType)
                {
                    case JsonToken.Comment:
                        continue;
                    case JsonToken.StartArray:
                        if(type == RootType.Unsolved)
                            type = RootType.Array;
                        else
                        {
                            innerType = RootType.Array;
                            innerValue = "";
                        }
                        break;
                    case JsonToken.StartConstructor:
                        if(type == RootType.Unsolved)
                            type = RootType.Constructor;
                        else
                        {
                            innerType = RootType.Constructor;
                            innerValue = "";
                        }
                        break;
                    case JsonToken.StartObject:
                        if(type == RootType.Unsolved)
                            type = RootType.Object;
                        else
                        {
                            innerType = RootType.Object;
                            innerValue = "";
                        }
                        break;
                    case JsonToken.EndArray:
                        if(innerType != RootType.Array)
                        {
                            if(type != RootType.Array || jsonReader.Read())
                                return null;
                        }
                        else
                        {
                            if(innerPropName == null && innerType == RootType.Array)
                            {
                                single.AddChild(JsonToElement(innerValue));
                            }
                            else if(innerPropName != null && innerType == RootType.Object)
                            {
                                single.AddAttribute(innerPropName, innerValue);
                                innerPropName = null;
                            }
                            else
                                return null;
                        }
                        innerValue = null;
                        break;
                    case JsonToken.EndConstructor:
                        if(innerType != RootType.Constructor)
                        {
                            if(type != RootType.Constructor || jsonReader.Read())
                                return null;
                        }
                        else
                        {
                            return null;
                        }
                        innerValue = null;
                        break;
                    case JsonToken.EndObject:
                        if(innerType != RootType.Object)
                        {
                            if(type != RootType.Object || jsonReader.Read())
                                return null;
                        }
                        else
                        {
                            if(innerPropName == null && innerType == RootType.Array)
                            {
                                single.AddChild(JsonToElement(innerValue));
                            }
                            else if(innerPropName != null && innerType == RootType.Object)
                            {
                                single.AddAttribute(innerPropName, innerValue);
                                innerPropName = null;
                            }
                            else
                                return null;
                        }
                        innerValue = null;
                        break;
                    case JsonToken.Null:
                    case JsonToken.Undefined:
                        if(innerPropName == null && innerType == RootType.Array)
                        {
                            single.AddChild((ConfigElement)null);
                        }
                        else if(innerPropName != null && innerType == RootType.Object)
                        {
                            single.AddAttribute(innerPropName, null);
                            innerPropName = null;
                        }
                        else if(innerPropName != null && innerType == RootType.Value)
                            baseValue = "";
                        else
                            return null;
                        break;
                    case JsonToken.PropertyName:
                        if(type == RootType.Unsolved)
                        {
                            type = RootType.Value;
                            baseValue = jsonReader.Value.ToString();
                            single = new ConfigElement(null, null);
                        }
                        else if(innerType == RootType.Value)
                            return null;
                        else if(innerPropName != null)
                            return null;
                        else
                        {
                            innerPropName = jsonReader.Value.ToString();
                        }
                        break;
                    default:
                        if(type == RootType.Unsolved)
                        {
                            type = RootType.Value;
                            baseValue = jsonReader.Value.ToString();
                        }
                        else if(type == RootType.Value)
                        {
                            if(single == null)
                                return null;
                            single.AddAttribute(baseValue, jsonReader.Value.ToString());
                            if(jsonReader.Read())
                                return null;
                        }
                        else
                        {
                            innerValue += jsonReader.Value.ToString();
                        }
                        break;
                }
            }
            jsonReader.Close();
            if(type == RootType.Unsolved)
                return null;
            else
                return single;
        }
    }
}