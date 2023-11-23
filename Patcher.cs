using AddinGrades.DTO;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace AddinGrades
{
    public class Patcher
    {
        public static Dictionary<(string oldVersion, string newVersion), System.Action> UpdaterDictionary = new(){

            { ("v1" , "v1.1"), UpdateFrom1To1Dot1 }
        };

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
