using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Xml_Json_Ini_Exchanger
{
    public enum EConfigType
    {
        Null,
        Ini,
        Xml,
        Json,
        Sql,
    }
    public class ConfigFile
    {
        ConfigFile()
        {
        }
        bool LoadFile(string filename,EConfigType type)
        {
        }
        bool SaveFile(string filename, EConfigType type)
        {
        }
        bool Load(XmlDocument xml)
        {
        }
        bool Load(DataSet data)
        {
        }
        bool Load()
        string Text
        {
            get; set;
        }
    }
}
