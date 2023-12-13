using AddinGrades.DTO;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddinGrades.Upgrader
{
    public class UpdateFrom1to1Dot1 : IUpdater
    {
        public string OriginVersion => "v1";

        public string FinalVersion => "v1.1";

        public void Update()
        { 
            Microsoft.Office.Interop.Excel.Application app = Utils.GetExcelApplication();
            if (app != null)
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
                    if (worksheet != null)
                    {
                        using (Unprotecter unprotecter = new(worksheet))
                        {
                            Microsoft.Office.Interop.Excel.Range range = worksheet.Columns.get_Range("C1");
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
    }
}
