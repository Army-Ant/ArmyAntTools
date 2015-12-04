namespace ArmyAnt
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
        public ConfigFile()
        {
        }
        public bool LoadFile(string filename,EConfigType type)
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
        public bool Load(IConfigFile cfg)
        {
            file = cfg;
            return file != null;
        }
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
        public EConfigType Type
        {
            get
            {
                return type;
            }
        }
        private IConfigFile file = null;
        private EConfigType type = EConfigType.Null;
    }
}
