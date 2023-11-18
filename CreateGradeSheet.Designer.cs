namespace AddinGrades
{
    partial class CreateGradeSheet
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
            this.groupClassPicker = new System.Windows.Forms.GroupBox();
            this.classesDropDown = new System.Windows.Forms.ComboBox();
            this.classesLbl = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.passwordTxt = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.emailTxt = new System.Windows.Forms.TextBox();
            this.createGradeSheetButton = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.numberOfSheetsComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupClassPicker.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupClassPicker);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(385, 224);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Class details";
            // 
            // groupClassPicker
            // 
            this.groupClassPicker.Controls.Add(this.classesDropDown);
            this.groupClassPicker.Controls.Add(this.classesLbl);
            this.groupClassPicker.Enabled = false;
            this.groupClassPicker.Location = new System.Drawing.Point(6, 134);
            this.groupClassPicker.Name = "groupClassPicker";
            this.groupClassPicker.Size = new System.Drawing.Size(367, 82);
            this.groupClassPicker.TabIndex = 5;
            this.groupClassPicker.TabStop = false;
            this.groupClassPicker.Text = "Get names of students (LOGIN REQUIRED)";
            // 
            // classesDropDown
            // 
            this.classesDropDown.FormattingEnabled = true;
            this.classesDropDown.Location = new System.Drawing.Point(12, 43);
            this.classesDropDown.Name = "classesDropDown";
            this.classesDropDown.Size = new System.Drawing.Size(340, 23);
            this.classesDropDown.TabIndex = 0;
            // 
            // classesLbl
            // 
            this.classesLbl.AutoSize = true;
            this.classesLbl.Location = new System.Drawing.Point(12, 25);
            this.classesLbl.Name = "classesLbl";
            this.classesLbl.Size = new System.Drawing.Size(69, 15);
            this.classesLbl.TabIndex = 2;
            this.classesLbl.Text = "Select class:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.loginButton);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(6, 22);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(367, 106);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Login for es.jobra.pt";
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(143, 78);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 2;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.passwordTxt);
            this.groupBox4.Location = new System.Drawing.Point(185, 22);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(173, 50);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Password";
            // 
            // passwordTxt
            // 
            this.passwordTxt.Location = new System.Drawing.Point(6, 21);
            this.passwordTxt.Name = "passwordTxt";
            this.passwordTxt.PasswordChar = '*';
            this.passwordTxt.Size = new System.Drawing.Size(161, 23);
            this.passwordTxt.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.emailTxt);
            this.groupBox3.Location = new System.Drawing.Point(6, 22);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(173, 50);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Email";
            // 
            // emailTxt
            // 
            this.emailTxt.Location = new System.Drawing.Point(6, 21);
            this.emailTxt.Name = "emailTxt";
            this.emailTxt.Size = new System.Drawing.Size(161, 23);
            this.emailTxt.TabIndex = 0;
            this.emailTxt.Text = "nunopinho1@gmail.com";
            // 
            // createGradeSheetButton
            // 
            this.createGradeSheetButton.Location = new System.Drawing.Point(12, 242);
            this.createGradeSheetButton.Name = "createGradeSheetButton";
            this.createGradeSheetButton.Size = new System.Drawing.Size(238, 39);
            this.createGradeSheetButton.TabIndex = 1;
            this.createGradeSheetButton.Text = "Create gradesheet";
            this.createGradeSheetButton.UseVisualStyleBackColor = true;
            this.createGradeSheetButton.Click += new System.EventHandler(this.createGradeSheetButton_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.numberOfSheetsComboBox);
            this.groupBox5.Location = new System.Drawing.Point(256, 242);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(141, 42);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Number of sheets";
            // 
            // numberOfSheetsComboBox
            // 
            this.numberOfSheetsComboBox.FormattingEnabled = true;
            this.numberOfSheetsComboBox.Location = new System.Drawing.Point(8, 16);
            this.numberOfSheetsComboBox.Name = "numberOfSheetsComboBox";
            this.numberOfSheetsComboBox.Size = new System.Drawing.Size(121, 23);
            this.numberOfSheetsComboBox.TabIndex = 0;
            this.numberOfSheetsComboBox.Text = "3";
            // 
            // CreateGradeSheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 289);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.createGradeSheetButton);
            this.Controls.Add(this.groupBox1);
            this.MaximumSize = new System.Drawing.Size(425, 328);
            this.MinimumSize = new System.Drawing.Size(425, 328);
            this.Name = "CreateGradeSheet";
            this.Text = "Grasheet Maker";
            this.groupBox1.ResumeLayout(false);
            this.groupClassPicker.ResumeLayout(false);
            this.groupClassPicker.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button loginButton;
        private GroupBox groupBox4;
        public TextBox passwordTxt;
        private GroupBox groupBox3;
        private TextBox emailTxt;
        private GroupBox groupClassPicker;
        private ComboBox classesDropDown;
        private Label classesLbl;
        private Button createGradeSheetButton;
        private GroupBox groupBox5;
        private ComboBox numberOfSheetsComboBox;
    }
}