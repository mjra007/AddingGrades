using AddinGrades.DTO;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace AddinGrades.Upgrader
{
    internal class UpdateFrom1Dot2To1Dot3 : IUpdater
    {
        public string OriginVersion => "v1.2";

        public string FinalVersion => "v1.3";

        public void Update()
        {
            Microsoft.Office.Interop.Excel.Application app = Utils.GetExcelApplication();
            if (app != null)
            {
                WorkbookData workbookData = Utils.LoadWorkbookData(app);
                if (workbookData != null)
                {
                    PatchHeaderNames(app, workbookData);
                    PatchHeaderNamesFeedbackTable();
                    InsertStudentPicturesFeedbackTable(workbookData);
                    InsertStudentPicturesGradeTable(workbookData);
                }
            }
        }

        private void InsertStudentPicturesGradeTable(WorkbookData workbookData)
        {
            if (workbookData != null)
            {
                foreach (string gradeSheetID in workbookData.GradeSheets.Keys)
                {
                    Worksheet sheet = Utils.GetWorksheetById(gradeSheetID);
                    if (sheet != null)
                    {
                        using (Unprotecter unprotecter = new(sheet))
                        {
                            Pictures excelPictures = (Pictures)sheet.Pictures(Type.Missing);

                            foreach (Picture item in excelPictures)
                            {
                                item.Delete();
                            }

                            Range currentCell = sheet.get_Range("A3");

                            double rColRow = 6; // Ratio of units of measure: columns widths to row heights
                            double rImgColWidth = 5.9; // Ratio of units of measure: image size and column widths

                            double lastMaxWidth = 0d;

                            IEnumerable<string> studentNames = Utils.GetStudentNames(sheet).ToList();
                            string className = workbookData.ClassName;

                            if (string.IsNullOrEmpty(className) is false)
                            {
                                foreach (string name in studentNames)
                                {
                                    if (File.Exists(Path.Combine(Program.ExcelAddinPathDir, "StudentImages", $"{className}-{name}.png")))
                                    {
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
                            }
                        }
                    }

                }
            }
        }

        private void InsertStudentPicturesFeedbackTable(WorkbookData workbookData)
        {
            Worksheet sheet = Utils.GetFeedbackSheet();
            if (sheet != null)
            {
                using (Unprotecter unprotecter = new(sheet))
                {
                    Range collumnNameCell = sheet.get_Range($"{Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(sheet, GradeTable.CollumnName.Student, "A1") + 1)}1");
                    collumnNameCell.EntireColumn.Insert(XlInsertShiftDirection.xlShiftToRight, XlInsertFormatOrigin.xlFormatFromRightOrBelow);

                    Range currentCell = sheet.get_Range("A2");

                    double rColRow = 6; // Ratio of units of measure: columns widths to row heights
                    double rImgColWidth = 5.9; // Ratio of units of measure: image size and column widths

                    double lastMaxWidth = 0d;

                    IEnumerable<string> studentNames = Utils.GetStudentNamesFromFeedback(sheet).ToList();
                    string className = workbookData.ClassName;

                    if (string.IsNullOrEmpty(className) is false)
                    {
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
                            }
                            currentCell = currentCell.Offset[1, 0];
                        }
                    }

                    FeedbackTable.LockCollumnsAndHeaders(sheet);
                    FeedbackTable.SetStyle(sheet);
                }
            }

        }

        static List<(string, string)> oldGradeHeaders = new()
            {
                ("Knowledge", GradeTable.CollumnName.Knowledge),
                ("Weighted Table", GradeTable.CollumnName.CourseworkWeigthtedTable),
                ("Final Grade", GradeTable.CollumnName.FinalGrade),
                ("Observations", GradeTable.CollumnName.Observations),
            };
        private void PatchHeaderNames(Microsoft.Office.Interop.Excel.Application app, WorkbookData data)
        {
            app.Calculation = XlCalculation.xlCalculationManual;

            //Fix collumn name from feedback to sinteses
            foreach (string gradeSheetID in data.GradeSheets.Keys)
            {
                Worksheet sheet = Utils.GetWorksheetById(gradeSheetID);
                if (sheet != null)
                {
                    using (Unprotecter unprotecter = new(sheet))
                    {
                        foreach (var item in oldGradeHeaders)
                        {
                            string feedbackColumnName = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(sheet, item.Item1) + 1);
                            sheet.Columns.get_Range($"{feedbackColumnName}2").Value2 = item.Item2;
                        }

                    }

                    //GradeTable.LockCollumnsAndHeaders(sheet);
                    GradeTable.ApplyStyles(sheet);
                    sheet.Protect();
                }
            }
            app.Calculation = XlCalculation.xlCalculationAutomatic;
        }

        static List<(string, string)> oldFeedbackHeaders = new()
            {
                ("Final Grade", GradeTable.CollumnName.FinalGrade),
                ("Student", GradeTable.CollumnName.Student),
            };
        private void PatchHeaderNamesFeedbackTable()
        {

            Worksheet sheet = Utils.GetFeedbackSheet();
            if (sheet != null)
            {
                using (Unprotecter unprotecter = new(sheet))
                {
                    foreach (var item in oldFeedbackHeaders)
                    {
                        string feedbackColumnName = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(sheet, item.Item1, "A1") + 1);
                        Range range = sheet.Columns.get_Range($"{feedbackColumnName}1");
                        range.Value = item.Item2;

                        int indexLast = GradeTable.GetLastHeaderCollumnWithValue("A1");
                        for (int i = 0; i < indexLast; i++)
                        {
                            if (range.Value is not null && range.Value.Equals(oldFeedbackHeaders[0].Item1))
                            {
                                range.Value = item.Item2;
                            }
                            range = range.Offset[0, 2];
                        }
                    }
                }
                sheet.Protect();
            }
        }
    }
}
