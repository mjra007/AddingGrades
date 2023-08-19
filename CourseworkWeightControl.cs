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

        public CourseworkWeightControl(string courseworkName, double weight, string tableName)
        {
            InitializeComponent();
            this.courseworkName.Text = courseworkName;
            this.courseworkWeight.Text = weight.ToString();
            this.tableName = tableName;
        }

        public string GetCourseworkName() => courseworkName.Text;

        public string GetWeight() => courseworkWeight.Text;
    }
}
