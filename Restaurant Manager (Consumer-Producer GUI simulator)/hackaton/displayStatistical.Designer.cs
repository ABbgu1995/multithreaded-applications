namespace hackaton
{
    partial class displayStatistical
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
            label2 = new Label();
            label1 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            pictureBoxFreeChairs = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFreeChairs).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(67, 165);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(77, 28);
            label2.TabIndex = 1;
            label2.Text = "Capcity";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.ActiveCaption;
            label1.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(232, 165);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.MinimumSize = new Size(55, 0);
            label1.Name = "label1";
            label1.Size = new Size(55, 46);
            label1.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.ActiveCaption;
            label3.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(232, 68);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.MinimumSize = new Size(55, 0);
            label3.Name = "label3";
            label3.Size = new Size(55, 46);
            label3.TabIndex = 4;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(67, 68);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(60, 28);
            label4.TabIndex = 3;
            label4.Text = "Clock";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = SystemColors.ActiveCaption;
            label5.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(232, 263);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.MinimumSize = new Size(55, 0);
            label5.Name = "label5";
            label5.Size = new Size(55, 46);
            label5.TabIndex = 6;
            label5.Click += label5_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(30, 263);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(174, 28);
            label6.TabIndex = 5;
            label6.Text = "How many waiting";
            label6.Click += label6_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = SystemColors.ActiveCaption;
            label7.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(232, 361);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.MinimumSize = new Size(55, 0);
            label7.Name = "label7";
            label7.Size = new Size(55, 46);
            label7.TabIndex = 8;
            label7.Click += label7_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(30, 361);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(157, 28);
            label8.TabIndex = 7;
            label8.Text = "avg waiting time";
            label8.Click += label8_Click;
            // 
            // pictureBoxFreeChairs
            // 
            pictureBoxFreeChairs.Location = new Point(807, 68);
            pictureBoxFreeChairs.Name = "pictureBoxFreeChairs";
            pictureBoxFreeChairs.Size = new Size(724, 576);
            pictureBoxFreeChairs.TabIndex = 9;
            pictureBoxFreeChairs.TabStop = false;
            // 
            // displayStatistical
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(1593, 795);
            Controls.Add(pictureBoxFreeChairs);
            Controls.Add(label7);
            Controls.Add(label8);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(label1);
            Controls.Add(label2);
            Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = SystemColors.ActiveCaptionText;
            Margin = new Padding(4, 5, 4, 5);
            MinimumSize = new Size(62, 47);
            Name = "displayStatistical";
            Text = "displayStatistical";
            ((System.ComponentModel.ISupportInitialize)pictureBoxFreeChairs).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private TextBox Capcity;
        private Label label1;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private PictureBox pictureBoxFreeChairs;
    }
}