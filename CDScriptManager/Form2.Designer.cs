namespace CDScriptManager
{
    partial class Form2
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            linkLabel1 = new LinkLabel();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = SystemColors.ControlText;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(364, 37);
            label1.TabIndex = 0;
            label1.Text = "Crashday Script Manager";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Italic, GraphicsUnit.Point);
            label2.ForeColor = SystemColors.ControlText;
            label2.Location = new Point(0, 37);
            label2.Name = "label2";
            label2.Size = new Size(364, 37);
            label2.TabIndex = 1;
            label2.Text = "CDScriptManager";
            label2.TextAlign = ContentAlignment.TopCenter;
            // 
            // label3
            // 
            label3.BorderStyle = BorderStyle.Fixed3D;
            label3.Dock = DockStyle.Top;
            label3.Location = new Point(0, 74);
            label3.Name = "label3";
            label3.Size = new Size(364, 2);
            label3.TabIndex = 2;
            // 
            // label4
            // 
            label4.Location = new Point(0, 76);
            label4.Name = "label4";
            label4.Padding = new Padding(10);
            label4.Size = new Size(100, 50);
            label4.TabIndex = 3;
            label4.Text = "Version";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.Location = new Point(151, 76);
            label5.Name = "label5";
            label5.Padding = new Padding(10);
            label5.Size = new Size(213, 50);
            label5.TabIndex = 4;
            label5.Text = "0.0.4 (2024.05.21)";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            label6.Location = new Point(0, 126);
            label6.Name = "label6";
            label6.Padding = new Padding(10);
            label6.Size = new Size(100, 50);
            label6.TabIndex = 5;
            label6.Text = "Author";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            label7.Location = new Point(264, 126);
            label7.Name = "label7";
            label7.Padding = new Padding(10);
            label7.Size = new Size(100, 50);
            label7.TabIndex = 6;
            label7.Text = "St1ngLeR";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.BorderStyle = BorderStyle.Fixed3D;
            label8.Location = new Point(0, 174);
            label8.Name = "label8";
            label8.Size = new Size(364, 2);
            label8.TabIndex = 7;
            // 
            // linkLabel1
            // 
            linkLabel1.Dock = DockStyle.Bottom;
            linkLabel1.Location = new Point(0, 176);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(364, 38);
            linkLabel1.TabIndex = 8;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "GitHub page";
            linkLabel1.TextAlign = ContentAlignment.MiddleCenter;
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(364, 214);
            Controls.Add(linkLabel1);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form2";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "About";
            Load += Form2_Load;
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private LinkLabel linkLabel1;
    }
}