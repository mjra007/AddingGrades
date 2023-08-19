namespace AddinGrades
{
    partial class CourseworkWeightControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.courseworkWeight = new System.Windows.Forms.TextBox();
            this.courseworkName = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 83);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Coursework Weight";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.courseworkName);
            this.groupBox2.Location = new System.Drawing.Point(6, 22);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(180, 56);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Coursework Name";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.courseworkWeight);
            this.groupBox3.Location = new System.Drawing.Point(192, 22);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(132, 61);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Weight";
            // 
            // courseworkWeight
            // 
            this.courseworkWeight.Location = new System.Drawing.Point(6, 22);
            this.courseworkWeight.Name = "courseworkWeight";
            this.courseworkWeight.Size = new System.Drawing.Size(120, 23);
            this.courseworkWeight.TabIndex = 0;
            // 
            // courseworkName
            // 
            this.courseworkName.AutoSize = true;
            this.courseworkName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.courseworkName.Location = new System.Drawing.Point(6, 19);
            this.courseworkName.Name = "courseworkName";
            this.courseworkName.Size = new System.Drawing.Size(0, 20);
            this.courseworkName.TabIndex = 0;
            // 
            // CourseworkWeightControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "CourseworkWeightControl";
            this.Size = new System.Drawing.Size(336, 91);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox3;
        private TextBox courseworkWeight;
        private GroupBox groupBox2;
        private Label courseworkName;
    }
}
