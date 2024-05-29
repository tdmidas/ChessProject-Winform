namespace ChessUI
{
    partial class ChessServer
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
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            buttonStart = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new System.Drawing.Point(75, 127);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(667, 291);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // buttonStart
            // 
            buttonStart.Location = new System.Drawing.Point(75, 49);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new System.Drawing.Size(121, 45);
            buttonStart.TabIndex = 1;
            buttonStart.Text = "Connect";
            buttonStart.UseVisualStyleBackColor = true;
            buttonStart.Click += button1_Click;
            // 
            // ChessServer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(buttonStart);
            Controls.Add(richTextBox1);
            Name = "ChessServer";
            Text = "ChessServer";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button buttonStart;
    }
}