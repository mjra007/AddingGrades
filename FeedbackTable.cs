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
            public const string Student = "Aluno";

        }

        public FeedbackTable(string SheetID)
        {
            this.SheetID = SheetID;
        }

        public void CreateTable(IEnumerable<string> students, string className)
        {
            Application app = ExcelDnaUtil.Application as Application;
            Worksheet worksheet = app.ActiveSheet as Worksheet;
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData(); 
            worksheet.Name = GradeTable.CollumnName.Feedback;


            Range currentCell = worksheet.get_Range("A2");

            double rColRow = 6; // Ratio of units of measure: columns widths to row heights
            double rImgColWidth = 5.9; // Ratio of units of measure: image size and column widths

            double lastMaxWidth = 0d;
            foreach (string name in students)
            {
                if (File.Exists(Path.Combine(Program.ExcelAddinPathDir, "StudentImages", $"{className}-{name}.png")))
                {
                    Pictures excelPictures = (Pictures)worksheet.Pictures(Type.Missing);
                    Picture excelPicture = excelPictures.Insert(Path.Combine(Program.ExcelAddinPathDir, "StudentImages", $"{className}-{name}.png"));
                    excelPicture.Top = currentCell.Top;
                    excelPicture.Left = currentCell.Left;
                    if (excelPicture.Width > lastMaxWidth)
                    {
                        currentCell.ColumnWidth = (excelPicture.Width / rImgColWidth);
                        lastMaxWidth = excelPicture.Width;
                    }
                    currentCell.RowHeight = excelPicture.Height;
                }
                currentCell = currentCell.Offset[1, 0];
            }

            currentCell = worksheet.get_Range("B2");
            foreach (string studentName in students)
            {
                currentCell.Cells[1] = studentName;
                currentCell = currentCell.Offset[1, 0]; 
            }
            currentCell.EntireColumn.AutoFit();

            currentCell = worksheet.get_Range("B1");
            currentCell.Columns.AutoFit();
            currentCell.Cells[1] = CollumnName.Student;
            foreach (string gradeSheetID in data.GradeSheets.Keys)
            {
                currentCell = currentCell.Offset[0, 1];
                currentCell.ColumnWidth = 10;
                currentCell.Value = GradeTable.CollumnName.FinalGrade;

                Range cellIteratorForFinalGrade = currentCell;

                currentCell = currentCell.Offset[0, 1];
                currentCell.ColumnWidth = 30;
                currentCell.Formula = $"=GetSheetName(\"{gradeSheetID}\")";
                 
                foreach (string studentName in students)
                {
                    cellIteratorForFinalGrade = cellIteratorForFinalGrade.Offset[1, 0];
                    cellIteratorForFinalGrade.Formula = 
                        $"=GetFinalGrade({Utils.GetExcelColumnName(currentCell.Column)}{currentCell.Row},B{cellIteratorForFinalGrade.Row})";
                }
            } 
        }

        public static void LockCollumnsAndHeaders(Worksheet worksheet)
        {
            int indexLast = GradeTable.GetLastHeaderCollumnWithValue("B1");
            string feedbackColumn = Utils.GetExcelColumnName(indexLast);

            using (Unprotecter unprotecter = new(worksheet))
            {
                //Unlock body of table
                Range range = worksheet.get_Range($"A2", $"{feedbackColumn}100");
                range.Locked = false;
                //Lock headers
                range = worksheet.get_Range($"A1", $"{feedbackColumn}1");
                range.Locked = true;
                 
                range = worksheet.get_Range($"C2", $"C100");
                for (int i = 0; i < indexLast; i++)
                {
                    range.Locked = true;
                    range = range.Offset[0, 2];
                } 
            } 
        } 

        public static void SetStyle(Worksheet worksheet)
        {
            int indexLast = GradeTable.GetLastHeaderCollumnWithValue("B1");
            string collumnName = Utils.GetExcelColumnName(indexLast+1);
            using (Unprotecter unprotecter = new(worksheet))
            {
                Range headers = worksheet.get_Range("A1", $"{collumnName}1");
                headers.Cells.Font.Size = 13;
                headers.Cells.Font.FontStyle = "Bold";
                headers.Interior.Color = ColorTranslator.ToOle(Color.LightGoldenrodYellow);

                Range range = worksheet.get_Range($"D2", $"D100"); 
                for (int i = 0; i < indexLast; i++)
                {
                    range.WrapText = true;
                    range.ColumnWidth = 30;
                    range.EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                    range.EntireColumn.VerticalAlignment = XlVAlign.xlVAlignTop;
                    range = range.Offset[0, 2];
                }

                range = worksheet.get_Range($"B2", $"B100"); 
                range.EntireColumn.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range.EntireColumn.AutoFit();
                 
                range = worksheet.get_Range($"C2", $"C100");
                for (int i = 0; i < indexLast; i++)
                {
                    range.EntireColumn.AutoFit();
                    range.EntireColumn.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    range.EntireColumn.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range = range.Offset[0, 2];
                }

            }
        }


    }
}
