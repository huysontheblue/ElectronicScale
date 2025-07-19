namespace ElectronicScale
{
    partial class weigh
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
            date1 = new DateTimePicker();
            date2 = new DateTimePicker();
            dataGridView1 = new DataGridView();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            button1 = new Button();
            button2 = new Button();
            txtApn = new TextBox();
            label6 = new Label();
            btnDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 20F);
            label1.Location = new Point(411, 9);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(224, 45);
            label1.TabIndex = 4;
            label1.Text = "数据库已称重";
            // 
            // date1
            // 
            date1.CustomFormat = "yyyy/MM/dd HH：mm:ss";
            date1.Format = DateTimePickerFormat.Custom;
            date1.Location = new Point(90, 96);
            date1.Name = "date1";
            date1.Size = new Size(189, 27);
            date1.TabIndex = 5;
            // 
            // date2
            // 
            date2.CustomFormat = "yyyy/MM/dd HH：mm:ss";
            date2.Format = DateTimePickerFormat.Custom;
            date2.Location = new Point(382, 96);
            date2.Name = "date2";
            date2.Size = new Size(189, 27);
            date2.TabIndex = 6;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(2, 150);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1147, 449);
            dataGridView1.TabIndex = 7;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 617);
            label2.Name = "label2";
            label2.Size = new Size(47, 20);
            label2.TabIndex = 8;
            label2.Text = "数量：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(74, 617);
            label3.Name = "label3";
            label3.Size = new Size(50, 20);
            label3.TabIndex = 9;
            label3.Text = "label3";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(13, 103);
            label4.Name = "label4";
            label4.Size = new Size(71, 20);
            label4.TabIndex = 10;
            label4.Text = "开始时间";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(304, 103);
            label5.Name = "label5";
            label5.Size = new Size(72, 20);
            label5.TabIndex = 11;
            label5.Text = "结束时间";
            // 
            // button1
            // 
            button1.Location = new Point(885, 96);
            button1.Name = "button1";
            button1.Size = new Size(94, 27);
            button1.TabIndex = 12;
            button1.Text = "搜索";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(1031, 96);
            button2.Name = "button2";
            button2.Size = new Size(89, 27);
            button2.TabIndex = 13;
            button2.Text = "导出";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // txtApn
            // 
            txtApn.Location = new Point(673, 96);
            txtApn.Name = "txtApn";
            txtApn.Size = new Size(158, 27);
            txtApn.TabIndex = 14;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(629, 103);
            label6.Name = "label6";
            label6.Size = new Size(38, 20);
            label6.TabIndex = 15;
            label6.Text = "APN";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(1031, 614);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(94, 27);
            btnDelete.TabIndex = 16;
            btnDelete.Text = "删除";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // weigh
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1151, 653);
            Controls.Add(btnDelete);
            Controls.Add(label6);
            Controls.Add(txtApn);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(dataGridView1);
            Controls.Add(date2);
            Controls.Add(date1);
            Controls.Add(label1);
            Name = "weigh";
            Text = "weigh";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private DateTimePicker date1;
        private DateTimePicker date2;
        private DataGridView dataGridView1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button button1;
        private Button button2;
        private TextBox txtApn;
        private Label label6;
        private Button btnDelete;
    }
}