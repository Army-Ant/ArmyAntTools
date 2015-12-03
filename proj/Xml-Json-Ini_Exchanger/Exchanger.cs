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
        ConfigFile()
        {
        }
        bool LoadFile(string filename,EConfigType type)
        {
            switch(type)
            {
                case EConfigType.Ini:
                    file = new IniFile();
                    break;
                case EConfigType.Xml:
                    file = new IniFile();
                    break;
                case EConfigType.Json:
                    file = new IniFile();
                    break;
                case EConfigType.Sql:
                    file = new IniFile();
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
        bool SaveFile(string filename, EConfigType type)
        {
            switch(type)
            {
                case EConfigType.Ini:
                case EConfigType.Xml:
                case EConfigType.Json:
                case EConfigType.Sql:
                    return file.ToFile(filename);
            }
            return false;
        }
        bool Load(IConfigFile cfg)
        {
            file = cfg;
            return file != null;
        }
        string Text
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
        EConfigType Type
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
