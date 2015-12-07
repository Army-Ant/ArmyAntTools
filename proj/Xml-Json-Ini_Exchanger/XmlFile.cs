using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ArmyAnt
{
    public class XmlFile : AConfigFile
    {
		/// <summary>
		/// ����XML�����ļ�
		/// </summary>
		/// <param name="filename">Ҫ�����XML�����ļ�</param>
		public XmlFile(string filename = null) : base(filename)
        {
        }
		/// <summary>
		/// ����һ�������������ô���XML��ʽ�������ݶ���
		/// </summary>
		/// <param name="value">Ҫ�����������õ���һ���������ݶ���</param>
		public XmlFile(AConfigFile value) : base(value)
        {
        }
		/// <summary>
		/// ��XML��ʽ�ı�����XML����
		/// <para><see cref="AConfigFile.LoadString(string)"/></para>
		/// <para><see cref="IConfigFile.LoadString(string)"/></para>
		/// </summary>
		/// <param name="text">Ҫ�����XML��ʽ�ı�</param>
		/// <returns>�ɹ�����<c>true</c></returns>
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
		/// ����������ת��ΪXML��ʽ�ı�
		/// <para><see cref="AConfigFile.ToString()"/></para>
		/// <para><see cref="IConfigFile.ToString()"/></para>
		/// </summary>
		/// <returns>ת�����XML��ʽ�����ı�</returns>
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