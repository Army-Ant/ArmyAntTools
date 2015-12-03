using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace ArmyAnt
{
    public interface IConfigFile
    {
        bool LoadFile(string filename);
        bool LoadString(string text);
        string ToString();
        bool ToFile(string filename);
        string Text
        {
            get; set;
        }
        ConfigElement[] Root
        {
            get;
        }
        ConfigElement this[int index]
        {
            get;
        }
        int this[ConfigElement field]
        {
            get;
        }
    }

    public abstract class AConfigFile : IConfigFile
    {
        public AConfigFile(string filename = null)
        {
            if(filename != null)
                LoadFile(filename);
        }
        public static AConfigFile CreateConfig(string filename, EConfigType type)
        {
            switch(type)
            {
                case EConfigType.Ini:
                    return new IniFile(filename);
                case EConfigType.Json:
                    return new JsonFile(filename);
                case EConfigType.Xml:
                    return new XmlFile(filename);
                case EConfigType.Sql:
                    return null;
                default:
                    throw new Exception("Error type of config file format");
            }
        }
        public bool LoadFile(string filename)
        {
            var f = File.OpenText(filename);
            var data = f.ReadToEnd();
            f.Close();
            return LoadString(data);
        }
        public abstract bool LoadString(string text);
        override public abstract string ToString();
        public bool ToFile(string filename)
        {
            try
            {

                var f = File.OpenWrite(filename);
                var str = Encoding.UTF8.GetBytes(ToString());
                f.Write(str, 0, str.Length);
                f.Close();
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }
        public string Text
        {
            get
            {
                return ToString();
            }
            set
            {
                LoadString(value);
            }
        }
        public ConfigElement[] Root
        {
            get
            {
                return root;
            }
            private set
            {
                root = value;
            }
        }
        public ConfigElement this[int index]
        {
            get
            {
                if(root == null || root.Length <= index)
                    return null;
                return root[index];
            }
            private set
            {
                if(root != null && root.Length > index)
                {
                    root[index] = value;
                }
            }
        }
        public int this[ConfigElement field]
        {
            get
            {
                for(int i = 0; root != null && root.Length > i; i++)
                {
                    if(root[i] == field)
                        return i;
                }
                return -1;
            }
        }
        protected internal ConfigElement[] root = null;
    }

    public class ConfigElement
    {
        public ConfigElement(string name, IConfigFile parent)
        {
            this.parent = parent;
            attributes.Add(new ConfigAttribute("name", name));
        }
        public ConfigElement(string text, EConfigType type)
        {
            switch(type)
            {
                case EConfigType.Ini:
                    {
                        var code = ConfigAttribute.CutToLines(text);
                        bool isnamed = false;
                        for(int i = 0; i < code.Length; i++)
                        {
                            if(code[i][0] == '[' && !isnamed)
                            {
                                attributes.Add(new ConfigAttribute("name", code[i].Remove(code[i].Length - 1).Remove(0, 1)));
                            }
                            else if(code[i][0] != '[' && isnamed)
                            {
                                attributes.Add(new ConfigAttribute(code[i].Remove(code[i].IndexOf('=')), code[i].Remove(0, code[i].IndexOf('='))));
                            }
                            else if(code[i][0] == '[' && isnamed)
                            {
                                throw new Exception("Too more titles in one param of Config Element text");
                            }
                        }
                    }
                    break;
                case EConfigType.Json:
                    {
                        var ret = JsonFile.JsonToElement(text);
                        if(ret == null)
                            throw new Exception("Error format in this json file");
                        children = ret.children;
                        attributes = ret.attributes;
                    }
                    break;
                case EConfigType.Xml:
                    break;
                case EConfigType.Sql:
                    break;
                default:
                    throw new Exception("Error type of config file format");
            }
        }
        public bool AddChild(ConfigElement child)
        {
            children.Add(child);
            return true;
        }
        public bool AddChild(ConfigElement[] child)
        {
            for(int i = 0; child != null && i < child.Length; i++)
                children.Add(child[i]);
            return true;
        }
        public bool AddAttribute(ConfigAttribute attr)
        {
            attributes.Add(attr);
            return true;
        }
        public bool AddAttribute(string key,string value)
        {
            attributes.Add(new ConfigAttribute(key, value));
            return true;
        }
        public ConfigAttribute[] Attributes
        {
            get
            {
                return attributes.ToArray();
            }
            internal set
            {
                attributes = new List<ConfigAttribute>(value);
            }
        }
        public ConfigElement[] Children
        {
            get
            {
                return children.ToArray();
            }
            internal set
            {
                children = new List<ConfigElement>(children);
            }
        }
        public ConfigElement this[int index]
        {
            get
            {
                return children[index];
            }
        }
        public ConfigAttribute this[string attrName]
        {
            get
            {
                for(var i = 0; i < attributes.Count; i++)
                {
                    if(attributes[i].key == attrName)
                        return attributes[i];
                }
                return null;
            }
        }
        public string GetText(EConfigType type)
        {
            string ret = "";
            switch(type)
            {
                case EConfigType.Ini:
                    if(children.Count > 0)
                        throw new Exception("Cannot save the config to ini file type when there is inner elements in root element");
                    ret += '[' + this["name"].value + ']';
                    for(int i = 0; i < attributes.Count; i++)
                    {
                        if(attributes[i].key != "name")
                        {
                            ret += '\n' + attributes[i].key + '=' + attributes[i].value;
                        }
                    }
                    break;
                case EConfigType.Json:
                    if(attributes.Count > 0)
                    {
                        ret += '{';
                        for(int i = 0; i < attributes.Count; i++)
                            ret += "\"" + attributes[i].key + "\":" + (attributes[i].value == null ? "null" : ("\"" + attributes[i].value + "\"")) + ";";
                        ret = ret.Remove(ret.Length - 1);
                    }
                    else if(children.Count > 0)
                    {
                        ret += '[';
                    }
                    if(children.Count > 0)
                    {
                        for(int i = 0; i < children.Count; i++)
                        {
                            if(attributes.Count > 0)
                                ret += "\"" + i + "\":" + (children[i] == null ? "null" : ("\"" + children[i].GetText(EConfigType.Json) + "\"")) + ";";
                            else
                                ret += (children[i] == null ? "null" : ("\"" + children[i].GetText(EConfigType.Json) + "\"")) + ",";
                        }
                        ret = ret.Remove(ret.Length - 1);
                    }
                    if(attributes.Count > 0)
                    {
                        ret += "}";
                    }
                    else if(children.Count > 0)
                    {
                        ret += "]";
                    }
                    else
                        ret = "null";
                    break;
                case EConfigType.Xml:
                    break;
                case EConfigType.Sql:
                    break;
            }
            return ret;
        }
        private IConfigFile parent = null;
        private List<ConfigElement> children = new List<ConfigElement>();
        private List<ConfigAttribute> attributes = new List<ConfigAttribute>();
    }
    public class ConfigAttribute
    {
        public ConfigAttribute(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        public string key = null;
        public string value = null;
        public ConfigElement ToElement(EConfigType type)
        {
            if(value == null || type == EConfigType.Null)
                return null;
            return new ConfigElement(value, type);
        }

        public static string[] CutToLines(string src)
        {
            var ret = new List<string>();
            ret.Insert(0,"");
            for(int i = 0, j = 0; i < src.Length; i++)
            {
                if(src[i] != '\n')
                    ret[j] += src[i];
                else if(ret[j] == "" && src[i] == ' ')
                    continue;
                else
                {
                    j++;
                    ret.Insert(j, "");
                }

            }
            return ret.ToArray();
        }
    }
}
