using System.Management.Automation;
using System.Windows.Forms;

namespace HST_WINDOWS_UTILITY
{
    partial class HST
    {
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HST));
            panel1 = new Panel();
            label1 = new Label();
            clbDEB = new CheckedListBox();
            btnDEB = new Button();
            btnREG = new Button();
            btnTS = new Button();
            clbSS = new CheckedListBox();
            btnSS = new Button();
            panel8 = new Panel();
            btnCRP = new Button();
            btnDU = new Button();
            btnDM = new Button();
            btnLV = new Button();
            btnAPP = new Button();
            panel2 = new Panel();
            label2 = new Label();
            btnExit = new PictureBox();
            panel3 = new Panel();
            tbES = new TextBox();
            tbCPU = new TextBox();
            label10 = new Label();
            llGithub = new LinkLabel();
            pictureBox1 = new PictureBox();
            tbRAM = new TextBox();
            tbGPU = new TextBox();
            panel4 = new Panel();
            btnCU = new Button();
            label4 = new Label();
            clbCU = new CheckedListBox();
            tbStatus = new TextBox();
            pictureBox2 = new PictureBox();
            toolTip1 = new ToolTip(components);
            panel1.SuspendLayout();
            panel8.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnExit).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(0, 8, 20);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(clbDEB);
            panel1.Controls.Add(btnDEB);
            panel1.Location = new Point(526, 222);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(225, 198);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(83, 156, 254);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(70, 19);
            label1.TabIndex = 3;
            label1.Text = "DEBLOAT";
            // 
            // clbDEB
            // 
            clbDEB.BackColor = Color.FromArgb(0, 8, 20);
            clbDEB.BorderStyle = BorderStyle.None;
            clbDEB.Font = new Font("Segoe UI", 9F);
            clbDEB.ForeColor = Color.White;
            clbDEB.FormattingEnabled = true;
            clbDEB.Items.AddRange(new object[] { "MS APPS", "EDGE", "ONEDRIVE", "XBOX", "STORE" });
            clbDEB.Location = new Point(26, 20);
            clbDEB.Margin = new Padding(4);
            clbDEB.Name = "clbDEB";
            clbDEB.Size = new Size(180, 90);
            clbDEB.TabIndex = 2;
            // 
            // btnDEB
            // 
            btnDEB.BackColor = Color.FromArgb(0, 8, 20);
            btnDEB.FlatAppearance.BorderColor = Color.FromArgb(30, 40, 60);
            btnDEB.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 15, 30);
            btnDEB.FlatStyle = FlatStyle.Flat;
            btnDEB.Font = new Font("Segoe UI", 9F);
            btnDEB.ForeColor = Color.FromArgb(102, 155, 188);
            btnDEB.Location = new Point(26, 118);
            btnDEB.Margin = new Padding(4);
            btnDEB.Name = "btnDEB";
            btnDEB.Size = new Size(180, 60);
            btnDEB.TabIndex = 1;
            btnDEB.Text = "DEBLOAT";
            toolTip1.SetToolTip(btnDEB, "Removes selected pre-installed Windows applications.");
            btnDEB.UseVisualStyleBackColor = false;
            btnDEB.Click += btnDEB_Click;
            // 
            // btnREG
            // 
            btnREG.BackColor = Color.FromArgb(0, 8, 20);
            btnREG.FlatAppearance.BorderColor = Color.FromArgb(30, 40, 60);
            btnREG.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 15, 30);
            btnREG.FlatStyle = FlatStyle.Flat;
            btnREG.Font = new Font("Segoe UI", 9F);
            btnREG.ForeColor = Color.FromArgb(102, 155, 188);
            btnREG.Location = new Point(43, 98);
            btnREG.Margin = new Padding(15);
            btnREG.Name = "btnREG";
            btnREG.Size = new Size(140, 60);
            btnREG.TabIndex = 1;
            btnREG.Text = "REGISTRY";
            toolTip1.SetToolTip(btnREG, "Optimizes Windows registry settings for performance.");
            btnREG.UseVisualStyleBackColor = false;
            btnREG.Click += btnREG_Click;
            // 
            // btnTS
            // 
            btnTS.BackColor = Color.FromArgb(0, 8, 20);
            btnTS.FlatAppearance.BorderColor = Color.FromArgb(30, 40, 60);
            btnTS.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 15, 30);
            btnTS.FlatStyle = FlatStyle.Flat;
            btnTS.Font = new Font("Segoe UI", 9F);
            btnTS.ForeColor = Color.FromArgb(102, 155, 188);
            btnTS.Location = new Point(43, 178);
            btnTS.Margin = new Padding(15);
            btnTS.Name = "btnTS";
            btnTS.Size = new Size(140, 60);
            btnTS.TabIndex = 1;
            btnTS.Text = "TASK SCHEDULER";
            toolTip1.SetToolTip(btnTS, "Disables non-essential scheduled tasks.");
            btnTS.UseVisualStyleBackColor = false;
            btnTS.Click += btnTS_Click;
            // 
            // clbSS
            // 
            clbSS.BackColor = Color.FromArgb(0, 8, 20);
            clbSS.BorderStyle = BorderStyle.None;
            clbSS.Font = new Font("Segoe UI", 9F);
            clbSS.ForeColor = Color.White;
            clbSS.FormattingEnabled = true;
            clbSS.Items.AddRange(new object[] { "INCLUDE BLUETOOTH", "INCLUDE HYPER-V", "INCLUDE XBOX" });
            clbSS.Location = new Point(26, 21);
            clbSS.Margin = new Padding(4);
            clbSS.Name = "clbSS";
            clbSS.RightToLeft = RightToLeft.No;
            clbSS.Size = new Size(180, 54);
            clbSS.TabIndex = 2;
            // 
            // btnSS
            // 
            btnSS.BackColor = Color.FromArgb(0, 8, 20);
            btnSS.FlatAppearance.BorderColor = Color.FromArgb(30, 40, 60);
            btnSS.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 15, 30);
            btnSS.FlatStyle = FlatStyle.Flat;
            btnSS.Font = new Font("Segoe UI", 9F);
            btnSS.ForeColor = Color.FromArgb(102, 155, 188);
            btnSS.Location = new Point(26, 82);
            btnSS.Margin = new Padding(4);
            btnSS.Name = "btnSS";
            btnSS.Size = new Size(180, 60);
            btnSS.TabIndex = 1;
            btnSS.Text = "SERVICES";
            toolTip1.SetToolTip(btnSS, "Optimizes Windows services based on selected options.");
            btnSS.UseVisualStyleBackColor = false;
            btnSS.Click += btnSS_Click;
            // 
            // panel8
            // 
            panel8.BackColor = Color.FromArgb(0, 8, 20);
            panel8.Controls.Add(btnTS);
            panel8.Controls.Add(btnCRP);
            panel8.Controls.Add(btnDU);
            panel8.Controls.Add(btnDM);
            panel8.Controls.Add(btnREG);
            panel8.Controls.Add(btnLV);
            panel8.Controls.Add(btnAPP);
            panel8.Location = new Point(13, 42);
            panel8.Margin = new Padding(4);
            panel8.Name = "panel8";
            panel8.Size = new Size(225, 578);
            panel8.TabIndex = 0;
            // 
            // btnCRP
            // 
            btnCRP.BackColor = Color.FromArgb(0, 8, 20);
            btnCRP.FlatAppearance.BorderColor = Color.FromArgb(30, 40, 60);
            btnCRP.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 15, 30);
            btnCRP.FlatStyle = FlatStyle.Flat;
            btnCRP.Font = new Font("Segoe UI", 9F);
            btnCRP.ForeColor = Color.FromArgb(0, 0, 192);
            btnCRP.Location = new Point(43, 18);
            btnCRP.Name = "btnCRP";
            btnCRP.Size = new Size(140, 60);
            btnCRP.TabIndex = 5;
            btnCRP.Text = "RESTORE POINT";
            btnCRP.UseVisualStyleBackColor = false;
            btnCRP.Click += btnCRP_Click;
            // 
            // btnDU
            // 
            btnDU.BackColor = Color.FromArgb(0, 8, 20);
            btnDU.FlatAppearance.BorderColor = Color.FromArgb(30, 40, 60);
            btnDU.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 15, 30);
            btnDU.FlatStyle = FlatStyle.Flat;
            btnDU.Font = new Font("Segoe UI", 9F);
            btnDU.ForeColor = Color.FromArgb(102, 155, 188);
            btnDU.Location = new Point(43, 258);
            btnDU.Margin = new Padding(15);
            btnDU.Name = "btnDU";
            btnDU.Size = new Size(140, 60);
            btnDU.TabIndex = 1;
            btnDU.Text = "DISABLE UPDATES";
            toolTip1.SetToolTip(btnDU, "Disables Windows Update services and automatic updates.");
            btnDU.UseVisualStyleBackColor = false;
            btnDU.Click += btnDU_Click;
            // 
            // btnDM
            // 
            btnDM.BackColor = Color.FromArgb(0, 8, 20);
            btnDM.FlatAppearance.BorderColor = Color.FromArgb(30, 40, 60);
            btnDM.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 15, 30);
            btnDM.FlatStyle = FlatStyle.Flat;
            btnDM.Font = new Font("Segoe UI", 9F);
            btnDM.ForeColor = Color.FromArgb(102, 155, 188);
            btnDM.Location = new Point(43, 418);
            btnDM.Margin = new Padding(15);
            btnDM.Name = "btnDM";
            btnDM.Size = new Size(140, 60);
            btnDM.TabIndex = 1;
            btnDM.Text = "DARK MODE";
            toolTip1.SetToolTip(btnDM, "Switches Windows to dark mode theme.");
            btnDM.UseVisualStyleBackColor = false;
            btnDM.Click += btnDM_Click;
            // 
            // btnLV
            // 
            btnLV.BackColor = Color.FromArgb(0, 8, 20);
            btnLV.FlatAppearance.BorderColor = Color.FromArgb(30, 40, 60);
            btnLV.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 15, 30);
            btnLV.FlatStyle = FlatStyle.Flat;
            btnLV.Font = new Font("Segoe UI", 9F);
            btnLV.ForeColor = Color.FromArgb(102, 155, 188);
            btnLV.Location = new Point(43, 338);
            btnLV.Margin = new Padding(15);
            btnLV.Name = "btnLV";
            btnLV.Size = new Size(140, 60);
            btnLV.TabIndex = 1;
            btnLV.Text = "LOWER VISUALS";
            toolTip1.SetToolTip(btnLV, "Reduces visual effects and animations for better performance.");
            btnLV.UseVisualStyleBackColor = false;
            btnLV.Click += btnLV_Click;
            // 
            // btnAPP
            // 
            btnAPP.BackColor = Color.FromArgb(0, 8, 20);
            btnAPP.FlatAppearance.BorderColor = Color.FromArgb(30, 40, 60);
            btnAPP.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 15, 30);
            btnAPP.FlatStyle = FlatStyle.Flat;
            btnAPP.Font = new Font("Segoe UI", 9F);
            btnAPP.ForeColor = Color.FromArgb(102, 155, 188);
            btnAPP.Location = new Point(43, 498);
            btnAPP.Name = "btnAPP";
            btnAPP.Size = new Size(140, 60);
            btnAPP.TabIndex = 5;
            btnAPP.Text = "POWERPLAN";
            toolTip1.SetToolTip(btnAPP, "Activates a high-performance power plan.");
            btnAPP.UseVisualStyleBackColor = false;
            btnAPP.Click += btnAPP_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(0, 8, 20);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(clbSS);
            panel2.Controls.Add(btnSS);
            panel2.Location = new Point(524, 42);
            panel2.Margin = new Padding(4);
            panel2.Name = "panel2";
            panel2.Size = new Size(225, 160);
            panel2.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(83, 156, 254);
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(70, 19);
            label2.TabIndex = 3;
            label2.Text = "SERVICES";
            // 
            // btnExit
            // 
            btnExit.Image = Properties.Resources.close;
            btnExit.Location = new Point(719, 7);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(33, 30);
            btnExit.SizeMode = PictureBoxSizeMode.Zoom;
            btnExit.TabIndex = 2;
            btnExit.TabStop = false;
            btnExit.Click += btnExit_Click;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(0, 8, 20);
            panel3.Controls.Add(tbES);
            panel3.Controls.Add(tbCPU);
            panel3.Controls.Add(label10);
            panel3.Controls.Add(llGithub);
            panel3.Controls.Add(pictureBox1);
            panel3.Controls.Add(tbRAM);
            panel3.Controls.Add(tbGPU);
            panel3.Location = new Point(270, 340);
            panel3.Margin = new Padding(4);
            panel3.Name = "panel3";
            panel3.Size = new Size(225, 245);
            panel3.TabIndex = 0;
            // 
            // tbES
            // 
            tbES.BackColor = Color.FromArgb(0, 8, 20);
            tbES.BorderStyle = BorderStyle.None;
            tbES.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            tbES.ForeColor = Color.FromArgb(83, 156, 254);
            tbES.Location = new Point(14, 170);
            tbES.Name = "tbES";
            tbES.ReadOnly = true;
            tbES.Size = new Size(197, 16);
            tbES.TabIndex = 5;
            tbES.TabStop = false;
            tbES.TextAlign = HorizontalAlignment.Center;
            // 
            // tbCPU
            // 
            tbCPU.BackColor = Color.FromArgb(0, 8, 20);
            tbCPU.BorderStyle = BorderStyle.None;
            tbCPU.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            tbCPU.ForeColor = Color.FromArgb(83, 156, 254);
            tbCPU.Location = new Point(14, 83);
            tbCPU.Name = "tbCPU";
            tbCPU.ReadOnly = true;
            tbCPU.Size = new Size(197, 16);
            tbCPU.TabIndex = 5;
            tbCPU.TabStop = false;
            tbCPU.TextAlign = HorizontalAlignment.Center;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label10.ForeColor = Color.FromArgb(83, 156, 254);
            label10.Location = new Point(0, 0);
            label10.Name = "label10";
            label10.Size = new Size(99, 19);
            label10.TabIndex = 2;
            label10.Text = "SYSTEM INFO";
            // 
            // llGithub
            // 
            llGithub.AutoSize = true;
            llGithub.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            llGithub.LinkBehavior = LinkBehavior.NeverUnderline;
            llGithub.LinkColor = Color.FromArgb(46, 164, 79);
            llGithub.Location = new Point(21, 226);
            llGithub.Name = "llGithub";
            llGithub.Size = new Size(53, 15);
            llGithub.TabIndex = 4;
            llGithub.TabStop = true;
            llGithub.Text = "GITHUB";
            llGithub.TextAlign = ContentAlignment.MiddleCenter;
            llGithub.LinkClicked += llGithub_LinkClicked;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(0, 8, 20);
            pictureBox1.Image = Properties.Resources.github_logo;
            pictureBox1.Location = new Point(-7, 223);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(38, 21);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // tbRAM
            // 
            tbRAM.BackColor = Color.FromArgb(0, 8, 20);
            tbRAM.BorderStyle = BorderStyle.None;
            tbRAM.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            tbRAM.ForeColor = Color.FromArgb(83, 156, 254);
            tbRAM.Location = new Point(14, 127);
            tbRAM.Name = "tbRAM";
            tbRAM.ReadOnly = true;
            tbRAM.Size = new Size(197, 16);
            tbRAM.TabIndex = 5;
            tbRAM.TabStop = false;
            tbRAM.TextAlign = HorizontalAlignment.Center;
            // 
            // tbGPU
            // 
            tbGPU.BackColor = Color.FromArgb(0, 8, 20);
            tbGPU.BorderStyle = BorderStyle.None;
            tbGPU.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            tbGPU.ForeColor = Color.FromArgb(83, 156, 254);
            tbGPU.Location = new Point(14, 38);
            tbGPU.Name = "tbGPU";
            tbGPU.ReadOnly = true;
            tbGPU.Size = new Size(197, 16);
            tbGPU.TabIndex = 5;
            tbGPU.TabStop = false;
            tbGPU.TextAlign = HorizontalAlignment.Center;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(0, 8, 20);
            panel4.Controls.Add(btnCU);
            panel4.Controls.Add(label4);
            panel4.Controls.Add(clbCU);
            panel4.Location = new Point(526, 439);
            panel4.Margin = new Padding(4);
            panel4.Name = "panel4";
            panel4.Size = new Size(225, 181);
            panel4.TabIndex = 0;
            // 
            // btnCU
            // 
            btnCU.BackColor = Color.FromArgb(0, 8, 20);
            btnCU.FlatAppearance.BorderColor = Color.FromArgb(30, 40, 60);
            btnCU.FlatAppearance.MouseOverBackColor = Color.FromArgb(5, 15, 30);
            btnCU.FlatStyle = FlatStyle.Flat;
            btnCU.Font = new Font("Segoe UI", 9F);
            btnCU.ForeColor = Color.FromArgb(102, 155, 188);
            btnCU.Location = new Point(24, 100);
            btnCU.Name = "btnCU";
            btnCU.Size = new Size(183, 60);
            btnCU.TabIndex = 4;
            btnCU.Text = "CLEANUP";
            toolTip1.SetToolTip(btnCU, "Cleans temporary files and system data based on selected options.");
            btnCU.UseVisualStyleBackColor = false;
            btnCU.Click += btnCU_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(83, 156, 254);
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(72, 19);
            label4.TabIndex = 3;
            label4.Text = "CLEANUP";
            // 
            // clbCU
            // 
            clbCU.BackColor = Color.FromArgb(0, 8, 20);
            clbCU.BorderStyle = BorderStyle.None;
            clbCU.Font = new Font("Segoe UI", 9F);
            clbCU.ForeColor = Color.White;
            clbCU.FormattingEnabled = true;
            clbCU.Items.AddRange(new object[] { "TEMP", "CACHE", "EVENT LOGS", "POWERPLANS" });
            clbCU.Location = new Point(24, 21);
            clbCU.Margin = new Padding(4);
            clbCU.Name = "clbCU";
            clbCU.RightToLeft = RightToLeft.No;
            clbCU.Size = new Size(183, 72);
            clbCU.TabIndex = 2;
            // 
            // tbStatus
            // 
            tbStatus.BackColor = Color.FromArgb(14, 19, 31);
            tbStatus.BorderStyle = BorderStyle.None;
            tbStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            tbStatus.ForeColor = Color.FromArgb(83, 156, 254);
            tbStatus.Location = new Point(284, 60);
            tbStatus.Multiline = true;
            tbStatus.Name = "tbStatus";
            tbStatus.ReadOnly = true;
            tbStatus.Size = new Size(197, 76);
            tbStatus.TabIndex = 4;
            tbStatus.TextAlign = HorizontalAlignment.Center;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.hst_high_resolution_logo_transparent;
            pictureBox2.Location = new Point(270, 183);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(225, 103);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(14, 19, 31);
            ClientSize = new Size(765, 633);
            Controls.Add(tbStatus);
            Controls.Add(pictureBox2);
            Controls.Add(panel2);
            Controls.Add(btnExit);
            Controls.Add(panel8);
            Controls.Add(panel3);
            Controls.Add(panel4);
            Controls.Add(panel1);
            Font = new Font("Segoe UI", 10F);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(6, 4, 6, 4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "HST";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel8.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)btnExit).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel panel1;
        private CheckedListBox clbDEB;
        private Label label1;
        private Button btnDEB;
        private Button btnREG;
        private Button btnTS;
        private CheckedListBox clbSS;
        private Button btnSS;
        private Panel panel8;
        private Button btnDU;
        private Label label10;
        private Panel panel2;
        private Label label2;
        private PictureBox btnExit;
        private Panel panel3;
        private TextBox tbES;
        private TextBox tbRAM;
        private TextBox tbGPU;
        private TextBox tbCPU;
        private LinkLabel llGithub;
        private PictureBox pictureBox1;
        private Button btnDM;
        private Button btnLV;
        private Panel panel4;
        private Label label4;
        private CheckedListBox clbCU;
        private PictureBox pictureBox2;
        private Button btnCU;
        private TextBox tbStatus;
        private Button btnCRP;
        private Button btnAPP;
        private ToolTip toolTip1;
        private System.ComponentModel.IContainer components;
    }
}