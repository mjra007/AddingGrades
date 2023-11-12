using AddinGrades.DTO;
using ExcelDna.Integration;
using Microsoft.Office.Interop.Excel;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace AddinGrades
{
    public partial class CreateGradeSheet : Form
    {
        ChromeDriver driver;
        public CreateGradeSheet()
        {
            InitializeComponent();
            driver = ChromeDriverInstaller.SetupChromeDriver();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            new LoginController().Login(driver, "nunopinho1@gmail.com", new NetworkCredential("", passwordTxt.Text).SecurePassword);
            IEnumerable<string> classNames = Utils.GetClasses(driver);
            classesDropDown.Items.AddRange(classNames.ToArray());
            groupClassPicker.Enabled = true;
        }

        private void createGradeSheetButton_Click(object sender, EventArgs e)
        { 
            Application app = ExcelDnaUtil.Application as Application;  
            Program.CreationOfGradeSheetInProgress = true;
            WorkbookData data = app.LoadWorkbookData().IfNullCreate();
            //Add custom ID to gradeSheet if not created already
            Worksheet worksheet = app.ActiveSheet as Worksheet;
            string gradeSheetID = worksheet.GetCustomID();
            gradeSheetID ??= worksheet.CreateCustomID();
            //create gradesheet in workbookdata
            GradeSheet gradeSheet = new();
            gradeSheet.CourseworkWeightedTables.Add(new CourseworkWeightedTable("Default", gradeSheet.Coursework,
                Enumerable.Repeat(0d, gradeSheet.Coursework.Count).ToArray()));
            data.GradeSheets.Add(gradeSheetID, gradeSheet);
            data.Save();

            if (classesDropDown.SelectedItem is null)
            {
                new GradeTable(gradeSheetID).CreateDefaultTable(data, app, new List<string>());
            }
            else
            {
                IEnumerable<string> studentNames = Utils.GetListOfStudents(driver, (string)classesDropDown.SelectedItem);
                new GradeTable(gradeSheetID).CreateDefaultTable(data, app, studentNames);
            }
            Program.CreationOfGradeSheetInProgress = false;
            this.Close();
        }

    }
} 
