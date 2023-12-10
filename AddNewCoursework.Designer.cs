namespace AddinGrades
{
    partial class AddNewCoursework
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
            button1 = new Button();
            groupBox3 = new GroupBox();
            courseworkWeightTxt = new TextBox();
            groupBox2 = new GroupBox();
            courseworkNameTxt = new TextBox();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(331, 125);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Adicionar um novo momento de avaliação";
            // 
            // button1
            // 
            button1.BackColor = Color.White;
            button1.BackgroundImageLayout = ImageLayout.None;
            button1.ForeColor = SystemColors.ActiveCaptionText;
            button1.ImageAlign = ContentAlignment.TopLeft;
            button1.Location = new Point(12, 85);
            button1.Name = "button1";
            button1.Size = new Size(307, 33);
            button1.TabIndex = 2;
            button1.Text = "Criar";
            button1.TextImageRelation = TextImageRelation.ImageBeforeText;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(courseworkWeightTxt);
            groupBox3.Location = new Point(201, 22);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(124, 57);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "Peso %";
            // 
            // courseworkWeightTxt
            // 
            courseworkWeightTxt.Location = new Point(7, 22);
            courseworkWeightTxt.Name = "courseworkWeightTxt";
            courseworkWeightTxt.Size = new Size(111, 23);
            courseworkWeightTxt.TabIndex = 1;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(courseworkNameTxt);
            groupBox2.Location = new Point(6, 22);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(189, 57);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Nome";
            // 
            // courseworkNameTxt
            // 
            courseworkNameTxt.Location = new Point(6, 22);
            courseworkNameTxt.Name = "courseworkNameTxt";
            courseworkNameTxt.Size = new Size(177, 23);
            courseworkNameTxt.TabIndex = 1;
            // 
            // AddNewCoursework
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox1);
            Name = "AddNewCoursework";
            Size = new Size(337, 131);
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
        private GroupBox groupBox2;
        private Button button1;
        private TextBox courseworkWeightTxt;
        private TextBox courseworkNameTxt;
    }
}
