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
    public partial class AddCoursework : Form
    {
         readonly string GradeSheetID;

        public AddCoursework(string gradeSheetID)
        {
            InitializeComponent();
            this.GradeSheetID = gradeSheetID;
            this.courseworkList.Items.AddRange(Utils.GetExcelApplication().LoadWorkbookData().GradeSheets[GradeSheetID].Coursework.Select(s=>s.Name).ToArray());
        }

        private void AddCourseworkClick(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(courseworkInput.Text)) return;

            if ( Utils.GetExcelApplication().LoadWorkbookData().GradeSheets[GradeSheetID].Coursework.Any(s=>s.Name == courseworkInput.Text))
            {
                MessageBox.Show("You already have a coursework with this name");
                return;
            }

            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
            Coursework newCoursework;
            data.GradeSheets[GradeSheetID].Coursework.Add(newCoursework = new Coursework(courseworkInput.Text));
            data.GradeSheets[GradeSheetID].CourseworkWeightedTables.ForEach(s => s.AddCoursework(newCoursework, 0));
            data.Save(); 
            courseworkInput.Text = string.Empty;
            this.courseworkList.Items.Clear();
            this.courseworkList.Items.AddRange(data.GradeSheets[GradeSheetID].Coursework.Select(s => s.Name).ToArray());
            GradeTable table = new(GradeSheetID);
            table.InsertNewCoursework(newCoursework);
            table.InsertDropdownForWeightedTable();
            table.InsertKnowledgeFunctionForRows();
        }

        private void removeCoursework_Click(object sender, EventArgs e)
        {
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
            var gradeTable = new GradeTable(GradeSheetID);
            var gradeSheet = data.GradeSheets[GradeSheetID];
            foreach (string toRemove in courseworkList.CheckedItems)
            {
                gradeSheet.Coursework.RemoveAll(s => s.Name == toRemove);
                gradeSheet.CourseworkWeightedTables
                    .ForEach(s => s.weights.Remove(gradeSheet.GetCoursework(toRemove).Object));
                gradeTable.DeleteCourseworkCollumn(toRemove);
            }
            data.Save();
            this.courseworkList.Items.Clear();
            this.courseworkList.Items.AddRange(gradeSheet.Coursework.Select(s => s.Name).ToArray());
            GradeTable table = new(GradeSheetID);
            table.InsertDropdownForWeightedTable();
            table.InsertKnowledgeFunctionForRows();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
