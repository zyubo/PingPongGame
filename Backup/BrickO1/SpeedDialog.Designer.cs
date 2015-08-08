namespace BrickO1
{
    partial class SpeedDialog
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
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FastRadio = new System.Windows.Forms.RadioButton();
            this.MediumRadio = new System.Windows.Forms.RadioButton();
            this.SlowRadio = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(65, 229);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FastRadio);
            this.groupBox1.Controls.Add(this.MediumRadio);
            this.groupBox1.Controls.Add(this.SlowRadio);
            this.groupBox1.Location = new System.Drawing.Point(24, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 144);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Speed";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // FastRadio
            // 
            this.FastRadio.AutoSize = true;
            this.FastRadio.Location = new System.Drawing.Point(28, 67);
            this.FastRadio.Name = "FastRadio";
            this.FastRadio.Size = new System.Drawing.Size(45, 17);
            this.FastRadio.TabIndex = 2;
            this.FastRadio.Text = "Fast";
            this.FastRadio.UseVisualStyleBackColor = true;
            // 
            // MediumRadio
            // 
            this.MediumRadio.AutoSize = true;
            this.MediumRadio.Location = new System.Drawing.Point(28, 43);
            this.MediumRadio.Name = "MediumRadio";
            this.MediumRadio.Size = new System.Drawing.Size(62, 17);
            this.MediumRadio.TabIndex = 1;
            this.MediumRadio.Text = "Medium";
            this.MediumRadio.UseVisualStyleBackColor = true;
            // 
            // SlowRadio
            // 
            this.SlowRadio.AutoSize = true;
            this.SlowRadio.Checked = true;
            this.SlowRadio.Location = new System.Drawing.Point(28, 19);
            this.SlowRadio.Name = "SlowRadio";
            this.SlowRadio.Size = new System.Drawing.Size(48, 17);
            this.SlowRadio.TabIndex = 0;
            this.SlowRadio.TabStop = true;
            this.SlowRadio.Text = "Slow";
            this.SlowRadio.UseVisualStyleBackColor = true;
            // 
            // SpeedDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 264);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Name = "SpeedDialog";
            this.Text = "Level";
            this.Load += new System.EventHandler(this.SpeedDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton FastRadio;
        private System.Windows.Forms.RadioButton MediumRadio;
        private System.Windows.Forms.RadioButton SlowRadio;
    }
}