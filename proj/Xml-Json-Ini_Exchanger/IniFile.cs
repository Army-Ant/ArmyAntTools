using System.Collections.Generic;
using System.Linq;

namespace ArmyAnt
{
	/// <summary>
	/// ����INI��ʽ�������ı���
	/// </summary>
    public class IniFile : AConfigFile
    {
		/// <summary>
		/// ����INI�����ļ�
		/// </summary>
		/// <param name="filename">Ҫ�����INI�����ļ�</param>
		public IniFile(string filename = null) : base(filename)
        {
        }
		/// <summary>
		/// ����һ�������������ô���INI��ʽ�������ݶ���
		/// </summary>
		/// <param name="value">Ҫ�����������õ���һ���������ݶ���</param>
        public IniFile(AConfigFile value) : base(value)
        {
        }
		/// <summary>
		/// ��INI��ʽ�ı�����INI����
		/// <para><see cref="AConfigFile.LoadString(string)"/></para>
		/// <para><see cref="IConfigFile.LoadString(string)"/></para>
		/// </summary>
		/// <param name="text">Ҫ�����INI��ʽ�ı�</param>
		/// <returns>�ɹ�����<c>true</c></returns>
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
                    ret.Insert(ret.Count, new ConfigElement(codes[i].Remove(codes[i].Length - 1).Remove(0, 1), null));
                }
                else if(ret.Count>0)
                {
                    ret.Last().AddAttribute(new ConfigAttribute(codes[i].Remove(codes[i].IndexOf('=')), codes[i].Remove(0, codes[i].IndexOf('=') + 1)));
                }
            }
			if(root == null)
				root = new ConfigElementCollection(ret.ToArray());
            return true;
        }
		/// <summary>
		/// ����������ת��ΪINI��ʽ�ı�
		/// <para><see cref="AConfigFile.ToString()"/></para>
		/// <para><see cref="IConfigFile.ToString()"/></para>
		/// </summary>
		/// <returns>ת�����INI��ʽ�����ı�</returns>
		override public string ToString()
        {
            string ret = "";
            for(int i = 0; root != null && i < root.Count; i++)
            {
                var name = root[i].name;
                ret += '[' + (name == "" ? i.ToString() : name) + "]\n";
                for(int j = 0; j < root[i].Attributes.Length; j++)
                {
                        ret += root[i].Attributes[j].key + '=' + root[i].Attributes[j].value + '\n';
                }
            }
            return ret;
        }
    }
}