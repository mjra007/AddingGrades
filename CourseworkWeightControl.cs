using AddinGrades.DTO;
using Microsoft.Office.Interop.Excel;
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
    public partial class CourseworkWeightControl : UserControl
    {
        public readonly string tableName;
        public readonly EventHandler<string> DeleteEvent;

        public CourseworkWeightControl(string courseworkName, double weight, string tableName, EventHandler<string> courseworkDeleteEvent)
        {
            InitializeComponent();
            this.courseworkName.Text = courseworkName;
            this.courseworkWeight.Text = weight.ToString();
            this.tableName = tableName;
            deleteGroup.Visible = false;
            deleteGroup.Enabled = false;
            DeleteEvent = courseworkDeleteEvent;
        }

        public string GetCourseworkName() => courseworkName.Text;

        public string GetWeight() => courseworkWeight.Text;

        private void deleteCourseworkBtn_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            groupBox1.Visible = false;
            deleteGroup.Visible = true;
            deleteGroup.Enabled = true;
        }

        private void yesButton_Click(object sender, EventArgs e)
        {
            DeleteEvent?.Invoke(this, this.courseworkName.Text);
        }

        private void noButton_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            groupBox1.Visible = true;
            deleteGroup.Visible = false;
            deleteGroup.Enabled = false;
        }
         

    }
}
