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

            numberOfSheetsComboBox.Items.AddRange(Enumerable.Range(1, 5).Cast<object>().ToArray());
            numberOfSheetsComboBox.SelectedItem = 3;
            if (Program.StudentsCache is not null)
            {
                classesDropDown.Items.Clear();
                classesDropDown.Items.AddRange(Program.StudentsCache.StudnetsByClass.Keys.ToArray());
                cacheData.Text = Program.StudentsCache.DateOfCache.ToString();
                groupClassPicker.Enabled = true;
            }
            else
            {
                cacheData.Text = "No cache data exists.";
            }
        }


        private void createGradeSheetButton_Click(object sender, EventArgs e)
        {
            Application app = ExcelDnaUtil.Application as Application;
            Program.CreationOfGradeSheetInProgress = true;
            WorkbookData data = app.LoadWorkbookData().IfNullCreate();

            Worksheet worksheet = app.ActiveWorkbook.ActiveSheet;
            if (string.IsNullOrEmpty(numberOfSheetsComboBox.SelectedItem.ToString()) is false)
            {
                int numberOfSheets = int.Parse(numberOfSheetsComboBox.SelectedItem.ToString());
                for (int i = 0; i < numberOfSheets; i++)
                {
                    //Add custom ID to gradeSheet if not created already
                    worksheet = app.ActiveWorkbook.Worksheets.Add(After: worksheet);
                    worksheet.Name = $"{DefaultGradeSheetName} {i + 1}";
                    string gradeSheetID = worksheet.GetCustomID();
                    gradeSheetID ??= worksheet.CreateCustomID();
                    //create gradesheet in workbookdata
                    GradeSheet gradeSheet = new();
                    gradeSheet.CourseworkWeightedTables.Add(new CourseworkWeightedTable("Padrão", gradeSheet.Coursework,
                        Enumerable.Repeat(0d, gradeSheet.Coursework.Count).ToArray()));
                    data.GradeSheets.Add(gradeSheetID, gradeSheet);
                    data.Save();
                    if (classesDropDown.SelectedItem is null)
                    {
                        new GradeTable(gradeSheetID).CreateDefaultTable(worksheet, data, app, new List<string>());
                    }
                    else
                    {
                        IEnumerable<string> studentNames = Program.StudentsCache.StudnetsByClass[(string)classesDropDown.SelectedItem];
                        new GradeTable(gradeSheetID).CreateDefaultTable(worksheet, data, app, studentNames);
                    }
                }
            }


            Worksheet feedback = Utils.GetFeedbackSheet();
            if (feedback != null)
            {
                FeedbackTable.LockCollumnsAndHeaders(feedback);
                feedback.Protect(AllowFormattingColumns: true, AllowFormattingCells: true, AllowFormattingRows: true);
            }
            //For some reason chrome driver makes it so you have to bind this event again 
            Utils.GetExcelApplication().ActiveWorkbook.SheetChange += Program.OnSheetChange;
            Program.CreationOfGradeSheetInProgress = false;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox1.Enabled = false;
            groupBox6.Visible = true;
            groupBox6.Enabled = true;
        }

        private void returnButton_Click(object sender, EventArgs e)
        {
            groupBox6.Visible = false;
            groupBox6.Enabled = false;
            groupBox1.Visible = true;
            groupBox1.Enabled = true;
        }

        private void loginButton_Click_1(object sender, EventArgs e)
        {
            driver = ChromeDriverInstaller.SetupChromeDriver();
            new LoginController().Login(driver, "nunopinho1@gmail.com", new NetworkCredential("", passwordTxt.Text).SecurePassword);
            IEnumerable<string> classNames = Utils.GetClasses(driver).ToList();
            StudentsCache studentsCache = new()
            {
                DateOfCache = DateTime.Now
            };
            progressBar1.Maximum = classNames.Count();
            foreach (string className in classNames)
            {
                studentsCache.StudnetsByClass.Add(className, Utils.GetListOfStudents(driver, className));
                progressBar1.Increment(1);
            }
            File.WriteAllText(Program.CacheFileName, WorkbookData.Serialize(studentsCache));
            Program.StudentsCache = studentsCache;
            classesDropDown.Items.Clear();
            classesDropDown.Items.AddRange(classNames.ToArray());
            groupClassPicker.Enabled = true;
            groupBox6.Visible = false;
            groupBox6.Enabled = false;
            groupBox1.Visible = true;
            groupBox1.Enabled = true;
            progressBar1.Value = 0;
        }
    }
}
