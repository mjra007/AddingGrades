using AddinGrades.DTO;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace AddinGrades.Upgrader
{
    internal class UpdateFrom1Dot1To1Dot2 : IUpdater
    {
        public string OriginVersion => "v1.1";

        public string FinalVersion => "v1.2";

        public void Update()
        {
            Microsoft.Office.Interop.Excel.Application app = Utils.GetExcelApplication();
            if (app != null)
            {
                WorkbookData workbookData = Utils.LoadWorkbookData(app);
                if (workbookData != null)
                {
                    foreach (string gradeSheetID in workbookData.GradeSheets.Keys)
                    {
                        Worksheet sheet = Utils.GetWorksheetById(gradeSheetID);
                        if (sheet != null)
                        {
                            using (Unprotecter unprotecter = new(sheet))
                            {
                                Range collumnNameCell = sheet.Range[$"{Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(sheet, GradeTable.CollumnName.Student) + 1)}2"];
                                collumnNameCell.EntireColumn.Insert(XlInsertShiftDirection.xlShiftToRight, XlInsertFormatOrigin.xlFormatFromRightOrBelow);

                                Range currentCell = sheet.get_Range("A3");

                                double rColRow = 6; // Ratio of units of measure: columns widths to row heights
                                double rImgColWidth = 5.9; // Ratio of units of measure: image size and column widths

                                double lastMaxWidth = 0d;

                                IEnumerable<string> studentNames = Utils.GetStudentNames(sheet);
                                string className = FindClassWithStudents(studentNames);

                                if (string.IsNullOrEmpty(className) is false)
                                {
                                    workbookData.ClassName = className;
                                    workbookData.Save();
                                    foreach (string name in studentNames)
                                    {
                                        if (File.Exists(Path.Combine(Program.ExcelAddinPathDir, "StudentImages", $"{className}-{name}.png")))
                                        {
                                            Pictures excelPictures = (Pictures)sheet.Pictures(Type.Missing);
                                            Picture excelPicture = excelPictures.Insert(Path.Combine(Program.ExcelAddinPathDir, "StudentImages", $"{className}-{name}.png"));
                                            excelPicture.Top = currentCell.Top;
                                            excelPicture.Left = currentCell.Left;
                                            if (excelPicture.Width > lastMaxWidth)
                                            {
                                                currentCell.ColumnWidth = (excelPicture.Width / rImgColWidth);
                                                lastMaxWidth = excelPicture.Width;
                                            }
                                            currentCell.RowHeight = excelPicture.Height;
                                            currentCell = currentCell.Offset[1, 0];
                                        }
                                    }
                                }
                            }
                            GradeTable.LockCollumnsAndHeaders(sheet);
                            GradeTable.ApplyStyles(sheet);
                        }
                    }
                    workbookData.Version = "v1.2";
                    workbookData.Save();
                }
            }
        } 

        private static string? FindClassWithStudents(IEnumerable<string> studentNames)
        {
            if (Program.StudentsCache is not null)
            {
                foreach (var pair in Program.StudentsCache.StudnetsByClass)
                {
                    string className = pair.Key;

                    int threshold = 3;
                    int counter = 0;
                    foreach (string studentName in studentNames)
                    {
                        if (pair.Value.Contains(studentName))
                        {
                            counter++;
                        }

                        if (counter == threshold) return className;
                    }
                }
            }
            return string.Empty;
        }

    }
}
