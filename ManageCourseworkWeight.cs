﻿using AddinGrades.DTO;

namespace AddinGrades
{
    public partial class ManageCourseworkWeight : Form
    {
        public readonly string GradeSheetID;
        public EventHandler<string> CourseworkDeleteEvent;
        public EventHandler<(string, double)> CourseworkAddEvent;

        public ManageCourseworkWeight(string gradeSheetID)
        {
            GradeSheetID = gradeSheetID;
            InitializeComponent();
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
            data.GradeSheets[GradeSheetID].CourseworkWeightedTables.ForEach(s => tablesList.Items.Add(s.name));
            CourseworkDeleteEvent += new EventHandler<string>(OnCourseworkDelete);
            CourseworkAddEvent += new EventHandler<(string, double)>(OnCourseworkAdd);
            if(tablesList.Items.Count >0)
             tablesList.SelectedIndex= 0;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
            if (string.IsNullOrWhiteSpace(newTableName.Text) == false && data.GradeSheets[GradeSheetID].CourseworkWeightedTables.Any(s => s.name.Equals(newTableName.Text)) == false)
            {
                data.GradeSheets[GradeSheetID].CourseworkWeightedTables.Add(
                new CourseworkWeightedTable(newTableName.Text, data.GradeSheets[GradeSheetID].Coursework,
                Enumerable.Repeat(element: 0d, data.GradeSheets[GradeSheetID].Coursework.Count).ToArray()));
                data.Save();
                this.tablesList.Items.Add(newTableName.Text);
                GradeTable table = new GradeTable(GradeSheetID);
                table.InsertDropdownForWeightedTable();
                table.InsertKnowledgeFunctionForRows(Utils.GetWorksheetById(GradeSheetID));
            }
        }

        private void tablesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshListOfCoursework();
        }

        private void RefreshListOfCoursework()
        {
            if (string.IsNullOrEmpty((string)tablesList.SelectedItem) is false)
            {
                WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
                var courseworkAndWeight = data.GradeSheets[GradeSheetID].CourseworkWeightedTables.Find(s => s.name == ((string)tablesList.SelectedItem)).weights;
                flowLayoutPanel1.Controls.Clear();
                foreach (var keyvaluepair in courseworkAndWeight)
                {
                    flowLayoutPanel1.Controls.Add(
                        new CourseworkWeightControl(keyvaluepair.Key.Name, keyvaluepair.Value * 100, (string)tablesList.SelectedItem, CourseworkDeleteEvent));
                } 
                flowLayoutPanel1.Controls.Add(new AddNewCoursework(GradeSheetID, CourseworkAddEvent));
            }
        }

        public void OnCourseworkAdd(object? sender, (string, double) pair)
        {
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
            Coursework newCoursework;
            data.GradeSheets[GradeSheetID].Coursework.Add(newCoursework = new Coursework(pair.Item1));
            data.GradeSheets[GradeSheetID].CourseworkWeightedTables.ForEach(s => s.AddCoursework(newCoursework, pair.Item2 / 100));
            data.Save();
            GradeTable table = new(GradeSheetID);
            table.InsertNewCoursework(newCoursework);
            table.InsertDropdownForWeightedTable();
            table.InsertKnowledgeFunctionForRows(Utils.GetWorksheetById(GradeSheetID));
            table.LockCollumnsAndHeaders();
            RefreshListOfCoursework();
            table.ApplyStyles();
            //For some reason chrome driver makes it so you have to bind this event again 
            Utils.GetExcelApplication().ActiveWorkbook.SheetChange += Program.OnSheetChange;
        }

        public void OnCourseworkDelete(object? sender, string courseworkName)
        {
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
            var gradeTable = new GradeTable(GradeSheetID);
            var gradeSheet = data.GradeSheets[GradeSheetID];
            gradeSheet.CourseworkWeightedTables
                    .ForEach(s => s.weights.Remove(gradeSheet.GetCoursework(courseworkName).Object));
            gradeSheet.Coursework.RemoveAll(s => s.Name == courseworkName);
            gradeTable.DeleteCourseworkCollumn(courseworkName);
            data.Save();
            flowLayoutPanel1.Controls.Remove(sender as Control);
            GradeTable table = new(GradeSheetID);
            table.InsertDropdownForWeightedTable();
            table.InsertKnowledgeFunctionForRows(Utils.GetWorksheetById(GradeSheetID));
            table.LockCollumnsAndHeaders();
            table.ApplyStyles();
            //For some reason chrome driver makes it so you have to bind this event again 
            Utils.GetExcelApplication().ActiveWorkbook.SheetChange += Program.OnSheetChange;
        }

        private void tablesGroup_Enter(object sender, EventArgs e)
        {

        }

        private void saveWeightChangesButton_Click(object sender, EventArgs e)
        {
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();

            double totalWeigths = 0d;
            foreach (Control item in flowLayoutPanel1.Controls)
            {
                if (item is CourseworkWeightControl courseworkWeight)
                {
                    if (double.TryParse(courseworkWeight.GetWeight(), out double parsedWeight))
                    {
                        totalWeigths += parsedWeight;
                    }
                    else
                    {
                        MessageBox.Show($"Please reconsider the weight for: {courseworkWeight.GetCourseworkName()}");
                        return;
                    }
                }
            }

            if (totalWeigths != 100)
            {
                MessageBox.Show($"The total weight percentage should be 100%");
                return;
            }


            foreach (Control item in flowLayoutPanel1.Controls)
            {
                if (item is CourseworkWeightControl courseworkWeight)
                {
                    if (double.TryParse(courseworkWeight.GetWeight(), out double parsedWeight))
                    {
                        data.GradeSheets[GradeSheetID].GetWeightedTable((string)tablesList.SelectedItem).Object
                        .ChangeWeight(data.GradeSheets[GradeSheetID].GetCoursework(courseworkWeight.GetCourseworkName()).Object,
                        parsedWeight / 100);
                    }
                    else
                    {
                        MessageBox.Show($"Please reconsider the weight for: {courseworkWeight.GetCourseworkName()}");
                        return;
                    }
                }
            }
            data.Save();
            GradeTable table = new(GradeSheetID);
            table.InsertKnowledgeFunctionForRows(Utils.GetWorksheetById(GradeSheetID));
            Utils.GetExcelApplication().CalculateFull();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            double total = 0d;
            foreach (Control item in flowLayoutPanel1.Controls)
            {
                if (item is CourseworkWeightControl courseworkWeight)
                {
                    if (double.TryParse(courseworkWeight.GetWeight(), out double parsedValue))
                        total += parsedValue;
                }

            }
            groupBox2.Text = $"Total: {total}/100  Unassigned: {100 - total}";
        }

        private void ManageCourseworkWeight_FormClosing(object sender, FormClosingEventArgs e)
        {
            Utils.GetExcelApplication().CalculateFull();
        }
    }
}
