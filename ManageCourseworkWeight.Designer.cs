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
            components = new System.ComponentModel.Container();
            groupBox1 = new GroupBox();
            saveWeightChangesButton = new Button();
            groupBox2 = new GroupBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            tablesGroup = new GroupBox();
            tablesList = new CheckedListBox();
            tableGroup = new GroupBox();
            CreateButton = new Button();
            newTableName = new TextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            tablesGroup.SuspendLayout();
            tableGroup.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(saveWeightChangesButton);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Controls.Add(tablesGroup);
            groupBox1.Controls.Add(tableGroup);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(666, 484);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Menu";
            // 
            // saveWeightChangesButton
            // 
            saveWeightChangesButton.Location = new Point(289, 446);
            saveWeightChangesButton.Name = "saveWeightChangesButton";
            saveWeightChangesButton.Size = new Size(372, 23);
            saveWeightChangesButton.TabIndex = 1;
            saveWeightChangesButton.Text = "Salvar pesos";
            saveWeightChangesButton.UseVisualStyleBackColor = true;
            saveWeightChangesButton.Click += saveWeightChangesButton_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(flowLayoutPanel1);
            groupBox2.Location = new Point(286, 22);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(375, 421);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Pesos";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(3, 19);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(369, 399);
            flowLayoutPanel1.TabIndex = 0;
            flowLayoutPanel1.WrapContents = false;
            // 
            // tablesGroup
            // 
            tablesGroup.Controls.Add(tablesList);
            tablesGroup.Location = new Point(6, 81);
            tablesGroup.Name = "tablesGroup";
            tablesGroup.Size = new Size(274, 392);
            tablesGroup.TabIndex = 1;
            tablesGroup.TabStop = false;
            tablesGroup.Text = "Seleciona uma tabela";
            tablesGroup.Enter += tablesGroup_Enter;
            // 
            // tablesList
            // 
            tablesList.FormattingEnabled = true;
            tablesList.Location = new Point(6, 22);
            tablesList.Name = "tablesList";
            tablesList.Size = new Size(262, 364);
            tablesList.TabIndex = 0;
            tablesList.SelectedIndexChanged += tablesList_SelectedIndexChanged;
            // 
            // tableGroup
            // 
            tableGroup.Controls.Add(CreateButton);
            tableGroup.Controls.Add(newTableName);
            tableGroup.Location = new Point(6, 22);
            tableGroup.Name = "tableGroup";
            tableGroup.Size = new Size(274, 53);
            tableGroup.TabIndex = 0;
            tableGroup.TabStop = false;
            tableGroup.Text = "Criar nova tabela de pesos";
            // 
            // CreateButton
            // 
            CreateButton.Location = new Point(193, 22);
            CreateButton.Name = "CreateButton";
            CreateButton.Size = new Size(75, 23);
            CreateButton.TabIndex = 1;
            CreateButton.Text = "Criar";
            CreateButton.UseVisualStyleBackColor = true;
            CreateButton.Click += CreateButton_Click;
            // 
            // newTableName
            // 
            newTableName.Location = new Point(6, 22);
            newTableName.Name = "newTableName";
            newTableName.Size = new Size(181, 23);
            newTableName.TabIndex = 0;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1500;
            timer1.Tick += OnTimerTick;
            // 
            // ManageCourseworkWeight
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(688, 506);
            Controls.Add(groupBox1);
            Name = "ManageCourseworkWeight";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Editar momentos de avaliação";
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            tablesGroup.ResumeLayout(false);
            tableGroup.ResumeLayout(false);
            tableGroup.PerformLayout();
            ResumeLayout(false);
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