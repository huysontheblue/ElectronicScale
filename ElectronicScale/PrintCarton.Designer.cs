namespace ElectronicScale
{
    partial class PrintCarton
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
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 16F);
            label1.Location = new Point(135, 29);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(262, 41);
            label1.TabIndex = 2;
            label1.Text = "打印标签(In lại tem)";
            label1.UseCompatibleTextRendering = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(31, 97);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(501, 27);
            textBox1.TabIndex = 4;
            // 
            // PrintCarton
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(568, 160);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Name = "PrintCarton";
            Text = "PrintCarton";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
    }
}