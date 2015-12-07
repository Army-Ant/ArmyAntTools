using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace ArmyAnt
{
	/// <summary>
	/// 定义所有类型配置文件读取类的接口
	/// </summary>
	public interface IConfigFile
	{
		/// <summary>
		/// 从文件载入配置
		/// </summary>
		/// <param name="filename">要载入的配置文件名</param>
		/// <returns>成功返回<c>true</c></returns>
		bool LoadFile(string filename);
		/// <summary>
		/// 从文本字符串载入配置
		/// </summary>
		/// <param name="text">要载入的配置字符串</param>
		/// <returns>成功返回<c>true</c></returns>
		bool LoadString(string text);
		/// <summary>
		/// 将配置信息转化为字符串
		/// </summary>
		/// <returns>按照本类型转化后的对应格式字符串配置信息</returns>
		string ToString();
		/// <summary>
		/// 转化为本格式的配置文件并保存
		/// </summary>
		/// <param name="filename">要保存的配置文件</param>
		/// <returns>成功返回<c>true</c></returns>
		bool ToFile(string filename);
		/// <summary>
		/// 保存为字符串或者读取字符串
		/// </summary>
		string Text
		{
			get; set;
		}
		/// <summary>
		/// 获取ConfigElement类型的配置内容的根森林
		/// </summary>
		ConfigElement[] Root
		{
			get;
		}
		/// <summary>
		/// 获取对应索引处的配置树根节点
		/// </summary>
		/// <param name="index">从0开始的索引，或者从-1开始的倒数索引</param>
		/// <returns>返回要检索的配置树根节点，如果索引超出范围，返回null</returns>
		ConfigElement this[int index]
		{
			get;
		}
		/// <summary>
		/// 获取该根节点在本配置森林中的索引位置
		/// </summary>
		/// <param name="field">要检索的根节点</param>
		/// <returns>返回该根节点所在的顺位索引(从0开始)，如果不存在，返回-1</returns>
		int this[ConfigElement field]
		{
			get;
		}
	}

	/// <summary>
	/// 继承<c>IConfigFile</c>接口的抽象类，定义配置文件读取的公共函数
	/// </summary>
	public abstract class AConfigFile : IConfigFile
	{
		/// <summary>
		/// 创建配置信息对象
		/// </summary>
		/// <param name="filename">要获取的配置文件</param>
		public AConfigFile(string filename = null)
		{
			if(filename != null)
				LoadFile(filename);
		}
		/// <summary>
		/// 使用另一个配置信息对象的内容引用来创建一个新的配置信息对象
		/// </summary>
		/// <param name="value">要共用信息的原配置信息对象</param>
		public AConfigFile(AConfigFile value)
		{
			root = value.root;
		}
		/// <summary>
		/// 按照对应类型创建一个配置信息对象
		/// </summary>
		/// <param name="filename">要读取信息的文件</param>
		/// <param name="type">目标配置文件的格式</param>
		/// <returns>返回配置信息对象</returns>
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
		/// <summary>
		/// 从文件载入配置
		/// <para><see cref="IConfigFile.LoadFile(string)"/></para>
		/// </summary>
		/// <param name="filename">要载入的配置文件名</param>
		/// <returns>成功返回<c>true</c></returns>
		virtual public bool LoadFile(string filename)
		{
			var f = File.OpenText(filename);
			var data = f.ReadToEnd();
			f.Close();
			return LoadString(data.Replace("\r", ""));
		}
		/// <summary>
		/// 从文本字符串载入配置
		/// <para><see cref="IConfigFile.LoadString(string)"/></para>
		/// </summary>
		/// <param name="text">要载入的配置字符串</param>
		/// <returns>成功返回<c>true</c></returns>
		public abstract bool LoadString(string text);
		/// <summary>
		/// 将配置信息转化为字符串
		/// <para><see cref="IConfigFile.ToString()"/></para>
		/// <para><see cref="Object.ToString()"/></para>
		/// </summary>
		/// <returns>按照本类型转化后的对应格式字符串配置信息</returns>
		override public abstract string ToString();
		/// <summary>
		/// 转化为本格式的配置文件并保存
		/// <para><see cref="IConfigFile.ToFile(string)"/></para>
		/// </summary>
		/// <param name="filename">要保存的配置文件</param>
		/// <returns>成功返回<c>true</c></returns>
		virtual public bool ToFile(string filename)
		{
			try
			{

				var f = File.OpenWrite(filename);
				var str = Encoding.UTF8.GetBytes(ToString());
				f.SetLength(0);
				f.Write(str, 0, str.Length);
				f.Close();
			}
			catch(Exception)
			{
				return false;
			}
			return true;
		}
		/// <summary>
		/// 保存为字符串或者读取字符串
		/// <para><see cref="IConfigFile.Text"/></para>
		/// </summary>
		virtual public string Text
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
		/// <summary>
		/// 获取ConfigElement类型的配置内容的根森林
		/// <para><see cref="IConfigFile.Root"/></para>
		/// </summary>
		virtual public ConfigElement[] Root
		{
			get
			{
				return root.ToArray();
			}
			private set
			{
				root = new ConfigElementCollection(value);
			}
		}
		/// <summary>
		/// 获取对应索引处的配置树根节点
		/// <para><see cref="IConfigFile.this[int]"/></para>
		/// </summary>
		/// <param name="index">从0开始的索引，或者从-1开始的倒数索引</param>
		/// <returns>返回要检索的配置树根节点，如果索引超出范围，返回null</returns>
		virtual public ConfigElement this[int index]
		{
			get
			{
				if(root == null || root.Count <= index)
					return null;
				return root[index];
			}
			private set
			{
				if(root != null && root.Count > index)
				{
					root[index] = value;
				}
			}
		}
		/// <summary>
		/// 获取该根节点在本配置森林中的索引位置
		/// <para><see cref="IConfigFile.this[ConfigElement]"/></para>
		/// </summary>
		/// <param name="field">要检索的根节点</param>
		/// <returns>返回该根节点所在的顺位索引(从0开始)，如果不存在，返回-1</returns>
		virtual public int this[ConfigElement field]
		{
			get
			{
				for(int i = 0; root != null && root.Count > i; i++)
				{
					if(root[i] == field)
						return i;
				}
				return -1;
			}
		}
		/// <summary>
		/// 配置信息
		/// </summary>
		protected internal ConfigElementCollection root = null;
	}
	/// <summary>
	/// 定义配置数据的单个节点元素的类
	/// </summary>
	public class ConfigElement
	{
		/// <summary>
		/// 根据配置元素节点的名称和其父节点，创建该元素。
		/// </summary>
		/// <param name="name">节点名字，可为null</param>
		/// <param name="parent">父节点，若本节点为根节点，则父节点为null</param>
		public ConfigElement(string name, ConfigElement parent)
		{
			this.parent = parent;
			children = new ConfigElementCollection(this);
			this.name = name;
		}
		/// <summary>
		/// 根据配置内容文本和格式类型，创建一个匿名根节点
		/// </summary>
		/// <param name="text">配置文本内容</param>
		/// <param name="type">文本内容的格式</param>
		public ConfigElement(string text, EConfigType type)
		{
			children = new ConfigElementCollection(this);
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
								name = code[i].Remove(code[i].Length - 1).Remove(0, 1);
							}
							else if(code[i][0] != '[' && isnamed)
							{
								attributes.Add(new ConfigAttribute(code[i].Remove(code[i].IndexOf('=')), code[i].Remove(0, code[i].IndexOf('=') + 1)));
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
						children.Clear();
						AddChild(JsonFile.JsonToElement(text));
						if(children.Count <= 0)
							throw new Exception("Error format in this json file");
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
		/// <summary>
		/// 添加子节点
		/// </summary>
		/// <param name="child">要添加的子节点</param>
		/// <returns>添加成功返回<c>true</c></returns>
		public bool AddChild(ConfigElement child)
		{
			return children.InsertElement(child);
		}
		/// <summary>
		/// 添加一组子节点
		/// </summary>
		/// <param name="child">要添加的子节点数组</param>
		/// <returns>添加成功返回<c>true</c></returns>
		public bool AddChild(ConfigElement[] child)
		{
			bool ret = true;
			for(int i = 0; child != null && i < child.Length && (ret = ret && children.InsertElement(child[i])); i++)
				;
			return ret;
		}
		/// <summary>
		/// 添加属性
		/// </summary>
		/// <param name="attr">要添加的属性</param>
		/// <returns>成功返回<c>true</c></returns>
		public bool AddAttribute(ConfigAttribute attr)
		{
			attributes.Add(attr);
			return true;
		}
		/// <summary>
		/// 添加属性字段
		/// </summary>
		/// <param name="key">要添加的属性名（键）</param>
		/// <param name="value">要添加的属性值</param>
		/// <returns>成功返回<c>true</c></returns>
		public bool AddAttribute(string key, string value)
		{
			if(key[0] == '\'' && key[key.Length - 1] == '\'')
				key = key.Remove(key.Length - 1).Remove(0, 1);
			if(key[0] == '"' && key[key.Length - 1] == '"')
				key = key.Remove(key.Length - 1).Remove(0, 1);
			if(value[0] == '\'' && value[value.Length - 1] == '\'')
				value = value.Remove(value.Length - 1).Remove(0, 1);
			if(value[0] == '"' && value[value.Length - 1] == '"')
				value = value.Remove(value.Length - 1).Remove(0, 1);
			attributes.Add(new ConfigAttribute(key, value));
			return true;
		}
		/// <summary>
		/// 获取本节点的所有属性
		/// </summary>
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
		/// <summary>
		/// 获取本节点的所有子节点
		/// </summary>
		public ConfigElement[] Children
		{
			get
			{
				return children.ToArray();
			}
			internal set
			{
				children = new ConfigElementCollection();
				children.AddElements(value);
			}
		}
		/// <summary>
		/// 获取对应索引处的子节点
		/// </summary>
		/// <param name="index">要获取的索引值</param>
		/// <returns>返回要检索的子节点，若索引超出范围，返回null</returns>
		public ConfigElement this[int index]
		{
			get
			{
				return children[index];
			}
		}
		/// <summary>
		/// 获取指定子节点在子节点群中的索引
		/// </summary>
		/// <param name="attrName">要检索的子节点</param>
		/// <returns>返回该子节点所在的索引顺位（从0开始），若无此子节点，返回-1</returns>
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
		/// <summary>
		/// 将此结点及其所有信息转化为指定格式的配置文本
		/// </summary>
		/// <param name="type">要转化成的配置格式</param>
		/// <returns>返回转化结果</returns>
		public string GetText(EConfigType type)
		{
			string ret = "";
			switch(type)
			{
				case EConfigType.Ini:
					if(children.Count > 0)
						throw new Exception("Cannot save the config to ini file type when there is inner elements in root element");
					ret += '[' + name + ']';
					for(int i = 0; i < attributes.Count; i++)
					{
						if(attributes[i].key != "name")
						{
							ret += '\n' + attributes[i].key + '=' + attributes[i].value;
						}
					}
					break;
				case EConfigType.Json:
					ret += "\t";
					if(attributes.Count > 0)
					{
						ret += '{';
						for(int i = 0; i < attributes.Count; i++)
						{
							ret += '"';
							ret += attributes[i].key + '"' + ":" + (attributes[i].value == null ? "null" : ('"' + attributes[i].value + '"')) + ",";
						}
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
								ret += '"' + i + '"' + ":" + (children[i] == null ? "null" : ('"' + children[i].GetText(EConfigType.Json) + '"')) + ",";
							else
								ret += (children[i] == null ? "null" : ('"' + children[i].GetText(EConfigType.Json) + '"')) + ",";
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
						ret += "null";
					break;
				case EConfigType.Xml:
					break;
				case EConfigType.Sql:
					break;
			}
			return ret;
		}
		/// <summary>
		/// 节点的名称
		/// </summary>
		public string name = "";
		/// <summary>
		/// 节点的内容文本(值)
		/// </summary>
		public string innerStr = "";
		/// <summary>
		/// 父节点引用
		/// </summary>
		private ConfigElement parent = null;
		/// <summary>
		/// 子节点集合
		/// </summary>
		private ConfigElementCollection children;
		/// <summary>
		/// 属性列表
		/// </summary>
		private List<ConfigAttribute> attributes = new List<ConfigAttribute>();
	}
	/// <summary>
	/// 定义一系列ConfigElement对象的集合
	/// </summary>
	public class ConfigElementCollection
	{
		/// <summary>
		/// 创建一个节点集合
		/// </summary>
		/// <param name="parent">父节点引用</param>
		public ConfigElementCollection(ConfigElement parent = null)
		{
			this.parent = parent;
		}
		/// <summary>
		/// 从已存在的节点数组创建一个节点集合
		/// </summary>
		/// <param name="children">已存在的节点集合</param>
		/// <param name="parent">父节点引用</param>
		public ConfigElementCollection(ConfigElement[] children, ConfigElement parent = null)
		{
			this.parent = parent;
			AddElements(children);
		}
		/// <summary>
		/// 在指定位置处插入一个节点元素，若索引超出范围则插入在最后（倒数索引则是最前）
		/// </summary>
		/// <param name="e">要插入的元素</param>
		/// <param name="index">要插入的索引(从0开始)或者倒数索引(从-1开始)</param>
		/// <returns>成功返回<c>true</c></returns>
		public bool InsertElement(ConfigElement e, int index = -1)
		{
			if(index < 0)
			{
				index = elements.Count + index + 1;
				if(index < 0)
					index = 0;
			}
			else if(index > elements.Count)
				index = elements.Count;
			elements.Insert(index, e);
			return true;
		}
		/// <summary>
		/// 在末尾加入一组元素
		/// </summary>
		/// <param name="es">要加入的元素数组</param>
		/// <returns>成功返回<c>true</c></returns>
		public bool AddElements(ConfigElement[] es)
		{
			for(int i = 0; es != null && i < es.Length; i++)
			{
				elements.Add(es[i]);
			}
			return es != null;
		}
		/// <summary>
		/// 删除指定索引处的元素
		/// </summary>
		/// <param name="index">要删除元素所在的索引位置(从0开始)或倒数位置(从-1开始)</param>
		/// <returns>成功返回<c>true</c></returns>
		public bool RemoveElementAt(int index = -1)
		{
			if(index < 0)
			{
				index = elements.Count + index + 1;
				if(index < 0)
					index = 0;
			}
			else if(index > elements.Count)
				index = elements.Count;
			elements.RemoveAt(index);
			return true;
		}
		/// <summary>
		/// 删除指定节点元素
		/// </summary>
		/// <param name="value">要删除的节点元素</param>
		/// <returns>成功返回true，失败或不存在返回false</returns>
		public bool RemoveElement(ConfigElement value)
		{
			return elements.Remove(value);
		}
		/// <summary>
		/// 获取子节点数组
		/// </summary>
		/// <returns>子节点数组</returns>
		public ConfigElement[] ToArray()
		{
			return elements.ToArray();
		}
		/// <summary>
		/// 清空所有子节点
		/// </summary>
		/// <returns></returns>
		public bool Clear()
		{
			elements.Clear();
			return true;
		}
		/// <summary>
		/// 获取子节点总数
		/// </summary>
		public int Count
		{
			get
			{
				return elements.Count;
			}
		}
		/// <summary>
		/// 获取指定索引位置处的子节点
		/// </summary>
		/// <param name="index">要获取的子节点的索引</param>
		/// <returns></returns>
		public ConfigElement this[int index]
		{
			get
			{
				if(elements.Count > index)
					return elements[index];
				return null;
			}
			set
			{
				if(elements.Count > index)
					elements[index] = value;
				else if(elements.Count == index)
					InsertElement(value, -1);
				else
					throw new Exception("Error index!");
			}
		}
		/// <summary>
		/// 父节点引用
		/// </summary>
		private ConfigElement parent = null;
		/// <summary>
		/// 子节点列表
		/// </summary>
		private List<ConfigElement> elements = new List<ConfigElement>();
	}
	/// <summary>
	/// 定义配置文件结点的单个属性的类
	/// </summary>
	public class ConfigAttribute
	{
		/// <summary>
		/// 根据指定的键值，创建属性
		/// </summary>
		/// <param name="key">属性名（键）</param>
		/// <param name="value">属性值</param>
		public ConfigAttribute(string key, string value)
		{
			this.key = key;
			this.value = value;
		}

		/// <summary>
		/// 属性名（键）
		/// </summary>
		public string key = null;
		/// <summary>
		/// 属性值
		/// </summary>
		public string value = null;
		/// <summary>
		/// 将一段文本字符串，逐行切割，并去掉空行、首尾处的空格和\t,以及和所有换行符（\r以及\n）
		/// </summary>
		/// <param name="src">要处理的字符串</param>
		/// <returns>切割后的字符串数组</returns>
		public static string[] CutToLines(string src)
		{
			var ret = new List<string>();
			ret.Insert(0, "");
			for(int i = 0, j = 0; i < src.Length; i++)
			{
				if(src[i] != '\n')
					ret[j] += src[i];
				else if(ret[j] == "" && src[i] == ' ')
					continue;
				else if(ret[j] != "")
				{
					j++;
					ret.Insert(j, "");
				}

			}
			if(ret[ret.Count - 1] == "")
				ret.Remove("");
			return ret.ToArray();
		}
	}
}
