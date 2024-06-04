namespace CDScriptManager
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            menuStrip1 = new MenuStrip();
            editToolStripMenuItem = new ToolStripMenuItem();
            runGameToolStripMenuItem = new ToolStripMenuItem();
            setGameExecutableToolStripMenuItem = new ToolStripMenuItem();
            presetsToolStripMenuItem = new ToolStripMenuItem();
            newPresetToolStripMenuItem = new ToolStripMenuItem();
            openPresetToolStripMenuItem = new ToolStripMenuItem();
            savePresetAsToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            informationToolStripMenuItem = new ToolStripMenuItem();
            gitHubPageToolStripMenuItem = new ToolStripMenuItem();
            button1 = new Button();
            checkedListBox1 = new CheckedListBox();
            label1 = new Label();
            groupBox1 = new GroupBox();
            label2 = new Label();
            button2 = new Button();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            menuStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, presetsToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(456, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { runGameToolStripMenuItem, setGameExecutableToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(50, 20);
            editToolStripMenuItem.Text = "Game";
            // 
            // runGameToolStripMenuItem
            // 
            runGameToolStripMenuItem.Name = "runGameToolStripMenuItem";
            runGameToolStripMenuItem.Size = new Size(183, 22);
            runGameToolStripMenuItem.Text = "Run game";
            runGameToolStripMenuItem.Click += runGameToolStripMenuItem_Click;
            // 
            // setGameExecutableToolStripMenuItem
            // 
            setGameExecutableToolStripMenuItem.Name = "setGameExecutableToolStripMenuItem";
            setGameExecutableToolStripMenuItem.Size = new Size(183, 22);
            setGameExecutableToolStripMenuItem.Text = "Set game executable";
            setGameExecutableToolStripMenuItem.Click += setGameExecutableToolStripMenuItem_Click;
            // 
            // presetsToolStripMenuItem
            // 
            presetsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newPresetToolStripMenuItem, openPresetToolStripMenuItem, savePresetAsToolStripMenuItem });
            presetsToolStripMenuItem.Name = "presetsToolStripMenuItem";
            presetsToolStripMenuItem.Size = new Size(56, 20);
            presetsToolStripMenuItem.Text = "Presets";
            // 
            // newPresetToolStripMenuItem
            // 
            newPresetToolStripMenuItem.Name = "newPresetToolStripMenuItem";
            newPresetToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newPresetToolStripMenuItem.Size = new Size(219, 22);
            newPresetToolStripMenuItem.Text = "New preset";
            newPresetToolStripMenuItem.Click += newPresetToolStripMenuItem_Click;
            // 
            // openPresetToolStripMenuItem
            // 
            openPresetToolStripMenuItem.Name = "openPresetToolStripMenuItem";
            openPresetToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openPresetToolStripMenuItem.Size = new Size(219, 22);
            openPresetToolStripMenuItem.Text = "Open preset";
            openPresetToolStripMenuItem.Click += openPresetToolStripMenuItem_Click;
            // 
            // savePresetAsToolStripMenuItem
            // 
            savePresetAsToolStripMenuItem.Name = "savePresetAsToolStripMenuItem";
            savePresetAsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            savePresetAsToolStripMenuItem.Size = new Size(219, 22);
            savePresetAsToolStripMenuItem.Text = "Save preset as";
            savePresetAsToolStripMenuItem.Click += savePresetAsToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { informationToolStripMenuItem, gitHubPageToolStripMenuItem });
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(52, 20);
            aboutToolStripMenuItem.Text = "About";
            // 
            // informationToolStripMenuItem
            // 
            informationToolStripMenuItem.Name = "informationToolStripMenuItem";
            informationToolStripMenuItem.ShortcutKeys = Keys.F1;
            informationToolStripMenuItem.Size = new Size(156, 22);
            informationToolStripMenuItem.Text = "Information";
            informationToolStripMenuItem.Click += informationToolStripMenuItem_Click;
            // 
            // gitHubPageToolStripMenuItem
            // 
            gitHubPageToolStripMenuItem.Name = "gitHubPageToolStripMenuItem";
            gitHubPageToolStripMenuItem.Size = new Size(156, 22);
            gitHubPageToolStripMenuItem.Text = "GitHub page";
            gitHubPageToolStripMenuItem.Click += gitHubPageToolStripMenuItem_Click;
            // 
            // button1
            // 
            button1.Enabled = false;
            button1.Location = new Point(370, 416);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "Configure";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // checkedListBox1
            // 
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new Point(12, 27);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(204, 364);
            checkedListBox1.TabIndex = 3;
            checkedListBox1.ItemCheck += checkedListBox1_ItemCheck;
            checkedListBox1.SelectedIndexChanged += checkedListBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.BorderStyle = BorderStyle.Fixed3D;
            label1.Location = new Point(0, 401);
            label1.Name = "label1";
            label1.Size = new Size(1000, 2);
            label1.TabIndex = 4;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label2);
            groupBox1.Location = new Point(222, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(222, 364);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Information";
            // 
            // label2
            // 
            label2.Location = new Point(6, 19);
            label2.Name = "label2";
            label2.Size = new Size(210, 342);
            label2.TabIndex = 0;
            label2.Text = "label2";
            // 
            // button2
            // 
            button2.Location = new Point(12, 416);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 2;
            button2.Text = "Refresh";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(456, 451);
            Controls.Add(groupBox1);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(checkedListBox1);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CDScriptManager";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem presetsToolStripMenuItem;
        private ToolStripMenuItem newPresetToolStripMenuItem;
        private ToolStripMenuItem openPresetToolStripMenuItem;
        private ToolStripMenuItem savePresetAsToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem informationToolStripMenuItem;
        private ToolStripMenuItem gitHubPageToolStripMenuItem;
        private Button button1;
        private CheckedListBox checkedListBox1;
        private Label label1;
        private GroupBox groupBox1;
        private Label label2;
        private Button button2;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem setGameExecutableToolStripMenuItem;
        private ToolStripMenuItem runGameToolStripMenuItem;
    }
}