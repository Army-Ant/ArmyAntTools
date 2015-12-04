namespace ArmyAnt
{
    partial class ConfigFileParser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonFileSelect = new System.Windows.Forms.Button();
            this.textFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboFileType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboTargetType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textTargetPath = new System.Windows.Forms.TextBox();
            this.buttonTargetSelect = new System.Windows.Forms.Button();
            this.buttonDoParse = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonFileSelect
            // 
            this.buttonFileSelect.Location = new System.Drawing.Point(329, 38);
            this.buttonFileSelect.Name = "buttonFileSelect";
            this.buttonFileSelect.Size = new System.Drawing.Size(38, 23);
            this.buttonFileSelect.TabIndex = 0;
            this.buttonFileSelect.Text = "...";
            this.buttonFileSelect.UseVisualStyleBackColor = true;
            this.buttonFileSelect.Click += new System.EventHandler(this.buttonFileSelect_Click);
            // 
            // textFilePath
            // 
            this.textFilePath.Location = new System.Drawing.Point(89, 39);
            this.textFilePath.Name = "textFilePath";
            this.textFilePath.ReadOnly = true;
            this.textFilePath.Size = new System.Drawing.Size(234, 21);
            this.textFilePath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "File type :";
            // 
            // comboFileType
            // 
            this.comboFileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFileType.FormattingEnabled = true;
            this.comboFileType.Items.AddRange(new object[] {
            "Ini",
            "Xml",
            "Json",
            "Sql"});
            this.comboFileType.Location = new System.Drawing.Point(90, 13);
            this.comboFileType.Name = "comboFileType";
            this.comboFileType.Size = new System.Drawing.Size(277, 20);
            this.comboFileType.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "File path :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Target type :";
            // 
            // comboTargetType
            // 
            this.comboTargetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTargetType.FormattingEnabled = true;
            this.comboTargetType.Items.AddRange(new object[] {
            "Ini",
            "Xml",
            "Json",
            "Sql"});
            this.comboTargetType.Location = new System.Drawing.Point(101, 66);
            this.comboTargetType.Name = "comboTargetType";
            this.comboTargetType.Size = new System.Drawing.Size(266, 20);
            this.comboTargetType.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "Target path :";
            // 
            // textTargetPath
            // 
            this.textTargetPath.Location = new System.Drawing.Point(101, 92);
            this.textTargetPath.Name = "textTargetPath";
            this.textTargetPath.ReadOnly = true;
            this.textTargetPath.Size = new System.Drawing.Size(222, 21);
            this.textTargetPath.TabIndex = 8;
            // 
            // buttonTargetSelect
            // 
            this.buttonTargetSelect.Location = new System.Drawing.Point(329, 91);
            this.buttonTargetSelect.Name = "buttonTargetSelect";
            this.buttonTargetSelect.Size = new System.Drawing.Size(38, 23);
            this.buttonTargetSelect.TabIndex = 9;
            this.buttonTargetSelect.Text = "...";
            this.buttonTargetSelect.UseVisualStyleBackColor = true;
            this.buttonTargetSelect.Click += new System.EventHandler(this.buttonTargetSelect_Click);
            // 
            // buttonDoParse
            // 
            this.buttonDoParse.Location = new System.Drawing.Point(72, 119);
            this.buttonDoParse.Name = "buttonDoParse";
            this.buttonDoParse.Size = new System.Drawing.Size(75, 23);
            this.buttonDoParse.TabIndex = 10;
            this.buttonDoParse.Text = "&DoParse";
            this.buttonDoParse.UseVisualStyleBackColor = true;
            this.buttonDoParse.Click += new System.EventHandler(this.buttonDoParse_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonExit.Location = new System.Drawing.Point(232, 119);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 11;
            this.buttonExit.Text = "&Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // ConfigFileParser
            // 
            this.AcceptButton = this.buttonDoParse;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonExit;
            this.ClientSize = new System.Drawing.Size(387, 153);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonDoParse);
            this.Controls.Add(this.buttonTargetSelect);
            this.Controls.Add(this.textTargetPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboTargetType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboFileType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textFilePath);
            this.Controls.Add(this.buttonFileSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigFileParser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Config File Parser";
            this.Load += new System.EventHandler(this.ConfigFileParser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonFileSelect;
        private System.Windows.Forms.TextBox textFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboFileType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboTargetType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textTargetPath;
        private System.Windows.Forms.Button buttonTargetSelect;
        private System.Windows.Forms.Button buttonDoParse;
        private System.Windows.Forms.Button buttonExit;
    }
}