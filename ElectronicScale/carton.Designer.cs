namespace ElectronicScale
{
    partial class carton
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
            label6 = new Label();
            txtApn = new TextBox();
            button2 = new Button();
            button1 = new Button();
            label5 = new Label();
            label4 = new Label();
            date2 = new DateTimePicker();
            date1 = new DateTimePicker();
            label1 = new Label();
            dataGridView1 = new DataGridView();
            btnDelete = new Button();
            label3 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(645, 115);
            label6.Name = "label6";
            label6.Size = new Size(38, 20);
            label6.TabIndex = 24;
            label6.Text = "APN";
            // 
            // txtApn
            // 
            txtApn.Location = new Point(689, 108);
            txtApn.Name = "txtApn";
            txtApn.Size = new Size(158, 27);
            txtApn.TabIndex = 23;
            // 
            // button2
            // 
            button2.Location = new Point(1047, 108);
            button2.Name = "button2";
            button2.Size = new Size(89, 27);
            button2.TabIndex = 22;
            button2.Text = "导出";
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(901, 108);
            button1.Name = "button1";
            button1.Size = new Size(94, 27);
            button1.TabIndex = 21;
            button1.Text = "搜索";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(320, 115);
            label5.Name = "label5";
            label5.Size = new Size(72, 20);
            label5.TabIndex = 20;
            label5.Text = "结束时间";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(29, 115);
            label4.Name = "label4";
            label4.Size = new Size(71, 20);
            label4.TabIndex = 19;
            label4.Text = "开始时间";
            // 
            // date2
            // 
            date2.CustomFormat = "yyyy/MM/dd HH：mm:ss";
            date2.Format = DateTimePickerFormat.Custom;
            date2.Location = new Point(398, 108);
            date2.Name = "date2";
            date2.Size = new Size(189, 27);
            date2.TabIndex = 18;
            // 
            // date1
            // 
            date1.CustomFormat = "yyyy/MM/dd HH：mm:ss";
            date1.Format = DateTimePickerFormat.Custom;
            date1.Location = new Point(106, 108);
            date1.Name = "date1";
            date1.Size = new Size(189, 27);
            date1.TabIndex = 17;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 20F);
            label1.Location = new Point(427, 21);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(224, 45);
            label1.TabIndex = 16;
            label1.Text = "数据库已包装";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(-2, 160);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1147, 449);
            dataGridView1.TabIndex = 25;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(1051, 616);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(94, 27);
            btnDelete.TabIndex = 28;
            btnDelete.Text = "删除";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(70, 619);
            label3.Name = "label3";
            label3.Size = new Size(50, 20);
            label3.TabIndex = 27;
            label3.Text = "label3";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 619);
            label2.Name = "label2";
            label2.Size = new Size(47, 20);
            label2.TabIndex = 26;
            label2.Text = "数量：";
            // 
            // carton
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1167, 648);
            Controls.Add(btnDelete);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(dataGridView1);
            Controls.Add(label6);
            Controls.Add(txtApn);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(date2);
            Controls.Add(date1);
            Controls.Add(label1);
            Name = "carton";
            Text = "carton";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label6;
        private TextBox txtApn;
        private Button button2;
        private Button button1;
        private Label label5;
        private Label label4;
        private DateTimePicker date2;
        private DateTimePicker date1;
        private Label label1;
        private DataGridView dataGridView1;
        private Button btnDelete;
        private Label label3;
        private Label label2;
    }
}