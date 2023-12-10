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
            groupBox4 = new GroupBox();
            percentageTotalGradeLabel = new Label();
            deleteCourseworkBtn = new Button();
            groupBox3 = new GroupBox();
            courseworkWeight = new TextBox();
            groupBox2 = new GroupBox();
            courseworkName = new Label();
            deleteGroup = new GroupBox();
            noButton = new Button();
            yesButton = new Button();
            label1 = new Label();
            groupBox1.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            deleteGroup.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupBox4);
            groupBox1.Controls.Add(deleteCourseworkBtn);
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(331, 83);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Peso";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(percentageTotalGradeLabel);
            groupBox4.Location = new Point(237, 22);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(94, 55);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "% Nota Total";
            // 
            // percentageTotalGradeLabel
            // 
            percentageTotalGradeLabel.AutoSize = true;
            percentageTotalGradeLabel.Location = new Point(15, 22);
            percentageTotalGradeLabel.Name = "percentageTotalGradeLabel";
            percentageTotalGradeLabel.Size = new Size(0, 15);
            percentageTotalGradeLabel.TabIndex = 0;
            // 
            // deleteCourseworkBtn
            // 
            deleteCourseworkBtn.BackColor = Color.Transparent;
            deleteCourseworkBtn.BackgroundImage = Properties.Resources.close;
            deleteCourseworkBtn.BackgroundImageLayout = ImageLayout.Stretch;
            deleteCourseworkBtn.FlatAppearance.BorderSize = 0;
            deleteCourseworkBtn.FlatStyle = FlatStyle.Flat;
            deleteCourseworkBtn.ForeColor = Color.Transparent;
            deleteCourseworkBtn.Location = new Point(314, 0);
            deleteCourseworkBtn.Name = "deleteCourseworkBtn";
            deleteCourseworkBtn.Size = new Size(17, 18);
            deleteCourseworkBtn.TabIndex = 2;
            deleteCourseworkBtn.UseVisualStyleBackColor = false;
            deleteCourseworkBtn.Click += deleteCourseworkBtn_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(courseworkWeight);
            groupBox3.Location = new Point(158, 22);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(73, 57);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "Peso %";
            // 
            // courseworkWeight
            // 
            courseworkWeight.Location = new Point(6, 22);
            courseworkWeight.Name = "courseworkWeight";
            courseworkWeight.Size = new Size(59, 23);
            courseworkWeight.TabIndex = 0;
            courseworkWeight.TextChanged += courseworkWeight_TextChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(courseworkName);
            groupBox2.Location = new Point(6, 22);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(146, 56);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Nome";
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
            // deleteGroup
            // 
            deleteGroup.Controls.Add(noButton);
            deleteGroup.Controls.Add(yesButton);
            deleteGroup.Controls.Add(label1);
            deleteGroup.Enabled = false;
            deleteGroup.Location = new Point(3, 3);
            deleteGroup.Name = "deleteGroup";
            deleteGroup.Size = new Size(331, 85);
            deleteGroup.TabIndex = 3;
            deleteGroup.TabStop = false;
            deleteGroup.Text = "Confirmation";
            // 
            // noButton
            // 
            noButton.Location = new Point(166, 56);
            noButton.Name = "noButton";
            noButton.Size = new Size(159, 23);
            noButton.TabIndex = 2;
            noButton.Text = "no";
            noButton.UseVisualStyleBackColor = true;
            noButton.Click += noButton_Click;
            // 
            // yesButton
            // 
            yesButton.Location = new Point(6, 56);
            yesButton.Name = "yesButton";
            yesButton.Size = new Size(154, 23);
            yesButton.TabIndex = 1;
            yesButton.Text = "yes";
            yesButton.UseVisualStyleBackColor = true;
            yesButton.Click += yesButton_Click;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(9, 16);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(312, 45);
            label1.TabIndex = 0;
            label1.Text = "Are you sure you want to delete this coursework?";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CourseworkWeightControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox1);
            Controls.Add(deleteGroup);
            Name = "CourseworkWeightControl";
            Size = new Size(338, 91);
            groupBox1.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            deleteGroup.ResumeLayout(false);
            ResumeLayout(false);
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
