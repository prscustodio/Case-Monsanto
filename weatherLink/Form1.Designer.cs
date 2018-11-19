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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BoxIP = new System.Windows.Forms.TextBox();
            this.BoxPorta = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // SerialButton
            // 
            this.SerialButton.Enabled = false;
            this.SerialButton.Location = new System.Drawing.Point(17, 15);
            this.SerialButton.Margin = new System.Windows.Forms.Padding(4);
            this.SerialButton.Name = "SerialButton";
            this.SerialButton.Size = new System.Drawing.Size(176, 50);
            this.SerialButton.TabIndex = 5;
            this.SerialButton.Text = "Serial";
            this.SerialButton.UseVisualStyleBackColor = true;
            this.SerialButton.Click += new System.EventHandler(this.Serial_Click);
            // 
            // TelnetButton
            // 
            this.TelnetButton.Enabled = false;
            this.TelnetButton.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.TelnetButton.Location = new System.Drawing.Point(203, 15);
            this.TelnetButton.Margin = new System.Windows.Forms.Padding(4);
            this.TelnetButton.Name = "TelnetButton";
            this.TelnetButton.Size = new System.Drawing.Size(183, 49);
            this.TelnetButton.TabIndex = 6;
            this.TelnetButton.Text = "Network";
            this.TelnetButton.UseVisualStyleBackColor = true;
            this.TelnetButton.Click += new System.EventHandler(this.Telnet_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(17, 134);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(367, 268);
            this.textBox1.TabIndex = 7;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "IP";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(289, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Porta";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // BoxIP
            // 
            this.BoxIP.Location = new System.Drawing.Point(17, 105);
            this.BoxIP.Name = "BoxIP";
            this.BoxIP.Size = new System.Drawing.Size(269, 22);
            this.BoxIP.TabIndex = 10;
            this.BoxIP.Text = "10.180.240.60";
            // 
            // BoxPorta
            // 
            this.BoxPorta.Location = new System.Drawing.Point(292, 104);
            this.BoxPorta.Name = "BoxPorta";
            this.BoxPorta.Size = new System.Drawing.Size(94, 22);
            this.BoxPorta.TabIndex = 11;
            this.BoxPorta.Text = "22222";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 411);
            this.Controls.Add(this.BoxPorta);
            this.Controls.Add(this.BoxIP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.TelnetButton);
            this.Controls.Add(this.SerialButton);
            this.Margin = new System.Windows.Forms.Padding(4);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox BoxIP;
        private System.Windows.Forms.TextBox BoxPorta;
    }
}

