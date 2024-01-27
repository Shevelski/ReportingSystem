namespace Utils
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
            label1 = new Label();
            label2 = new Label();
            e1 = new TextBox();
            d1 = new TextBox();
            e2 = new TextBox();
            d2 = new TextBox();
            ecopy = new Button();
            dcopy = new Button();
            dpaste = new Button();
            epaste = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(160, 23);
            label1.Name = "label1";
            label1.Size = new Size(44, 15);
            label1.TabIndex = 0;
            label1.Text = "Encript";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(160, 163);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 1;
            label2.Text = "Decript";
            // 
            // e1
            // 
            e1.Location = new Point(24, 20);
            e1.Name = "e1";
            e1.Size = new Size(100, 23);
            e1.TabIndex = 4;
            e1.TextChanged += e1_TextChanged;
            // 
            // d1
            // 
            d1.Location = new Point(24, 114);
            d1.Name = "d1";
            d1.Size = new Size(196, 23);
            d1.TabIndex = 5;
            d1.TextChanged += d1_TextChanged;
            // 
            // e2
            // 
            e2.Location = new Point(24, 58);
            e2.Name = "e2";
            e2.Size = new Size(195, 23);
            e2.TabIndex = 6;
            // 
            // d2
            // 
            d2.Location = new Point(24, 156);
            d2.Name = "d2";
            d2.Size = new Size(100, 23);
            d2.TabIndex = 7;
            // 
            // ecopy
            // 
            ecopy.Location = new Point(260, 58);
            ecopy.Name = "ecopy";
            ecopy.Size = new Size(75, 23);
            ecopy.TabIndex = 8;
            ecopy.Text = "Copy";
            ecopy.UseVisualStyleBackColor = true;
            ecopy.Click += ecopy_Click;
            // 
            // dcopy
            // 
            dcopy.Location = new Point(260, 155);
            dcopy.Name = "dcopy";
            dcopy.Size = new Size(75, 23);
            dcopy.TabIndex = 9;
            dcopy.Text = "Copy";
            dcopy.UseVisualStyleBackColor = true;
            dcopy.Click += dcopy_Click;
            // 
            // dpaste
            // 
            dpaste.Location = new Point(260, 114);
            dpaste.Name = "dpaste";
            dpaste.Size = new Size(75, 23);
            dpaste.TabIndex = 10;
            dpaste.Text = "Paste";
            dpaste.UseVisualStyleBackColor = true;
            dpaste.Click += dpaste_Click;
            // 
            // epaste
            // 
            epaste.Location = new Point(260, 20);
            epaste.Name = "epaste";
            epaste.Size = new Size(75, 23);
            epaste.TabIndex = 11;
            epaste.Text = "Paste";
            epaste.UseVisualStyleBackColor = true;
            epaste.Click += epaste_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(435, 308);
            Controls.Add(epaste);
            Controls.Add(dpaste);
            Controls.Add(dcopy);
            Controls.Add(ecopy);
            Controls.Add(d2);
            Controls.Add(e2);
            Controls.Add(d1);
            Controls.Add(e1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox e1;
        private TextBox d1;
        private TextBox e2;
        private TextBox d2;
        private Button ecopy;
        private Button dcopy;
        private Button dpaste;
        private Button epaste;
    }
}