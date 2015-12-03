using System.Collections.Generic;
using System.Linq;

namespace ArmyAnt
{
    public class IniFile : AConfigFile
    {
        public IniFile(string filename = null) : base(filename)
        {
        }
        override public bool LoadString(string text)
        {
            var codes = ConfigAttribute.CutToLines(text);
            var ret = new List<ConfigElement>();
            if(codes.Length < 2)
                return false;
            for(int i = 0; i < codes.Length; i++)
            {
                if(codes[i][0] == ';')
                    continue;
                else if(codes[i][0] == '[')
                {
                    ret.Insert(ret.Count, new ConfigElement(codes[i].Remove(codes[i].Length - 1).Remove(0, 1), this));
                }
                else if(ret.Count>0)
                {
                    ret.Last().AddAttribute(new ConfigAttribute(codes[i].Remove(codes[i].IndexOf('=')), codes[i].Remove(0, codes[i].IndexOf('='))));
                }
            }
            root = ret.ToArray();
            return true;
        }
        override public string ToString()
        {
            string ret = "";
            for(int i = 0; root != null && i < root.Length; i++)
            {
                ret += '[' + root[i]["name"].value + "]\n";
                for(int j = 0; j < root[i].Attributes.Length; j++)
                {
                    if(root[i].Attributes[j].key != "name")
                    {
                        ret += root[i].Attributes[j].key + '=' + root[i].Attributes[j].value + '\n';
                    }
                }
            }
            return ret;
        }
    }
}