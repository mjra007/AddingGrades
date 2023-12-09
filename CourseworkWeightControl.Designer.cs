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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.percentageTotalGradeLabel = new System.Windows.Forms.Label();
            this.deleteCourseworkBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.courseworkWeight = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.courseworkName = new System.Windows.Forms.Label();
            this.deleteGroup = new System.Windows.Forms.GroupBox();
            this.noButton = new System.Windows.Forms.Button();
            this.yesButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.deleteGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.deleteCourseworkBtn);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(331, 83);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Weight";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.percentageTotalGradeLabel);
            this.groupBox4.Location = new System.Drawing.Point(237, 24);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(94, 53);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "% Total Grade";
            // 
            // percentageTotalGradeLabel
            // 
            this.percentageTotalGradeLabel.AutoSize = true;
            this.percentageTotalGradeLabel.Location = new System.Drawing.Point(15, 22);
            this.percentageTotalGradeLabel.Name = "percentageTotalGradeLabel";
            this.percentageTotalGradeLabel.Size = new System.Drawing.Size(0, 15);
            this.percentageTotalGradeLabel.TabIndex = 0;
            // 
            // deleteCourseworkBtn
            // 
            this.deleteCourseworkBtn.BackColor = System.Drawing.Color.Transparent;
            this.deleteCourseworkBtn.BackgroundImage = global::AddinGrades.Properties.Resources.close;
            this.deleteCourseworkBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.deleteCourseworkBtn.FlatAppearance.BorderSize = 0;
            this.deleteCourseworkBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteCourseworkBtn.ForeColor = System.Drawing.Color.Transparent;
            this.deleteCourseworkBtn.Location = new System.Drawing.Point(314, 0);
            this.deleteCourseworkBtn.Name = "deleteCourseworkBtn";
            this.deleteCourseworkBtn.Size = new System.Drawing.Size(17, 18);
            this.deleteCourseworkBtn.TabIndex = 2;
            this.deleteCourseworkBtn.UseVisualStyleBackColor = false;
            this.deleteCourseworkBtn.Click += new System.EventHandler(this.deleteCourseworkBtn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.courseworkWeight);
            this.groupBox3.Location = new System.Drawing.Point(158, 24);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(73, 55);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Weight %";
            // 
            // courseworkWeight
            // 
            this.courseworkWeight.Location = new System.Drawing.Point(6, 22);
            this.courseworkWeight.Name = "courseworkWeight";
            this.courseworkWeight.Size = new System.Drawing.Size(59, 23);
            this.courseworkWeight.TabIndex = 0;
            this.courseworkWeight.TextChanged += new System.EventHandler(this.courseworkWeight_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.courseworkName);
            this.groupBox2.Location = new System.Drawing.Point(6, 22);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(146, 56);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Coursework Name";
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
            // deleteGroup
            // 
            this.deleteGroup.Controls.Add(this.noButton);
            this.deleteGroup.Controls.Add(this.yesButton);
            this.deleteGroup.Controls.Add(this.label1);
            this.deleteGroup.Enabled = false;
            this.deleteGroup.Location = new System.Drawing.Point(3, 3);
            this.deleteGroup.Name = "deleteGroup";
            this.deleteGroup.Size = new System.Drawing.Size(331, 85);
            this.deleteGroup.TabIndex = 3;
            this.deleteGroup.TabStop = false;
            this.deleteGroup.Text = "Confirmation";
            // 
            // noButton
            // 
            this.noButton.Location = new System.Drawing.Point(166, 56);
            this.noButton.Name = "noButton";
            this.noButton.Size = new System.Drawing.Size(159, 23);
            this.noButton.TabIndex = 2;
            this.noButton.Text = "no";
            this.noButton.UseVisualStyleBackColor = true;
            this.noButton.Click += new System.EventHandler(this.noButton_Click);
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(6, 56);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(154, 23);
            this.yesButton.TabIndex = 1;
            this.yesButton.Text = "yes";
            this.yesButton.UseVisualStyleBackColor = true;
            this.yesButton.Click += new System.EventHandler(this.yesButton_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(312, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "Are you sure you want to delete this coursework?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CourseworkWeightControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.deleteGroup);
            this.Name = "CourseworkWeightControl";
            this.Size = new System.Drawing.Size(338, 91);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.deleteGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox3;
        private TextBox courseworkWeight;
        private GroupBox groupBox2;
        private Label courseworkName;
        private Button deleteCourseworkBtn;
        private GroupBox deleteGroup;
        private Label label1;
        private Button noButton;
        private Button yesButton;
        private GroupBox groupBox4;
        private Label percentageTotalGradeLabel;
    }
}
