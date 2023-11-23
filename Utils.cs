using AddinGrades.DTO;
using ExcelDna.Integration;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers; 
using System.Collections.ObjectModel; 
using Application = Microsoft.Office.Interop.Excel.Application; 
using Range = Microsoft.Office.Interop.Excel.Range;
using System.Linq;

namespace AddinGrades
{
    public static class Utils
    {
        /// <summary>
        /// Takes in a collumn number starting at 1 and returns A, B, C etc
        /// Useful for these kind of methods     Range range = ws.get_Range("A1", GetExcelColumnName(columnNumber) + "1");
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;
            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }
            return columnName;
        }

        public static WorkbookData LoadWorkbookData(this Microsoft.Office.Interop.Excel.Application application)
        {
            if (application is null || application.ActiveWorkbook is null) return null;
            foreach (var item in application.ActiveWorkbook.CustomXMLParts.Cast<CustomXMLPart>())
            {
                    string xml = item.XML;
                    if (xml.Contains("WorkbookData"))
                    {
                        return WorkbookData.Deserialize<WorkbookData>(xml); 
                    }
            }
            return null;
        }

        public static Application GetExcelApplication()
        {
            return ExcelDnaUtil.Application as Application;
        }

        public static string? GetCurrentSheetID()
        {
            Worksheet sheet = GetExcelApplication().ActiveSheet as Worksheet;
            if (sheet.CustomProperties.Cast<CustomProperty>().Any(c => c.Name == "CustomID"))
            {
                return sheet.GetProperty("CustomID");
            }
            return null; 
        } 
          
        public static Worksheet? GetSheetByExcelReference(ExcelReference reference)
        {
            string sheetName = (string)XlCall.Excel(XlCall.xlSheetNm, reference);
            sheetName = sheetName.Split("]")[1];
            foreach (Worksheet sheet in Utils.GetExcelApplication().Worksheets)
            {
                if (sheet.Name.Equals(sheetName)) return sheet;
            }
            return null;
        }

        public static WorkbookData IfNullCreate(this WorkbookData data)
        {
            Application app = ExcelDnaUtil.Application as Application;
            if (data is null)
            {
                app.CreateWorkbookData();
                return app.LoadWorkbookData();
            }
            return data;
        }

        public static void CreateWorkbookData(this Application application)
        {
            application.ActiveWorkbook.CustomXMLParts.Add(WorkbookData.Serialize(new WorkbookData()));
        }

     
        public static string CreateCustomID(this Worksheet worksheet, bool feedbackSheet = false)
        {
            string gradeSheetID;
            worksheet.CustomProperties.Add("CustomID", gradeSheetID = Guid.NewGuid().ToString());
            if(feedbackSheet)
                worksheet.CustomProperties.Add("FeedbackSheet", feedbackSheet.ToString());
            return gradeSheetID;
        }

        public static bool IsFeedback()
        {
            Worksheet sheet = GetExcelApplication().ActiveSheet as Worksheet;
            if (sheet.CustomProperties.Cast<CustomProperty>().Any(c => c.Name == "FeedbackSheet"))
            {
                return bool.Parse(sheet.GetProperty("FeedbackSheet"));
            }
            return false;
        }

        public static bool IsFeedback(this Worksheet sheet)
        { 
            if (sheet.CustomProperties.Cast<CustomProperty>().Any(c => c.Name == "FeedbackSheet"))
            { 
                return bool.Parse(sheet.GetProperty("FeedbackSheet"));
            }
            return false;
        }

        public static string? GetCustomID(this Worksheet sheet)
        {
            if (sheet.CustomProperties.Cast<CustomProperty>().Any(c => c.Name == "CustomID"))
            {
                return sheet.GetProperty("CustomID");
            }
            return null;
        }

        public static Worksheet? GetFeedbackSheet()
        {
            Application app = ExcelDnaUtil.Application as Application;
            foreach (Worksheet worksheet in app.Sheets)
            { 
                if (worksheet.IsFeedback())
                {
                    return worksheet;
                } 
            }
            return null;
        }

        public static string GetProperty(this Worksheet ws, string name)
        {
            foreach (CustomProperty cp in ws.CustomProperties)
                if (cp.Name == name)
                    return cp.Value;
            return null;
        }

        public static Worksheet? GetWorksheetById(string gradeID)
        {
            Application app = GetExcelApplication();
            foreach (Worksheet sheet in app.Worksheets)
            { 
                if (sheet is not null && sheet.GetCustomID() is not null && sheet.GetCustomID().Equals(gradeID)) return sheet;
            }
            return null;
        } 

        public static void SetProperty(this Worksheet ws, string name, string value)
        {
            bool found = false;
            CustomProperties cps = ws.CustomProperties;
            foreach (CustomProperty cp in cps)
            {
                if (cp.Name == name)
                {
                    found = true;
                    cp.Value = value;
                }
            }
            if (!found)
                cps.Add(name, value);
        }

        public static bool IsEditing(Application excelApp)
        {
            if (excelApp.Interactive)
            {
                try
                {
                    excelApp.Interactive = false;
                    excelApp.Interactive = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Please stop cell editing before you continue.");
                    return true;
                }
            }
            return false;
        }

        static List<string> FilteredTags = new() { "Abandonou", "Desistiu", "Anulou Matrícula", "Excluído por Faltas", "Exclusão por motivos disciplinares",
        "Transferido de Escola", "Transferido de Turma", "Transferido de Curso"};
        public static List<string> GetListOfStudents(ChromeDriver driver, string className)
        {
            List<string> studentNames = new();
            try
            {
                driver.Navigate().GoToUrl("https://jobra.eschoolingserver.com/DesktopDefault.aspx?tabindex=1&tabid=245&portalId=0");
                var firstWindow = driver.WindowHandles.First();
                driver.OpenClass(className); ;
                var windowClasss = driver.WindowHandles.Last();
                driver.SwitchTo().Window(windowClasss); 
                driver.FindElement(By.Id("__tab_ctl00_editContentPlaceHolder_Tabs_tp3")).Click();
                var studentsFiltered = driver.FindElements(By.CssSelector("div[class='divImgStudent']")).Select(s => s.FindElements(By.TagName("a")))
                    .Where(s => s.Where(d => FilteredTags.Contains(d.GetAttribute("title"))).All(s => s.GetAttribute("style").Equals("display: none;") is true))
                    .Select(s=>s.First(d=>d.GetAttribute("id").Equals("studentImage")).GetAttribute("title")).ToList();
                studentNames.AddRange(studentsFiltered); 
                driver.Close();
                driver.SwitchTo().Window(firstWindow);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return studentNames;
        }
        public static void InsertInputById(this WebDriver driver, string id, string value) => driver.FindElement(By.Id(id)).SendKeys(value);

        public static void OpenClass(this WebDriver driver, string className)
        {
            foreach (ISearchContext searchContext in new WebDriverWait((IWebDriver)driver, TimeSpan.FromSeconds(10.0)).Until<IWebElement>(ExpectedConditions.ElementExists(By.Id("table_387_divTable"))).FindElements(By.TagName("tr")).Skip<IWebElement>(1))
            {
                ReadOnlyCollection<IWebElement> elements = searchContext.FindElements(By.TagName("td"));
                if (elements[2].Text.Equals(className))
                    elements[1].Click();
            }
        } 
        public static IEnumerable<string> GetClasses(this ChromeDriver driver)
        {
            driver.Navigate().GoToUrl("https://jobra.eschoolingserver.com/DesktopDefault.aspx?tabindex=1&tabid=245&portalId=0");
            foreach (ISearchContext searchContext in new WebDriverWait((IWebDriver)driver, TimeSpan.FromSeconds(10.0)).Until<IWebElement>(ExpectedConditions.ElementExists(By.Id("table_387_divTable"))).FindElements(By.TagName("tr")).Skip<IWebElement>(1))
                yield return searchContext.FindElements(By.TagName("td"))[2].Text; 
        }

         
        public static int? GetCollumnByNameIndex(string sheetID, string courseworkName, string firstCell = "A2")
        {
            Worksheet worksheet = Utils.GetWorksheetById(sheetID);
            Range currentCell = worksheet.get_Range(firstCell);
            int counter = 0;

            while (currentCell.Value2 != courseworkName)
            {
                string c = currentCell.Value2;
                currentCell = currentCell.Offset[0, 1];
                if (counter == 100)
                {
                    return null;
                }
                counter++;
            };
            return counter;
        }

        public static int GetCollumnByNameIndex(Worksheet sheet, string courseworkName, string firstCell = "A2")
        { 
            Range currentCell = sheet.get_Range(firstCell);
            int counter = 0;

            while (currentCell.Value2 != courseworkName)
            {
                string c = currentCell.Value2;
                currentCell = currentCell.Offset[0, 1];
                counter++;
            };
            return counter;
        }

        public static int? GetRowByNameIndex(string sheetID, string studentName,string columnName, int limit = 50)
        {
            Worksheet worksheet = Utils.GetWorksheetById(sheetID);
            if (worksheet is null) return null;
            Range currentCell = worksheet.get_Range($"{columnName}1");
            int counter = 0;
            bool found;
            while (found = currentCell.Value2 != studentName)
            {
                string c = currentCell.Value2;
                currentCell = currentCell.Offset[1, 0];
                if(counter == limit)
                {
                    return null;
                }
                counter++;
            };
            return found == false ? counter : null; 
        }

        public static int? GetRowByNameIndex(Worksheet worksheet, string rowValue, string columnName, int limit = 50)
        { 
            Range currentCell = worksheet.get_Range($"{columnName}1");
            int counter = 0;
            bool found;
            while (found = currentCell.Value2 != rowValue)
            {
                string c = currentCell.Value2;
                currentCell = currentCell.Offset[1, 0];
                if (counter == limit)
                {
                    return null;
                }
                counter++;
            };
            return found == false ? counter : null;
        }
    }
}
