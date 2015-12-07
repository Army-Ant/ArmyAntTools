namespace ArmyAnt
{
	/// <summary>
	/// 代表配置文件格式的枚举
	/// </summary>
    public enum EConfigType
    {
        Null,
        Ini,
        Xml,
        Json,
        Sql,
    }
	/// <summary>
	/// 配置文件存取转换器
	/// </summary>
	public class ConfigFile
	{
		/// <summary>
		/// 读取配置文件
		/// </summary>
		/// <param name="filename">要读取的配置文件名</param>
		/// <param name="type">配置文件格式</param>
		/// <returns>成功返回<c>true</c></returns>
		public bool LoadFile(string filename, EConfigType type)
		{
			switch(type)
			{
				case EConfigType.Ini:
					file = new IniFile();
					break;
				case EConfigType.Xml:
					file = new XmlFile();
					break;
				case EConfigType.Json:
					file = new JsonFile();
					break;
				case EConfigType.Sql:
					//file = new IniFile();
					break;
				default:
					return false;
			}
			this.type = type;
			if(file.LoadFile(filename))
				return true;
			file = null;
			this.type = EConfigType.Null;
			return false;
		}
		/// <summary>
		/// 保存为指定格式的配置文件
		/// </summary>
		/// <param name="filename">要保存到的文件</param>
		/// <param name="type">要保存的格式</param>
		/// <returns>成功返回<c>true</c></returns>
		public bool SaveFile(string filename, EConfigType type)
		{
			switch(type)
			{
				case EConfigType.Ini:
					return new IniFile((AConfigFile)file).ToFile(filename);
				case EConfigType.Xml:
					return new XmlFile((AConfigFile)file).ToFile(filename);
				case EConfigType.Json:
					return new JsonFile((AConfigFile)file).ToFile(filename);
				case EConfigType.Sql:
					return new IniFile((AConfigFile)file).ToFile(filename);
			}
			return false;
		}
		/// <summary>
		/// 从打开的配置数据对象读取配置数据
		/// </summary>
		/// <param name="cfg">要读取的配置数据对象</param>
		/// <returns>成功返回<c>true</c></returns>
		public bool Load(IConfigFile cfg)
		{
			file = cfg;
			return file != null;
		}
		/// <summary>
		/// 获取原格式的配置数据文本，或者按照原格式输入新的配置数据文本
		/// </summary>
		public string Text
		{
			get
			{
				if(file == null)
					return null;
				return file.Text;
			}
			set
			{
				if(file != null)
					file.Text = value;
			}
		}
		/// <summary>
		/// 获取原配置格式
		/// </summary>
		public EConfigType Type
		{
			get
			{
				return type;
			}
		}
		/// <summary>
		/// 检查本实例是否已经载入了数据
		/// </summary>
		public bool IsLoaded
		{
			get
			{
				return file != null && type != EConfigType.Null;
			}
		}
		/// <summary>
		/// 用于处理配置数据的对象
		/// </summary>
        private IConfigFile file = null;
		/// <summary>
		/// 配置数据源的格式
		/// </summary>
        private EConfigType type = EConfigType.Null;
    }
}
