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
            groupBox1 = new GroupBox();
            groupBox3 = new GroupBox();
            courseworkWeight = new TextBox();
            groupBox2 = new GroupBox();
            courseworkName = new Label();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(330, 83);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Weight";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(courseworkWeight);
            groupBox3.Location = new Point(192, 22);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(132, 61);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "Weight %";
            // 
            // courseworkWeight
            // 
            courseworkWeight.Location = new Point(6, 22);
            courseworkWeight.Name = "courseworkWeight";
            courseworkWeight.Size = new Size(120, 23);
            courseworkWeight.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(courseworkName);
            groupBox2.Location = new Point(6, 22);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(180, 56);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Coursework Name";
            // 
            // courseworkName
            // 
            courseworkName.AutoSize = true;
            courseworkName.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            courseworkName.Location = new Point(6, 19);
            courseworkName.Name = "courseworkName";
            courseworkName.Size = new Size(0, 20);
            courseworkName.TabIndex = 0;
            // 
            // CourseworkWeightControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox1);
            Name = "CourseworkWeightControl";
            Size = new Size(336, 91);
            groupBox1.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox3;
        private TextBox courseworkWeight;
        private GroupBox groupBox2;
        private Label courseworkName;
    }
}
