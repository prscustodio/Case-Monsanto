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
            this.SerialButton = new System.Windows.Forms.Button();
            this.TelnetButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // SerialButton
            // 
            this.SerialButton.Location = new System.Drawing.Point(13, 12);
            this.SerialButton.Name = "SerialButton";
            this.SerialButton.Size = new System.Drawing.Size(132, 41);
            this.SerialButton.TabIndex = 5;
            this.SerialButton.Text = "Serial";
            this.SerialButton.UseVisualStyleBackColor = true;
            this.SerialButton.Click += new System.EventHandler(this.Serial_Click);
            // 
            // TelnetButton
            // 
            this.TelnetButton.Location = new System.Drawing.Point(152, 12);
            this.TelnetButton.Name = "TelnetButton";
            this.TelnetButton.Size = new System.Drawing.Size(137, 40);
            this.TelnetButton.TabIndex = 6;
            this.TelnetButton.Text = "Network";
            this.TelnetButton.UseVisualStyleBackColor = true;
            this.TelnetButton.Click += new System.EventHandler(this.Telnet_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 60);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(276, 193);
            this.textBox1.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 265);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.TelnetButton);
            this.Controls.Add(this.SerialButton);
            this.Name = "Form1";
            this.Text = "Weatherlink";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SerialButton;
        private System.Windows.Forms.Button TelnetButton;
        private System.Windows.Forms.TextBox textBox1;
    }
}

