namespace ElectronicScale
{
    partial class Settings
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
            groupBox1 = new GroupBox();
            cb_baudrate_1 = new ComboBox();
            cb_com_1 = new ComboBox();
            label2 = new Label();
            label1 = new Label();
            groupBox2 = new GroupBox();
            nud_dalarm_time = new NumericUpDown();
            nud_ualarm_time = new NumericUpDown();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label3 = new Label();
            label4 = new Label();
            rb_voice_off = new RadioButton();
            rb_voice_on = new RadioButton();
            cb_baudrate_2 = new ComboBox();
            cb_com_2 = new ComboBox();
            groupBox3 = new GroupBox();
            tb_filepath = new TextBox();
            label12 = new Label();
            bt_save = new Button();
            label8 = new Label();
            groupBox4 = new GroupBox();
            textBox1 = new TextBox();
            label10 = new Label();
            comboBox1 = new ComboBox();
            label9 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nud_dalarm_time).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nud_ualarm_time).BeginInit();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cb_baudrate_1);
            groupBox1.Controls.Add(cb_com_1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(45, 94);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(330, 151);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "电子秤";
            // 
            // cb_baudrate_1
            // 
            cb_baudrate_1.FormattingEnabled = true;
            cb_baudrate_1.Location = new Point(122, 82);
            cb_baudrate_1.Name = "cb_baudrate_1";
            cb_baudrate_1.Size = new Size(182, 32);
            cb_baudrate_1.TabIndex = 2;
            cb_baudrate_1.SelectionChangeCommitted += cb_baudrate_1_SelectionChangeCommitted;
            // 
            // cb_com_1
            // 
            cb_com_1.FormattingEnabled = true;
            cb_com_1.Location = new Point(122, 36);
            cb_com_1.Name = "cb_com_1";
            cb_com_1.Size = new Size(182, 32);
            cb_com_1.TabIndex = 2;
            cb_com_1.SelectionChangeCommitted += cb_com_1_SelectionChangeCommitted;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(22, 82);
            label2.Name = "label2";
            label2.Size = new Size(64, 24);
            label2.TabIndex = 1;
            label2.Text = "波特率";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(22, 37);
            label1.Name = "label1";
            label1.Size = new Size(55, 24);
            label1.TabIndex = 0;
            label1.Text = "COM";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(nud_dalarm_time);
            groupBox2.Controls.Add(nud_ualarm_time);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(rb_voice_off);
            groupBox2.Controls.Add(rb_voice_on);
            groupBox2.Controls.Add(cb_baudrate_2);
            groupBox2.Controls.Add(cb_com_2);
            groupBox2.Location = new Point(564, 94);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(330, 257);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "报警器";
            // 
            // nud_dalarm_time
            // 
            nud_dalarm_time.Location = new Point(162, 215);
            nud_dalarm_time.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            nud_dalarm_time.Name = "nud_dalarm_time";
            nud_dalarm_time.Size = new Size(142, 30);
            nud_dalarm_time.TabIndex = 9;
            nud_dalarm_time.ValueChanged += nud_dalarm_time_ValueChanged;
            // 
            // nud_ualarm_time
            // 
            nud_ualarm_time.Location = new Point(160, 172);
            nud_ualarm_time.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            nud_ualarm_time.Name = "nud_ualarm_time";
            nud_ualarm_time.Size = new Size(142, 30);
            nud_ualarm_time.TabIndex = 9;
            nud_ualarm_time.ValueChanged += nud_ualarm_time_ValueChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(22, 217);
            label7.Name = "label7";
            label7.Size = new Size(118, 24);
            label7.TabIndex = 6;
            label7.Text = "下限报警时长";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(22, 172);
            label6.Name = "label6";
            label6.Size = new Size(118, 24);
            label6.TabIndex = 5;
            label6.Text = "上限报警时长";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(22, 127);
            label5.Name = "label5";
            label5.Size = new Size(46, 24);
            label5.TabIndex = 4;
            label5.Text = "声音";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(22, 82);
            label3.Name = "label3";
            label3.Size = new Size(64, 24);
            label3.TabIndex = 1;
            label3.Text = "波特率";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(22, 37);
            label4.Name = "label4";
            label4.Size = new Size(55, 24);
            label4.TabIndex = 0;
            label4.Text = "COM";
            // 
            // rb_voice_off
            // 
            rb_voice_off.AutoSize = true;
            rb_voice_off.Location = new Point(204, 127);
            rb_voice_off.Name = "rb_voice_off";
            rb_voice_off.Size = new Size(53, 28);
            rb_voice_off.TabIndex = 8;
            rb_voice_off.TabStop = true;
            rb_voice_off.Text = "关";
            rb_voice_off.UseVisualStyleBackColor = true;
            rb_voice_off.CheckedChanged += rb_voice_off_CheckedChanged;
            // 
            // rb_voice_on
            // 
            rb_voice_on.AutoSize = true;
            rb_voice_on.Location = new Point(124, 127);
            rb_voice_on.Name = "rb_voice_on";
            rb_voice_on.Size = new Size(53, 28);
            rb_voice_on.TabIndex = 7;
            rb_voice_on.TabStop = true;
            rb_voice_on.Text = "开";
            rb_voice_on.UseVisualStyleBackColor = true;
            rb_voice_on.CheckedChanged += rb_voice_on_CheckedChanged;
            // 
            // cb_baudrate_2
            // 
            cb_baudrate_2.FormattingEnabled = true;
            cb_baudrate_2.Location = new Point(122, 79);
            cb_baudrate_2.Name = "cb_baudrate_2";
            cb_baudrate_2.Size = new Size(182, 32);
            cb_baudrate_2.TabIndex = 2;
            cb_baudrate_2.SelectionChangeCommitted += cb_baudrate_2_SelectionChangeCommitted;
            // 
            // cb_com_2
            // 
            cb_com_2.FormattingEnabled = true;
            cb_com_2.Location = new Point(122, 36);
            cb_com_2.Name = "cb_com_2";
            cb_com_2.Size = new Size(182, 32);
            cb_com_2.TabIndex = 2;
            cb_com_2.SelectionChangeCommitted += cb_com_2_SelectionChangeCommitted;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tb_filepath);
            groupBox3.Controls.Add(label12);
            groupBox3.Location = new Point(35, 452);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(859, 117);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "文件路径";
            // 
            // tb_filepath
            // 
            tb_filepath.Location = new Point(111, 43);
            tb_filepath.Name = "tb_filepath";
            tb_filepath.Size = new Size(738, 30);
            tb_filepath.TabIndex = 1;
            tb_filepath.DoubleClick += tb_filepath_DoubleClick;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(23, 49);
            label12.Name = "label12";
            label12.Size = new Size(82, 24);
            label12.TabIndex = 0;
            label12.Text = "文件路径";
            // 
            // bt_save
            // 
            bt_save.Location = new Point(362, 603);
            bt_save.Name = "bt_save";
            bt_save.Size = new Size(112, 34);
            bt_save.TabIndex = 5;
            bt_save.Text = "保存";
            bt_save.UseVisualStyleBackColor = true;
            bt_save.Click += bt_save_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft YaHei UI", 14F);
            label8.Location = new Point(420, 37);
            label8.Name = "label8";
            label8.Size = new Size(71, 36);
            label8.TabIndex = 6;
            label8.Text = "设置";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(textBox1);
            groupBox4.Controls.Add(label10);
            groupBox4.Controls.Add(comboBox1);
            groupBox4.Controls.Add(label9);
            groupBox4.Location = new Point(45, 253);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(330, 176);
            groupBox4.TabIndex = 7;
            groupBox4.TabStop = false;
            groupBox4.Text = "打印机";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(93, 101);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(231, 30);
            textBox1.TabIndex = 3;
            textBox1.KeyPress += textBox1_KeyPress;
            textBox1.Leave += textBox1_Leave;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(15, 107);
            label10.Name = "label10";
            label10.Size = new Size(80, 24);
            label10.TabIndex = 2;
            label10.Text = "打印机IP";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(93, 50);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(231, 32);
            comboBox1.TabIndex = 1;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(23, 56);
            label9.Name = "label9";
            label9.Size = new Size(64, 24);
            label9.TabIndex = 0;
            label9.Text = "打印机";
            // 
            // Settings
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(974, 696);
            Controls.Add(groupBox4);
            Controls.Add(label8);
            Controls.Add(bt_save);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(groupBox3);
            Name = "Settings";
            Text = "Settings";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nud_dalarm_time).EndInit();
            ((System.ComponentModel.ISupportInitialize)nud_ualarm_time).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private ComboBox cb_com_1;
        private Label label2;
        private Label label1;
        private GroupBox groupBox2;
        private ComboBox cb_com_2;
        private Label label3;
        private Label label4;
        private NumericUpDown nud_ualarm_time;
        private Label label7;
        private Label label6;
        private Label label5;
        private RadioButton rb_voice_off;
        private RadioButton rb_voice_on;
        private ComboBox cb_baudrate_1;
        private NumericUpDown nud_dalarm_time;
        private ComboBox cb_baudrate_2;
        private GroupBox groupBox3;
        private TextBox tb_filepath;
        private Label label12;
        private Button bt_save;
        private Label label8;
        private GroupBox groupBox4;
        private ComboBox comboBox1;
        private Label label9;
        private TextBox textBox1;
        private Label label10;
    }
}