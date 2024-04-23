namespace hackaton
{
    partial class mainScreen
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
            button1 = new Button();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            producers = new TextBox();
            consumers = new TextBox();
            producers_rate = new TextBox();
            consumers_rate = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(364, 428);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 50;
            button1.Text = "Srtart";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(323, 26);
            label5.Name = "label5";
            label5.Size = new Size(176, 20);
            label5.TabIndex = 49;
            label5.Text = "Amit And Barak Hummus";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(263, 346);
            label4.Name = "label4";
            label4.Size = new Size(109, 20);
            label4.TabIndex = 48;
            label4.Text = "consumers rate";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(263, 265);
            label3.Name = "label3";
            label3.Size = new Size(104, 20);
            label3.TabIndex = 47;
            label3.Text = "Producers rate";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(241, 182);
            label2.Name = "label2";
            label2.Size = new Size(155, 20);
            label2.TabIndex = 46;
            label2.Text = "Number of consumers";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(245, 94);
            label1.Name = "label1";
            label1.Size = new Size(151, 20);
            label1.TabIndex = 45;
            label1.Text = "Number of producers";
            // 
            // producers
            // 
            producers.Location = new Point(435, 91);
            producers.Name = "producers";
            producers.Size = new Size(125, 27);
            producers.TabIndex = 44;
            // 
            // consumers
            // 
            consumers.Location = new Point(435, 175);
            consumers.Name = "consumers";
            consumers.Size = new Size(125, 27);
            consumers.TabIndex = 43;
            consumers.TextChanged += consumers_TextChanged;
            // 
            // producers_rate
            // 
            producers_rate.Location = new Point(435, 258);
            producers_rate.Name = "producers_rate";
            producers_rate.Size = new Size(125, 27);
            producers_rate.TabIndex = 42;
            // 
            // consumers_rate
            // 
            consumers_rate.Location = new Point(435, 343);
            consumers_rate.Name = "consumers_rate";
            consumers_rate.Size = new Size(125, 27);
            consumers_rate.TabIndex = 41;
            // 
            // mainScreen
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(919, 536);
            Controls.Add(button1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(producers);
            Controls.Add(consumers);
            Controls.Add(producers_rate);
            Controls.Add(consumers_rate);
            Name = "mainScreen";
            Text = "mainScreen";
            Load += mainScreen_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox producers;
        private TextBox consumers;
        private TextBox producers_rate;
        private TextBox consumers_rate;
    }
}