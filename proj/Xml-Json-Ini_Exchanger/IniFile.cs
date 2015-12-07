using System.Collections.Generic;
using System.Linq;

namespace ArmyAnt
{
	/// <summary>
	/// 处理INI格式的配置文本类
	/// </summary>
    public class IniFile : AConfigFile
    {
		/// <summary>
		/// 载入INI配置文件
		/// </summary>
		/// <param name="filename">要载入的INI配置文件</param>
		public IniFile(string filename = null) : base(filename)
        {
        }
		/// <summary>
		/// 从另一个配置数据引用创建INI格式配置数据对象
		/// </summary>
		/// <param name="value">要共享数据引用的另一个配置数据对象</param>
        public IniFile(AConfigFile value) : base(value)
        {
        }
		/// <summary>
		/// 从INI格式文本载入INI配置
		/// <para><see cref="AConfigFile.LoadString(string)"/></para>
		/// <para><see cref="IConfigFile.LoadString(string)"/></para>
		/// </summary>
		/// <param name="text">要载入的INI格式文本</param>
		/// <returns>成功返回<c>true</c></returns>
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
		/// 将配置数据转化为INI格式文本
		/// <para><see cref="AConfigFile.ToString()"/></para>
		/// <para><see cref="IConfigFile.ToString()"/></para>
		/// </summary>
		/// <returns>转化后的INI格式配置文本</returns>
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