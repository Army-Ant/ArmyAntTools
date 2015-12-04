namespace ArmyAnt
{
    partial class MainForm
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
            this.buttonConfigFileParser = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonConfigFileParser
            // 
            this.buttonConfigFileParser.Location = new System.Drawing.Point(34, 295);
            this.buttonConfigFileParser.Name = "buttonConfigFileParser";
            this.buttonConfigFileParser.Size = new System.Drawing.Size(212, 23);
            this.buttonConfigFileParser.TabIndex = 0;
            this.buttonConfigFileParser.Text = "&Config File Parser (on test)";
            this.buttonConfigFileParser.UseVisualStyleBackColor = true;
            this.buttonConfigFileParser.Click += new System.EventHandler(this.buttonConfigFileParser_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonExit.Location = new System.Drawing.Point(420, 295);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 1;
            this.buttonExit.Text = "&Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.buttonConfigFileParser;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonExit;
            this.ClientSize = new System.Drawing.Size(538, 399);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonConfigFileParser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Army Ant Developer Tools";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonConfigFileParser;
        private System.Windows.Forms.Button buttonExit;
    }
}