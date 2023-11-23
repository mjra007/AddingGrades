using AddinGrades.DTO;
using ExcelDna.Integration;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace AddinGrades
{
    public class FeedbackTable
    {
        public readonly string SheetID; 
        public static class CollumnName
        {
            public const string Student = "Student";

        }

        public FeedbackTable(string SheetID)
        {
            this.SheetID = SheetID;
        }

        public void CreateTable(IEnumerable<string> students)
        {
            Application app = ExcelDnaUtil.Application as Application;
            Worksheet worksheet = app.ActiveSheet as Worksheet;
            Range currentCell = worksheet.get_Range("A2"); 
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData(); 
            worksheet.Name = "Sínteses"; 

            foreach (string studentName in students)
            {
                currentCell.Cells[1] = studentName;
                currentCell = currentCell.Offset[1, 0]; 
            } 

            currentCell = worksheet.get_Range("A1");
            currentCell.Columns.AutoFit();
            currentCell.Cells[1] = CollumnName.Student;
            foreach (string gradeSheetID in data.GradeSheets.Keys)
            {
                currentCell = currentCell.Offset[0, 1];
                currentCell.ColumnWidth = 10;
                currentCell.Value = "Final Grade";

                Range cellIteratorForFinalGrade = currentCell;

                currentCell = currentCell.Offset[0, 1];
                currentCell.ColumnWidth = 25;
                currentCell.Formula = $"=GetSheetName(\"{gradeSheetID}\")";
                 
                foreach (string studentName in students)
                {
                    cellIteratorForFinalGrade = cellIteratorForFinalGrade.Offset[1, 0];
                    cellIteratorForFinalGrade.Formula = 
                        $"=GetFinalGrade({Utils.GetExcelColumnName(currentCell.Column)}{currentCell.Row},A{cellIteratorForFinalGrade.Row})";
                }
            } 
        }

        public static void LockCollumnsAndHeaders(Worksheet worksheet)
        {
            int indexLast = GradeTable.GetLastHeaderCollumnWithValue() + 1;
            string feedbackColumn = Utils.GetExcelColumnName(indexLast);

            using (Unprotecter unprotecter = new(worksheet))
            {
                //Unlock body of table
                Range range = worksheet.get_Range($"A2", $"{feedbackColumn}100");
                range.Locked = false;
                //Lock headers
                range = worksheet.get_Range($"A1", $"{feedbackColumn}1");
                range.Locked = true;
                 
                range = worksheet.get_Range($"B2", $"B100");
                for (int i = 0; i < indexLast; i++)
                {
                    range.Locked = true;
                    range = range.Offset[0, 2];
                }
            } 
        } 



    }
}
