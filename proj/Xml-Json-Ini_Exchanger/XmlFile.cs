using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ArmyAnt
{
    public class XmlFile : AConfigFile
    {
        public XmlFile(string filename = null) : base(filename)
        {
        }
        public XmlFile(AConfigFile value) : base(value)
        {
        }

        override public bool LoadString(string text)
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(text));
            var root = new List<ConfigElement>();
            while(xmlReader.Read())
            {
                root.Add(XmlToElement(xmlReader.Value));
            }
            if(root.Count > 0)
                this.root = root.ToArray();
            return this.root != null;
        }
        override public string ToString()
        {
            string ret = "";
            for(int i = 0; root != null && i < root.Length; i++)
            {
                ret += root[i].GetText(EConfigType.Xml) + "\n";
            }
            return ret;
        }
        public static ConfigElement XmlToElement(string text)
        {
            XmlTextReader reader = new XmlTextReader(new StringReader(text));
            ConfigElement ret = new ConfigElement("", null);


            return ret;
        }
    }
}