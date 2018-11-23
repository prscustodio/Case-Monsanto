namespace WeatherLink
{
    partial class Form1
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
            if (disposing && (components != null))
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
            this.FilePathTb = new System.Windows.Forms.TextBox();
            this.FilePathLbl = new System.Windows.Forms.Label();
            this.LogTb = new System.Windows.Forms.TextBox();
            this.LogLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // FilePathTb
            // 
            this.FilePathTb.Location = new System.Drawing.Point(17, 56);
            this.FilePathTb.Name = "FilePathTb";
            this.FilePathTb.Size = new System.Drawing.Size(367, 22);
            this.FilePathTb.TabIndex = 1;
            // 
            // FilePathLbl
            // 
            this.FilePathLbl.AutoSize = true;
            this.FilePathLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilePathLbl.Location = new System.Drawing.Point(14, 28);
            this.FilePathLbl.Name = "FilePathLbl";
            this.FilePathLbl.Size = new System.Drawing.Size(320, 20);
            this.FilePathLbl.TabIndex = 0;
            this.FilePathLbl.Text = "Caminho do Arquivo de Configuração";
            this.FilePathLbl.Click += new System.EventHandler(this.FilePathLbl_Click);
            // 
            // LogTb
            // 
            this.LogTb.Location = new System.Drawing.Point(17, 124);
            this.LogTb.Margin = new System.Windows.Forms.Padding(4);
            this.LogTb.Multiline = true;
            this.LogTb.Name = "LogTb";
            this.LogTb.Size = new System.Drawing.Size(367, 268);
            this.LogTb.TabIndex = 2;
            this.LogTb.TextChanged += new System.EventHandler(this.LogTb_TextChanged);
            // 
            // LogLbl
            // 
            this.LogLbl.AutoSize = true;
            this.LogLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogLbl.Location = new System.Drawing.Point(14, 100);
            this.LogLbl.Name = "LogLbl";
            this.LogLbl.Size = new System.Drawing.Size(40, 20);
            this.LogLbl.TabIndex = 3;
            this.LogLbl.Text = "Log";
            this.LogLbl.Click += new System.EventHandler(this.label1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 405);
            this.Controls.Add(this.LogLbl);
            this.Controls.Add(this.FilePathLbl);
            this.Controls.Add(this.FilePathTb);
            this.Controls.Add(this.LogTb);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Weatherlink";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label FilePathLbl;
        private System.Windows.Forms.TextBox FilePathTb;
        private System.Windows.Forms.TextBox LogTb;
        private System.Windows.Forms.Label LogLbl;
    }
}

