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
            txtSN = new TextBox();
            label2 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 16F);
            label1.Location = new Point(137, 18);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(262, 41);
            label1.TabIndex = 2;
            label1.Text = "打印标签(In lại tem)";
            label1.UseCompatibleTextRendering = true;
            // 
            // txtSN
            // 
            txtSN.Location = new Point(30, 108);
            txtSN.Name = "txtSN";
            txtSN.Size = new Size(501, 27);
            txtSN.TabIndex = 4;
            txtSN.KeyPress += txtSN_KeyPress;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(30, 74);
            label2.Name = "label2";
            label2.Size = new Size(50, 20);
            label2.TabIndex = 5;
            label2.Text = "label2";
            // 
            // PrintCarton
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(568, 160);
            Controls.Add(label2);
            Controls.Add(txtSN);
            Controls.Add(label1);
            Name = "PrintCarton";
            Text = "PrintCarton";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtSN;
        private Label label2;
    }
}