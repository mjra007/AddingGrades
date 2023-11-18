using AddinGrades.DTO;
using ExcelDna.Integration;
using Microsoft.Office.Interop.Excel;
using OpenQA.Selenium.Chrome;
using System.Net;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace AddinGrades
{
    public partial class CreateGradeSheet : Form
    {
        ChromeDriver driver;
        private static string DefaultGradeSheetName = "Módulo";
        public CreateGradeSheet()
        {
            InitializeComponent();
            driver = ChromeDriverInstaller.SetupChromeDriver();
            numberOfSheetsComboBox.Items.AddRange(Enumerable.Range(1, 5).Cast<object>().ToArray());
            numberOfSheetsComboBox.SelectedItem = 3;
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
            IEnumerable<string> studentNames = new List<string>();
            if (string.IsNullOrEmpty(numberOfSheetsComboBox.SelectedItem.ToString()) is false)
            {
                int numberOfSheets = int.Parse(numberOfSheetsComboBox.SelectedItem.ToString());
                for (int i = 0; i < numberOfSheets; i++)
                {
                    //Add custom ID to gradeSheet if not created already
                    Worksheet worksheet = app.ActiveWorkbook.Worksheets.Add();
                    worksheet.Name = $"{DefaultGradeSheetName} {numberOfSheets - i}";
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
                        new GradeTable(gradeSheetID).CreateDefaultTable(worksheet, data, app, new List<string>());
                    }
                    else
                    {
                        if(studentNames.Any() is false)
                            studentNames = Utils.GetListOfStudents(driver, (string)classesDropDown.SelectedItem);
                        new GradeTable(gradeSheetID).CreateDefaultTable(worksheet, data, app, studentNames);
                    }
                } 
            }

            //For some reason chrome driver makes it so you have to bind this event again 
            Utils.GetExcelApplication().ActiveWorkbook.SheetChange += Program.OnSheetChange;
            Program.CreationOfGradeSheetInProgress = false;

            this.Close();
        }

    }
} 
