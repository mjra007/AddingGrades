using AddinGrades.DTO;
using Microsoft.Office.Interop.Excel; 
using Application = Microsoft.Office.Interop.Excel.Application;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace AddinGrades
{
    public class GradeTable
    {
        static readonly List<string> DefaultColumns = new()
        {
            "Aluno", "Knowledge", "Atitudes", "Coursework Weight Table", "Final Grade", "Feedback", "Observations"
        };

        public readonly string GradeSheetID;

        public GradeTable(string gradeSheetID) 
        {
            GradeSheetID = gradeSheetID;
        }

        public void CreateDefaultTable(WorkbookData data, Application app)
        {
            Worksheet worksheet = app.ActiveSheet as Worksheet;
            Range currentCell = worksheet.get_Range("A2");

            foreach (string columnName in DefaultColumns)
            {
                currentCell.Cells[1] = columnName;
                currentCell = currentCell.Offset[0, 1];
            }

            string lastColumn = Utils.GetExcelColumnName(DefaultColumns.Count);
            worksheet.get_Range("A3", $"{lastColumn}100").Locked = false;
            worksheet.get_Range("A2", $"{lastColumn}2").Cells.Font.Size = 13;
            worksheet.get_Range("A2", $"{lastColumn}2").Locked = true;
            worksheet.get_Range("A2", $"{lastColumn}2").Interior.Color = ColorTranslator.ToOle(Color.LightGoldenrodYellow);
            worksheet.Columns.AutoFit();
            worksheet.Columns[1].ColumnWidth = 25;
            worksheet.Protect();

            InsertDropdownForWeightedTable(); 
        }

        //public void RecalculateGrades()
        //{
        //    Worksheet sheet = Utils.GetWorksheetById(GradeSheetID);
        //    int knowledgeCollumn = GetCollumnByName("Knowledge") + 1;
        //    int tableCollumn = GetCollumnByName("Coursework Weight Table") + 1;
        //    string knowledgeCollumnName = Utils.GetExcelColumnName(knowledgeCollumn);
        //    string weightedTableCollumnName = Utils.GetExcelColumnName(tableCollumn); 
        //    sheet.Unprotect(); 
        //    Range cellKnowledge = sheet.get_Range($"{knowledgeCollumnName}3");
        //    WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
        //}

        public void InsertDropdownForWeightedTable()
        {
            Worksheet sheet = Utils.GetWorksheetById(GradeSheetID);
            int weightedTableCollumn = GetCollumnByName("Coursework Weight Table") + 1;
            string collumnName = Utils.GetExcelColumnName(weightedTableCollumn); 
            sheet.Unprotect(); 
            Range cell = sheet.get_Range($"{collumnName}3");
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
            for (int i = 0; i < 35; i++)
            {
                cell.Validation.Delete();
                cell.Validation.Add(
                   XlDVType.xlValidateList,
                   XlDVAlertStyle.xlValidAlertInformation,
                   XlFormatConditionOperator.xlBetween,
                   string.Join(',', data.GradeSheets[GradeSheetID].CourseworkWeightedTables.Select(s => s.name)),
                   Type.Missing);

                cell.Validation.IgnoreBlank = true;
                cell.Validation.InCellDropdown = true;
                cell.Value2 = data.GradeSheets[GradeSheetID].CourseworkWeightedTables.First().name;
                cell = cell.Offset[1, 0];
            } 
            sheet.Protect(); 
        }

        public void InsertNewCoursework(params Coursework[] courseworks)
        {
            Worksheet sheet = Utils.GetWorksheetById(GradeSheetID); 
            sheet.Unprotect(); 
            int insertCollumn = GetLastCourseworkCollumn()+1;

            foreach (Coursework coursework in courseworks)
            {
                Range collumnNameCell = sheet.Range[$"{Utils.GetExcelColumnName(insertCollumn)}2"];
                collumnNameCell.EntireColumn.Insert(XlInsertShiftDirection.xlShiftToRight, XlInsertFormatOrigin.xlFormatFromRightOrBelow);
                //reference needs to be updated so it gets the new inserted cell instead of the moved one
                collumnNameCell = sheet.Range[$"{Utils.GetExcelColumnName(insertCollumn)}2"];
                collumnNameCell.Value2 = coursework.Name;
                insertCollumn++;  
            }
            sheet.Columns.AutoFit();
            sheet.Protect();
        }

        public void DeleteCourseworkCollumn(string name)
        {
            Worksheet worksheet = Utils.GetWorksheetById(GradeSheetID); 
            worksheet.Unprotect(); 
            string collumnLetter =Utils.GetExcelColumnName(GetCourseworkIndex(name)+1);
            worksheet.get_Range($"{collumnLetter}2").Delete();
            worksheet.Protect();
        }

        public int GetLastCourseworkCollumn()
        {
            Worksheet worksheet = Utils.GetWorksheetById(GradeSheetID);
            Range currentCell = worksheet.get_Range("A2");
            int counter = 0;
            //Looks for the collumn before knowledge. All coursework collumns should be between Aluno and Knowledge
            while(currentCell.Value2 != "Knowledge")
            {
                currentCell = currentCell.Offset[0, 1];
                counter++;
            };
            return counter;
        }

        public int GetCourseworkIndex(string courseworkName)
        {
            Worksheet worksheet = Utils.GetWorksheetById(GradeSheetID);
            Range currentCell = worksheet.get_Range("A2");
            int counter = 0;

            while (currentCell.Value2 != courseworkName)
            {
                string c = currentCell.Value2;
                currentCell = currentCell.Offset[0, 1];
                counter++; 
            };
            return counter;
        }

        public int GetCollumnByName(string name)
        {
            return GetCourseworkIndex(name);
        }

    }
}
