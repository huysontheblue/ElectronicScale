namespace ElectronicScale
{
    partial class EditBaseData
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
            dataGridView1 = new DataGridView();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            button3 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(9, 110);
            dataGridView1.Margin = new Padding(2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(1220, 668);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellEndEdit += UpdateData;
            // 
            // button1
            // 
            button1.Location = new Point(17, 58);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(147, 36);
            button1.TabIndex = 1;
            button1.Text = "新增";
            button1.UseVisualStyleBackColor = true;
            button1.Click += AddData;
            // 
            // button2
            // 
            button2.Location = new Point(177, 58);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(156, 36);
            button2.TabIndex = 2;
            button2.Text = "删除";
            button2.UseVisualStyleBackColor = true;
            button2.Click += DeleteData;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 20F);
            label1.Location = new Point(471, 8);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(224, 45);
            label1.TabIndex = 3;
            label1.Text = "基础数据维护";
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(365, 61);
            textBox1.Margin = new Padding(2);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(615, 34);
            textBox1.TabIndex = 4;
            // 
            // button3
            // 
            button3.Location = new Point(1023, 61);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.Size = new Size(142, 37);
            button3.TabIndex = 5;
            button3.Text = "查找";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // EditBaseData
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1234, 788);
            Controls.Add(button3);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Margin = new Padding(2);
            Name = "EditBaseData";
            Text = "EditBaseData";
            Load += EditBaseData_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Button button1;
        private Button button2;
        private Label label1;
        private TextBox textBox1;
        private Button button3;
    }
}