using System.Collections.Generic;
using System.IO;

namespace ArmyAnt
{
    public class JsonFile : AConfigFile
    {
		/// <summary>
		/// ����JSON�����ļ�
		/// </summary>
		/// <param name="filename">Ҫ�����JSON�����ļ�</param>
		public JsonFile(string filename = null) : base(filename)
        {
        }
		/// <summary>
		/// ����һ�������������ô���JSON��ʽ�������ݶ���
		/// </summary>
		/// <param name="value">Ҫ�����������õ���һ���������ݶ���</param>
		public JsonFile(AConfigFile value) : base(value)
        {
        }
		/// <summary>
		/// ��JSON��ʽ�ı�����XML����
		/// <para><see cref="AConfigFile.LoadString(string)"/></para>
		/// <para><see cref="IConfigFile.LoadString(string)"/></para>
		/// </summary>
		/// <param name="text">Ҫ�����JSON��ʽ�ı�</param>
		/// <returns>�ɹ�����<c>true</c></returns>
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
		/// ����������ת��ΪJSON��ʽ�ı�
		/// <para><see cref="AConfigFile.ToString()"/></para>
		/// <para><see cref="IConfigFile.ToString()"/></para>
		/// </summary>
		/// <returns>ת�����JSON��ʽ�����ı�</returns>
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
		/// ��ʾJSON��ʽ���������
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
		/// ��JSON��ʽ�ı�ת��Ϊ��������Ԫ��
		/// </summary>
		/// <param name="text">Ҫת����JSON��ʽ�ı�</param>
		/// <returns>����ת�������������Ԫ��</returns>
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
		/// ����JSON�ı����ݽ���������ӣ�����ı������Ǹ���Ƕ���������������ӽڵ���н���������ӵ��ӽڵ㼯����
		/// </summary>
		/// <param name="parent">Ҫ��ӵ��ı�(��)�ڵ�</param>
		/// <param name="key">����(�ڵ�)������</param>
		/// <param name="value">ֵ�ı�����</param>
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
		/// ����JSON��ʽ��JSON�ı����з��Ž���
		/// </summary>
		/// <param name="text">Ҫ������JSON�ı�</param>
		/// <returns>���ؽ�������ֶ�����</returns>
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