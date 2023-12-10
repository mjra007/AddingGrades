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
            groupBox7 = new GroupBox();
            cacheData = new Label();
            button1 = new Button();
            label1 = new Label();
            groupClassPicker = new GroupBox();
            groupBox5 = new GroupBox();
            numberOfSheetsComboBox = new ComboBox();
            classesDropDown = new ComboBox();
            createGradeSheetButton = new Button();
            classesLbl = new Label();
            groupBox6 = new GroupBox();
            returnButton = new Button();
            groupBox2 = new GroupBox();
            progressBar1 = new ProgressBar();
            loginButton = new Button();
            groupBox4 = new GroupBox();
            passwordTxt = new TextBox();
            groupBox3 = new GroupBox();
            emailTxt = new TextBox();
            groupBox1.SuspendLayout();
            groupBox7.SuspendLayout();
            groupClassPicker.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupBox7);
            groupBox1.Controls.Add(groupClassPicker);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(385, 269);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Detalhes";
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(cacheData);
            groupBox7.Controls.Add(button1);
            groupBox7.Controls.Add(label1);
            groupBox7.Location = new Point(9, 163);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(364, 100);
            groupBox7.TabIndex = 6;
            groupBox7.TabStop = false;
            groupBox7.Text = "Definições de cache";
            // 
            // cacheData
            // 
            cacheData.AutoSize = true;
            cacheData.Location = new Point(12, 48);
            cacheData.Name = "cacheData";
            cacheData.Size = new Size(0, 15);
            cacheData.TabIndex = 2;
            // 
            // button1
            // 
            button1.Location = new Point(223, 19);
            button1.Name = "button1";
            button1.Size = new Size(129, 72);
            button1.TabIndex = 1;
            button1.Text = "Atualizar cache";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 28);
            label1.Name = "label1";
            label1.Size = new Size(66, 15);
            label1.TabIndex = 0;
            label1.Text = "Atualizado:";
            // 
            // groupClassPicker
            // 
            groupClassPicker.Controls.Add(groupBox5);
            groupClassPicker.Controls.Add(classesDropDown);
            groupClassPicker.Controls.Add(createGradeSheetButton);
            groupClassPicker.Controls.Add(classesLbl);
            groupClassPicker.Enabled = false;
            groupClassPicker.Location = new Point(9, 22);
            groupClassPicker.Name = "groupClassPicker";
            groupClassPicker.Size = new Size(367, 135);
            groupClassPicker.TabIndex = 5;
            groupClassPicker.TabStop = false;
            groupClassPicker.Text = "Exportar nome de alunos";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(numberOfSheetsComboBox);
            groupBox5.Location = new Point(212, 72);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(149, 47);
            groupBox5.TabIndex = 2;
            groupBox5.TabStop = false;
            groupBox5.Text = "Numero de folhas";
            // 
            // numberOfSheetsComboBox
            // 
            numberOfSheetsComboBox.FormattingEnabled = true;
            numberOfSheetsComboBox.Location = new Point(8, 16);
            numberOfSheetsComboBox.Name = "numberOfSheetsComboBox";
            numberOfSheetsComboBox.Size = new Size(129, 23);
            numberOfSheetsComboBox.TabIndex = 0;
            numberOfSheetsComboBox.Text = "3";
            // 
            // classesDropDown
            // 
            classesDropDown.FormattingEnabled = true;
            classesDropDown.ImeMode = ImeMode.NoControl;
            classesDropDown.Location = new Point(12, 43);
            classesDropDown.Name = "classesDropDown";
            classesDropDown.Size = new Size(340, 23);
            classesDropDown.TabIndex = 0;
            // 
            // createGradeSheetButton
            // 
            createGradeSheetButton.Location = new Point(12, 72);
            createGradeSheetButton.Name = "createGradeSheetButton";
            createGradeSheetButton.Size = new Size(194, 47);
            createGradeSheetButton.TabIndex = 1;
            createGradeSheetButton.Text = "Criar folha de notas";
            createGradeSheetButton.UseVisualStyleBackColor = true;
            createGradeSheetButton.Click += createGradeSheetButton_Click;
            // 
            // classesLbl
            // 
            classesLbl.AutoSize = true;
            classesLbl.Location = new Point(12, 25);
            classesLbl.Name = "classesLbl";
            classesLbl.Size = new Size(122, 15);
            classesLbl.TabIndex = 2;
            classesLbl.Text = "Seleciona uma turma:";
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(returnButton);
            groupBox6.Controls.Add(groupBox2);
            groupBox6.Location = new Point(12, 12);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(385, 269);
            groupBox6.TabIndex = 3;
            groupBox6.TabStop = false;
            groupBox6.Text = "Cache";
            // 
            // returnButton
            // 
            returnButton.BackColor = Color.Transparent;
            returnButton.BackgroundImage = Properties.Resources.previous;
            returnButton.BackgroundImageLayout = ImageLayout.Stretch;
            returnButton.FlatAppearance.BorderSize = 0;
            returnButton.FlatStyle = FlatStyle.Flat;
            returnButton.Location = new Point(347, 0);
            returnButton.Name = "returnButton";
            returnButton.Size = new Size(32, 32);
            returnButton.TabIndex = 3;
            returnButton.Text = " ";
            returnButton.UseVisualStyleBackColor = false;
            returnButton.Click += returnButton_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(progressBar1);
            groupBox2.Controls.Add(loginButton);
            groupBox2.Controls.Add(groupBox4);
            groupBox2.Controls.Add(groupBox3);
            groupBox2.Location = new Point(6, 22);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(367, 241);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Login for es.jobra.pt";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(6, 196);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(355, 36);
            progressBar1.TabIndex = 3;
            // 
            // loginButton
            // 
            loginButton.Location = new Point(6, 84);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(355, 102);
            loginButton.TabIndex = 2;
            loginButton.Text = "Cache";
            loginButton.UseVisualStyleBackColor = true;
            loginButton.Click += loginButton_Click_1;
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
            // CreateGradeSheet
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(409, 289);
            Controls.Add(groupBox1);
            Controls.Add(groupBox6);
            MaximumSize = new Size(425, 328);
            MinimumSize = new Size(425, 328);
            Name = "CreateGradeSheet";
            Text = "Grasheet Maker";
            groupBox1.ResumeLayout(false);
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            groupClassPicker.ResumeLayout(false);
            groupClassPicker.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupClassPicker;
        private ComboBox classesDropDown;
        private Label classesLbl;
        private Button createGradeSheetButton;
        private GroupBox groupBox5;
        private ComboBox numberOfSheetsComboBox;
        private GroupBox groupBox6;
        private GroupBox groupBox2;
        private Button loginButton;
        private GroupBox groupBox4;
        public TextBox passwordTxt;
        private GroupBox groupBox3;
        private TextBox emailTxt;
        private GroupBox groupBox7;
        private Label cacheData;
        private Button button1;
        private Label label1;
        private Button returnButton;
        private ProgressBar progressBar1;
    }
}