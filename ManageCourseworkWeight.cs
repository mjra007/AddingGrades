using AddinGrades.DTO;
using Microsoft.VisualStudio.Services.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddinGrades
{
    public partial class ManageCourseworkWeight : Form
    {
        public readonly string GradeSheetID;

        public ManageCourseworkWeight(string gradeSheetID)
        {
            GradeSheetID = gradeSheetID;
            InitializeComponent();
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
            data.GradeSheets[GradeSheetID].CourseworkWeightedTables.ForEach(s => tablesList.Items.Add(s.name));
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
                table.InsertKnowledgeFunctionForRows();
            }
        }

        private void tablesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
            var courseworkAndWeight = data.GradeSheets[GradeSheetID].CourseworkWeightedTables.Find(s => s.name == ((string)tablesList.SelectedItem)).weights;
            flowLayoutPanel1.Controls.Clear();
            foreach (var keyvaluepair in courseworkAndWeight)
            {
                flowLayoutPanel1.Controls.Add(
                    new CourseworkWeightControl(keyvaluepair.Key.Name, keyvaluepair.Value*100, (string)tablesList.SelectedItem));
            }
        }

        private void tablesGroup_Enter(object sender, EventArgs e)
        {

        }

        private void saveWeightChangesButton_Click(object sender, EventArgs e)
        {
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
            foreach (CourseworkWeightControl item in flowLayoutPanel1.Controls.Cast<CourseworkWeightControl>())
            {
                data.GradeSheets[GradeSheetID].GetWeightedTable((string)tablesList.SelectedItem).Object
                    .ChangeWeight(data.GradeSheets[GradeSheetID].GetCoursework(item.GetCourseworkName()).Object,
                    double.Parse(item.GetWeight()) / 100);
            }
            data.Save();
            GradeTable table = new(GradeSheetID);
            table.InsertKnowledgeFunctionForRows();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            double total = 0d;
            foreach (CourseworkWeightControl item in flowLayoutPanel1.Controls.Cast<CourseworkWeightControl>())
            {
                if(double.TryParse(item.GetWeight(),out double parsedValue ))
                    total += parsedValue;
            }

            groupBox2.Text = $"Total weights: {total}/100";
        }

    }
}
