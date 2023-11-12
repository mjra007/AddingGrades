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
            groupBox1 = new GroupBox();
            groupClassPicker = new GroupBox();
            classesDropDown = new ComboBox();
            classesLbl = new Label();
            groupBox2 = new GroupBox();
            loginButton = new Button();
            groupBox4 = new GroupBox();
            passwordTxt = new TextBox();
            groupBox3 = new GroupBox();
            emailTxt = new TextBox();
            createGradeSheetButton = new Button();
            groupBox1.SuspendLayout();
            groupClassPicker.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupClassPicker);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(385, 224);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Class details";
            // 
            // groupClassPicker
            // 
            groupClassPicker.Controls.Add(classesDropDown);
            groupClassPicker.Controls.Add(classesLbl);
            groupClassPicker.Enabled = false;
            groupClassPicker.Location = new Point(6, 134);
            groupClassPicker.Name = "groupClassPicker";
            groupClassPicker.Size = new Size(367, 82);
            groupClassPicker.TabIndex = 5;
            groupClassPicker.TabStop = false;
            groupClassPicker.Text = "Get names of students (LOGIN REQUIRED)";
            // 
            // classesDropDown
            // 
            classesDropDown.FormattingEnabled = true;
            classesDropDown.Location = new Point(12, 43);
            classesDropDown.Name = "classesDropDown";
            classesDropDown.Size = new Size(340, 23);
            classesDropDown.TabIndex = 0;
            // 
            // classesLbl
            // 
            classesLbl.AutoSize = true;
            classesLbl.Location = new Point(12, 25);
            classesLbl.Name = "classesLbl";
            classesLbl.Size = new Size(69, 15);
            classesLbl.TabIndex = 2;
            classesLbl.Text = "Select class:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(loginButton);
            groupBox2.Controls.Add(groupBox4);
            groupBox2.Controls.Add(groupBox3);
            groupBox2.Location = new Point(6, 22);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(367, 106);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Login for es.jobra.pt";
            // 
            // loginButton
            // 
            loginButton.Location = new Point(143, 78);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(75, 23);
            loginButton.TabIndex = 2;
            loginButton.Text = "Login";
            loginButton.UseVisualStyleBackColor = true;
            loginButton.Click += loginButton_Click;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(passwordTxt);
            groupBox4.Location = new Point(185, 22);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(173, 50);
            groupBox4.TabIndex = 1;
            groupBox4.TabStop = false;
            groupBox4.Text = "Password";
            // 
            // passwordTxt
            // 
            passwordTxt.Location = new Point(6, 21);
            passwordTxt.Name = "passwordTxt";
            passwordTxt.PasswordChar = '*';
            passwordTxt.Size = new Size(161, 23);
            passwordTxt.TabIndex = 1;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(emailTxt);
            groupBox3.Location = new Point(6, 22);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(173, 50);
            groupBox3.TabIndex = 0;
            groupBox3.TabStop = false;
            groupBox3.Text = "Email";
            // 
            // emailTxt
            // 
            emailTxt.Location = new Point(6, 21);
            emailTxt.Name = "emailTxt";
            emailTxt.Size = new Size(161, 23);
            emailTxt.TabIndex = 0;
            emailTxt.Text = "nunopinho1@gmail.com";
            // 
            // createGradeSheetButton
            // 
            createGradeSheetButton.Location = new Point(12, 242);
            createGradeSheetButton.Name = "createGradeSheetButton";
            createGradeSheetButton.Size = new Size(385, 39);
            createGradeSheetButton.TabIndex = 1;
            createGradeSheetButton.Text = "Create gradesheet";
            createGradeSheetButton.UseVisualStyleBackColor = true;
            createGradeSheetButton.Click += createGradeSheetButton_Click;
            // 
            // CreateGradeSheet
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(409, 289);
            Controls.Add(createGradeSheetButton);
            Controls.Add(groupBox1);
            MaximumSize = new Size(425, 328);
            MinimumSize = new Size(425, 328);
            Name = "CreateGradeSheet";
            Text = "Grasheet Maker";
            groupBox1.ResumeLayout(false);
            groupClassPicker.ResumeLayout(false);
            groupClassPicker.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
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
    }
}