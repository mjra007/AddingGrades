namespace AddinGrades
{
    partial class AddCoursework
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.courseworkInput = new System.Windows.Forms.TextBox();
            this.courseworkList = new System.Windows.Forms.CheckedListBox();
            this.removeCoursework = new System.Windows.Forms.Button();
            this.AddCourseworkButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.courseworkInput);
            this.groupBox1.Controls.Add(this.courseworkList);
            this.groupBox1.Controls.Add(this.removeCoursework);
            this.groupBox1.Controls.Add(this.AddCourseworkButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(376, 195);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add/Remove coursework";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // courseworkInput
            // 
            this.courseworkInput.Location = new System.Drawing.Point(89, 166);
            this.courseworkInput.Name = "courseworkInput";
            this.courseworkInput.Size = new System.Drawing.Size(147, 23);
            this.courseworkInput.TabIndex = 9;
            // 
            // courseworkList
            // 
            this.courseworkList.FormattingEnabled = true;
            this.courseworkList.Location = new System.Drawing.Point(6, 20);
            this.courseworkList.Name = "courseworkList";
            this.courseworkList.Size = new System.Drawing.Size(364, 130);
            this.courseworkList.TabIndex = 8;
            // 
            // removeCoursework
            // 
            this.removeCoursework.Location = new System.Drawing.Point(242, 166);
            this.removeCoursework.Name = "removeCoursework";
            this.removeCoursework.Size = new System.Drawing.Size(128, 23);
            this.removeCoursework.TabIndex = 7;
            this.removeCoursework.Text = "Remove Selected";
            this.removeCoursework.UseVisualStyleBackColor = true;
            this.removeCoursework.Click += new System.EventHandler(this.removeCoursework_Click);
            // 
            // AddCourseworkButton
            // 
            this.AddCourseworkButton.Location = new System.Drawing.Point(6, 166);
            this.AddCourseworkButton.Name = "AddCourseworkButton";
            this.AddCourseworkButton.Size = new System.Drawing.Size(77, 23);
            this.AddCourseworkButton.TabIndex = 6;
            this.AddCourseworkButton.Text = "Add";
            this.AddCourseworkButton.UseVisualStyleBackColor = true;
            this.AddCourseworkButton.Click += new System.EventHandler(this.AddCourseworkClick);
            // 
            // AddCoursework
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 211);
            this.Controls.Add(this.groupBox1);
            this.MaximumSize = new System.Drawing.Size(412, 250);
            this.MinimumSize = new System.Drawing.Size(412, 250);
            this.Name = "AddCoursework";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddCoursework";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private Button removeCoursework;
        private Button AddCourseworkButton;
        private TextBox courseworkInput;
        private CheckedListBox courseworkList;
    }
}