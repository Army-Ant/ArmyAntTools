using System.Collections.Generic;
using System.IO;

namespace ArmyAnt
{
    public class JsonFile : AConfigFile
    {
		/// <summary>
		/// 载入JSON配置文件
		/// </summary>
		/// <param name="filename">要载入的JSON配置文件</param>
		public JsonFile(string filename = null) : base(filename)
        {
        }
		/// <summary>
		/// 从另一个配置数据引用创建JSON格式配置数据对象
		/// </summary>
		/// <param name="value">要共享数据引用的另一个配置数据对象</param>
		public JsonFile(AConfigFile value) : base(value)
        {
        }
		/// <summary>
		/// 从JSON格式文本载入XML配置
		/// <para><see cref="AConfigFile.LoadString(string)"/></para>
		/// <para><see cref="IConfigFile.LoadString(string)"/></para>
		/// </summary>
		/// <param name="text">要载入的JSON格式文本</param>
		/// <returns>成功返回<c>true</c></returns>
		override public bool LoadString(string text)
        {
            var ret = JsonToElement(text);
            if(ret == null)
                return false;
            else if(ret.Children.Length <= 0)
            {
                root = new ConfigElementCollection();
                root.InsertElement(ret);
            }
            else
            {
                root = new ConfigElementCollection();
                root.AddElements(ret.Children);
            }
            return true;
        }
		/// <summary>
		/// 将配置数据转化为JSON格式文本
		/// <para><see cref="AConfigFile.ToString()"/></para>
		/// <para><see cref="IConfigFile.ToString()"/></para>
		/// </summary>
		/// <returns>转化后的JSON格式配置文本</returns>
		override public string ToString()
        {
            string str = "[\n";
            
            for(int i = 0; root != null && i < root.Count; i++)
            {
                str += root[i].GetText(EConfigType.Json) + "\n";
            }
            str += "]";
            return str;
        }
		/// <summary>
		/// 表示JSON格式对象的类型
		/// </summary>
        internal enum RootType : byte
        {
            Unsolved,
            Value,
            Object,
            Array,
            Constructor,
        }
		/// <summary>
		/// 将JSON格式文本转化为配置数据元素
		/// </summary>
		/// <param name="text">要转化的JSON格式文本</param>
		/// <returns>返回转化后的配置数据元素</returns>
        public static ConfigElement JsonToElement(string text)
        {
            var ret = new ConfigElement("", null);
            bool isJsonArray = false;
            var strs = JsonCutStrings(text);
            if(strs == null)
                return null;
            switch(strs[0])
            {
                case "[":
                    if(strs[strs.Length - 1] != "]")
                        return null;
                    isJsonArray = true;
                    break;
                case "{":
                    if(strs[strs.Length - 1] != "}")
                        return null;
                    isJsonArray = false;
                    break;
                default:
                    return null;
            }
            string parser = "";
            int depth = 0;
            if(isJsonArray)
            {
                for(int i = 1; i < strs.Length - 1; i++)
                {
                    if(strs[i] == "[" || strs[i] == "{")
                    {
                        parser += strs[i];
                        depth++;
                    }
                    else if(strs[i] == "]" || strs[i] == "}")
                    {
                        parser += strs[i];
                        if(--depth == 0)
                        {
                            ret.AddChild(JsonToElement(parser));
                            parser = "";
                        }
                        else if(depth < 0)
                            return null;
                    }
                    else if(depth > 0)
                        parser += strs[i];
                    else if(depth < 0)
                        return null;
                    else if(strs[i] == ":")
                        return null;
                    else if(strs[i] == ",")
                    {
                        if(parser == "")
                            return null;
                        ret.AddChild(new ConfigElement(parser, null));
                        parser = "";
                    }
                    else
                        parser += strs[i];
                }
                if(depth > 0|| strs[strs.Length - 2] == ",")
                    return null;
                if(parser != "")
                    ret.AddChild(new ConfigElement(parser, null));
            }
            else
            {
                string key = "";
                for(int i = 1; i < strs.Length - 1; i++)
                {
                    if(key != "" && !((key[0] == '\'' && key[key.Length - 1] == '\'') || (key[0] == '"' && key[key.Length - 1] == '"')))
                        return null;
                    if(strs[i] == "[" || strs[i] == "{")
                    {
                        if(key == "")
                            return null;
                        parser += strs[i];
                        depth++;
                    }
                    else if(strs[i] == "]" || strs[i] == "}")
                    {
                        parser += strs[i];
                        if(--depth == 0)
                        {
                            ToAddAttr(ret, key, parser);
                            parser = "";
                            key = "";
                        }
                        else if(depth < 0)
                            return null;
                    }
                    else if(depth > 0)
                        parser += strs[i];
                    else if(depth < 0)
                        return null;
                    else if(strs[i + 1] == ":")
                    {
                        if(key != "")
                            return null;
                        key = strs[i];
                    }
                    else if(strs[i] == ":")
                    {
                        continue;
                    }
                    else if(strs[i] == ",")
                    {
                        if(key == "")
                            return null;
                        ToAddAttr(ret, key, parser);
                        parser = "";
                        key = "";
                    }
                    else
                        parser += strs[i];
                }
                if(depth > 0 || strs[strs.Length - 2] == ",")
                    return null;
                if(key != "")
                {
                    if(parser != "")
                        ToAddAttr(ret, key, parser);
                    else
                        return null;
                }
                else if(parser != "")
                    return null;
            }

            return ret;
        }
		/// <summary>
		/// 根据JSON文本内容进行属性添加，如果文本内容是个内嵌的数组或对象，则按照子节点进行解析，并添加到子节点集合中
		/// </summary>
		/// <param name="parent">要添加到的本(父)节点</param>
		/// <param name="key">属性(节点)的名称</param>
		/// <param name="value">值文本内容</param>
		private static void ToAddAttr(ConfigElement parent, string key, string value)
		{
			if(value[0] == '[' || value[0] == '{')
			{
				var elem = new ConfigElement(value, EConfigType.Json);
				elem.name = key;
				key = key.Remove(key.Length - 1).Remove(0, 1);
				parent.AddChild(elem);
			}
			else
			{
				parent.AddAttribute(key, value);
			}
		}
		/// <summary>
		/// 按照JSON格式对JSON文本进行符号解析
		/// </summary>
		/// <param name="text">要解析的JSON文本</param>
		/// <returns>返回解析后的字段数组</returns>
        private static string[] JsonCutStrings(string text)
        {
            var strs = new List<string>();
            string tmp = "";
            bool isSingleStringStart = false;
            bool isDoubleStringStart = false;
            char[] oper = { '{', '}', '[', ']', ':', ',' };
            for(int i = 0; i < text.Length; i++)
            {
                if(isSingleStringStart)
                {
                    if(text[i] == '\'')
                    {
                        strs.Add(tmp + "'");
                        tmp = "";
                        isSingleStringStart = false;
                    }
                    else
                        tmp += text[i];
                }
                else if(isDoubleStringStart)
                {
                    if(text[i] == '"')
                    {
                        strs.Add(tmp + '"');
                        tmp = "";
                        isDoubleStringStart = false;
                    }
                    else
                        tmp += text[i];
                }
                else if(text[i] == '\'')
                {
                    if(tmp != "")
                        strs.Add(tmp);
                    tmp = "'";
                    isSingleStringStart = true;
                }
                else if(text[i] == '"')
                {
                    if(tmp != "")
                        strs.Add(tmp);
                    tmp = "\"";
                    isDoubleStringStart = true;
                }
                else if(text[i] == ' ' || text[i] == '\r' || text[i] == '\n' || text[i] == '\t')
                    continue;
                else if(System.Array.IndexOf(oper, text[i]) < 0)
                {
                    tmp += text[i];
                }
                else
                {
                    if(tmp != "")
                        strs.Add(tmp);
                    tmp = "";
                    strs.Add("" + text[i]);
                }
            }
            if(isSingleStringStart || isDoubleStringStart)
                return null;
            if(tmp != "")
                strs.Add(tmp);
            return strs.ToArray();
        }
    }
}