using AddinGrades.DTO;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace AddinGrades
{
    public class Patcher
    {
        public static Dictionary<(string oldVersion, string newVersion), System.Action> UpdaterDictionary = new(){

            { ("v1" , "v1.1"), UpdateFrom1To1Dot1 },
            { ("v1" , "v1.2"), UpdateFrom1To1Dot2 },
            { ("v1.1", "v1.2"), UpdateFrom1Dot1To1Dot2 }
        };

        private static void UpdateFrom1Dot1To1Dot2()
        {
            UpdateFrom1To1Dot2();
        }

        private static void UpdateFrom1To1Dot2()
        {
            Program.LoggerPanel.WriteLineToPanel("Upgrading project from v1.0 to v1.2");
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
                                Range collumnNameCell = sheet.Range[$"{Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(sheet,GradeTable.CollumnName.Student)+1)}2"];
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
                    foreach(string studentName in studentNames)
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

        //Updates project from 1.0 to 1.1
        private static void UpdateFrom1To1Dot1()
        {
            Program.LoggerPanel.WriteLineToPanel("Upgrading project from v1.0 to v1.1");
            Microsoft.Office.Interop.Excel.Application app = Utils.GetExcelApplication();
            if( app != null)
            {
                WorkbookData workbookData = Utils.LoadWorkbookData(app);
                if (workbookData != null)
                {
                    //Fix collumn name from feedback to sinteses
                    foreach (string gradeSheetID in workbookData.GradeSheets.Keys)
                    { 
                        Worksheet sheet = Utils.GetWorksheetById(gradeSheetID);
                        if (sheet != null)
                        {
                            using (Unprotecter unprotecter = new(sheet))
                            {
                                string feedbackColumnName = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(sheet, "Feedback") + 1);
                                sheet.Columns.get_Range($"{feedbackColumnName}2").Value = GradeTable.CollumnName.Feedback;
                            }

                            GradeTable.LockCollumnsAndHeaders(sheet);
                            GradeTable.ApplyStyles(sheet);
                            sheet.Protect();
                        }
                    }

                    //Fix collumn order of sinteses page
                    Worksheet worksheet = Utils.GetFeedbackSheet();
                    var orderedWorkSheetsByName = workbookData.GradeSheets.Keys.Select(id => (Utils.GetWorksheetById(id).Name, id)).OrderBy(s => s.Name);
                    if (worksheet != null) {
                        using (Unprotecter unprotecter = new(worksheet))
                        {
                            Range range = worksheet.Columns.get_Range("C1");
                            foreach (var item in orderedWorkSheetsByName)
                            {
                                range.Value = $"=GetSheetName(\"{item.id}\")";
                                range = range.Offset[0, 2];
                            }
                        }

                        FeedbackTable.LockCollumnsAndHeaders(worksheet);
                        worksheet.Protect();
                    }

                    workbookData.Version = "v1.1";
                    workbookData.Save();  
                     
                }
            } 
        }
         
        public static void UpdateWorkbook(string sheetVersion)
        {
            if (UpdaterDictionary.TryGetValue((sheetVersion, Program.Version), out var updaterMethod))
            {
                updaterMethod.Invoke();
            }
        }
    }
}
