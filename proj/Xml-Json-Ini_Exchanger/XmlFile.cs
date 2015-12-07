using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ArmyAnt
{
    public class XmlFile : AConfigFile
    {
		/// <summary>
		/// 载入XML配置文件
		/// </summary>
		/// <param name="filename">要载入的XML配置文件</param>
		public XmlFile(string filename = null) : base(filename)
        {
        }
		/// <summary>
		/// 从另一个配置数据引用创建XML格式配置数据对象
		/// </summary>
		/// <param name="value">要共享数据引用的另一个配置数据对象</param>
		public XmlFile(AConfigFile value) : base(value)
        {
        }
		/// <summary>
		/// 从XML格式文本载入XML配置
		/// <para><see cref="AConfigFile.LoadString(string)"/></para>
		/// <para><see cref="IConfigFile.LoadString(string)"/></para>
		/// </summary>
		/// <param name="text">要载入的XML格式文本</param>
		/// <returns>成功返回<c>true</c></returns>
		override public bool LoadString(string text)
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(text));
            var root = new List<ConfigElement>();
            while(xmlReader.Read())
            {
                root.Add(XmlToElement(xmlReader.Value));
            }
            if(root.Count > 0)
                this.root = new ConfigElementCollection(root.ToArray());
            return this.root != null;
        }
		/// <summary>
		/// 将配置数据转化为XML格式文本
		/// <para><see cref="AConfigFile.ToString()"/></para>
		/// <para><see cref="IConfigFile.ToString()"/></para>
		/// </summary>
		/// <returns>转化后的XML格式配置文本</returns>
		override public string ToString()
        {
            string ret = "";
            for(int i = 0; root != null && i < root.Count; i++)
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