using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArmyAnt;

namespace ArmyAnt
{
    public partial class ConfigFileParser : Form
    {
        private ConfigFile file = null;
        public ConfigFileParser()
        {
            InitializeComponent();
        }

        private void ConfigFileParser_Load(object sender, EventArgs e)
        {
            comboFileType.SelectedIndex = 2;
            comboTargetType.SelectedIndex = 0;
        }

        private void buttonFileSelect_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();
            switch(comboFileType.SelectedIndex)
            {
                case 0:
                    fd.Filter = "INI config files (*.ini)|*.ini";
                    break;
                case 1:
                    fd.Filter = "XML files (*.xml)|*.xml|HTML files (*.html, *.html5, *.htm, *.xhtml)|*.html;*.html5;*.htm;*.xhtml";
                    break;
                case 2:
                    fd.Filter = "JSON files (*.json)|*.json|Javascript code files (*.js)|*.js";
                    break;
                case 3:
                    fd.Filter = "SQL data files (*.sql)|*.sql|Windows log files (*.vtx)|*.vtx|Excel files (*.xlsx)|*.xlsx|Excel 97-2003 files (*.xls)|*.xls|Access files (*.accdb)|*.accdb|Access 2000-2003 files (*.mdb)|*.mdb";
                    break;
            }
            fd.Filter = fd.Filter + "|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if(textFilePath.Text == "")
                fd.FileName = "test";
            else
                fd.FileName = textFilePath.Text;
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;
            fd.AutoUpgradeEnabled = true;
            fd.Multiselect = false;
            fd.Title = "选择要打开的配置文件";
            switch(fd.ShowDialog(this))
            {
                case DialogResult.OK:
                    textFilePath.Text = fd.FileName;
                    break;
            }
        }

        private void buttonTargetSelect_Click(object sender, EventArgs e)
        {
            var fd = new SaveFileDialog();
            switch(comboTargetType.SelectedIndex)
            {
                case 0:
                    fd.Filter = "INI config files (*.ini)|*.ini";
                    break;
                case 1:
                    fd.Filter = "XML files (*.xml)|*.xml|HTML files (*.html, *.html5, *.htm, *.xhtml)|*.html;*.html5;*.htm;*.xhtml";
                    break;
                case 2:
                    fd.Filter = "JSON files (*.json)|*.json|Javascript code files (*.js)|*.js";
                    break;
                case 3:
                    fd.Filter = "SQL data files (*.sql)|*.sql|Windows log files (*.vtx)|*.vtx|Excel files (*.xlsx)|*.xlsx|Excel 97-2003 files (*.xls)|*.xls|Access files (*.accdb)|*.accdb|Access 2000-2003 files (*.mdb)|*.mdb";
                    break;
            }
            fd.Filter = fd.Filter + "|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if(textTargetPath.Text == "")
                fd.FileName = "test";
            else
                fd.FileName = textTargetPath.Text;
            fd.CheckFileExists = false;
            fd.CreatePrompt = false;
            fd.CheckPathExists = false;
            fd.OverwritePrompt = true;
            fd.AutoUpgradeEnabled = true;
            fd.ValidateNames = true;
            fd.Title = "选择要保存的位置";
            switch(fd.ShowDialog(this))
            {
                case DialogResult.OK:
                    textTargetPath.Text = fd.FileName;
                    break;
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonDoParse_Click(object sender, EventArgs e)
        {
            if(textFilePath.Text == "")
            {
                buttonFileSelect_Click(sender, e);
                if(textFilePath.Text == "")
                    return;
            }
            if(textTargetPath.Text == "")
            {
                buttonTargetSelect_Click(sender, e);
                if(textTargetPath.Text == "")
                    return;
            }
            if(MessageBox.Show(this, "Are you sure to do exchange ?", "执行数据转换", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;
            file = new ConfigFile();
            EConfigType loadType = (EConfigType)(comboFileType.SelectedIndex + 1);
            EConfigType saveType = (EConfigType)(comboTargetType.SelectedIndex + 1);
			if(file.LoadFile(textFilePath.Text, loadType) && file.SaveFile(textTargetPath.Text, saveType))
				MessageBox.Show(this, "Parsing successful !", "执行数据转换");
			else
				MessageBox.Show(this, "Parsing failed !", "执行数据转换");
        }
    }
}
