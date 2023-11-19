using AddinGrades.DTO;
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
    public partial class AddNewCoursework : UserControl
    {
        public EventHandler<(string, double)> CourseworkAddEvent;
        public readonly string gradeSheetID;
        public AddNewCoursework(string gradeSheetID, EventHandler<(string, double)> courseworkAddEvent)
        {
            InitializeComponent();
            this.gradeSheetID = gradeSheetID;
            this.CourseworkAddEvent = courseworkAddEvent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(courseworkNameTxt.Text)) return;

            if (Utils.GetExcelApplication().LoadWorkbookData().GradeSheets[gradeSheetID].Coursework.Any(s => s.Name == courseworkNameTxt.Text))
            {
                MessageBox.Show("You already have a coursework with this name");
                return;
            }
            if (double.TryParse(courseworkWeightTxt.Text, out double courseworkWeight))
            {

                CourseworkAddEvent?.Invoke(courseworkNameTxt.Text, (courseworkNameTxt.Text, courseworkWeight));
            }
            else
            {
                MessageBox.Show("Cannot parse the weight value as a number");
                return;
            }
        }
    }
}
