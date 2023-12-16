namespace AddinGrades
{
    partial class ManageCourseworkWeight
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.saveWeightChangesButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tablesGroup = new System.Windows.Forms.GroupBox();
            this.tablesList = new System.Windows.Forms.CheckedListBox();
            this.tableGroup = new System.Windows.Forms.GroupBox();
            this.CreateButton = new System.Windows.Forms.Button();
            this.newTableName = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tablesGroup.SuspendLayout();
            this.tableGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.saveWeightChangesButton);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.tablesGroup);
            this.groupBox1.Controls.Add(this.tableGroup);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(666, 484);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Menu";
            // 
            // saveWeightChangesButton
            // 
            this.saveWeightChangesButton.Location = new System.Drawing.Point(289, 446);
            this.saveWeightChangesButton.Name = "saveWeightChangesButton";
            this.saveWeightChangesButton.Size = new System.Drawing.Size(372, 23);
            this.saveWeightChangesButton.TabIndex = 1;
            this.saveWeightChangesButton.Text = "Salvar pesos";
            this.saveWeightChangesButton.UseVisualStyleBackColor = true;
            this.saveWeightChangesButton.Click += new System.EventHandler(this.saveWeightChangesButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.flowLayoutPanel1);
            this.groupBox2.Location = new System.Drawing.Point(286, 22);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(375, 421);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Pesos";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 19);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(369, 399);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // tablesGroup
            // 
            this.tablesGroup.Controls.Add(this.tablesList);
            this.tablesGroup.Location = new System.Drawing.Point(6, 81);
            this.tablesGroup.Name = "tablesGroup";
            this.tablesGroup.Size = new System.Drawing.Size(274, 392);
            this.tablesGroup.TabIndex = 1;
            this.tablesGroup.TabStop = false;
            this.tablesGroup.Text = "Seleciona uma tabela";
            // 
            // tablesList
            // 
            this.tablesList.FormattingEnabled = true;
            this.tablesList.Location = new System.Drawing.Point(6, 22);
            this.tablesList.Name = "tablesList";
            this.tablesList.Size = new System.Drawing.Size(262, 364);
            this.tablesList.TabIndex = 0;
            this.tablesList.SelectedIndexChanged += new System.EventHandler(this.tablesList_SelectedIndexChanged);
            // 
            // tableGroup
            // 
            this.tableGroup.Controls.Add(this.CreateButton);
            this.tableGroup.Controls.Add(this.newTableName);
            this.tableGroup.Location = new System.Drawing.Point(6, 22);
            this.tableGroup.Name = "tableGroup";
            this.tableGroup.Size = new System.Drawing.Size(274, 53);
            this.tableGroup.TabIndex = 0;
            this.tableGroup.TabStop = false;
            this.tableGroup.Text = "Criar nova tabela de pesos";
            // 
            // CreateButton
            // 
            this.CreateButton.Location = new System.Drawing.Point(193, 22);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(75, 23);
            this.CreateButton.TabIndex = 1;
            this.CreateButton.Text = "Criar";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // newTableName
            // 
            this.newTableName.Location = new System.Drawing.Point(6, 22);
            this.newTableName.Name = "newTableName";
            this.newTableName.Size = new System.Drawing.Size(181, 23);
            this.newTableName.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1500;
            this.timer1.Tick += new System.EventHandler(this.OnTimerTick);
            // 
            // ManageCourseworkWeight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 506);
            this.Controls.Add(this.groupBox1);
            this.Name = "ManageCourseworkWeight";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Editar momentos de avaliação";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ManageCourseworkWeight_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tablesGroup.ResumeLayout(false);
            this.tableGroup.ResumeLayout(false);
            this.tableGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox tablesGroup;
        private CheckedListBox tablesList;
        private GroupBox tableGroup;
        private Button CreateButton;
        private TextBox newTableName;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button saveWeightChangesButton;
        private System.Windows.Forms.Timer timer1;
    }
}