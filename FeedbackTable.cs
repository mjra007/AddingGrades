﻿using AddinGrades.DTO;
using ExcelDna.Integration;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace AddinGrades
{
    public class FeedbackTable
    {
        public readonly string GradeSheetID; 
        public static class CollumnName
        {
            public const string Student = "Student";

        }

        public FeedbackTable(string gradeSheetID)
        {
            GradeSheetID = gradeSheetID;
        }

        public void CreateTable(IEnumerable<string> students)
        {
            Application app = ExcelDnaUtil.Application as Application;
            Worksheet worksheet = app.ActiveSheet as Worksheet;
            Range currentCell = worksheet.get_Range("A1"); 
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
            Dictionary<string,string> worksheetsIDNameMapping = app.Worksheets.Cast<Worksheet>()
                .Select(s=>(s.GetCustomID(), s.Name)).ToDictionary(s=>s.Item1, s=>s.Name);

            worksheet.Name = "Sínteses";
            currentCell.Cells[1] = CollumnName.Student;
            foreach (string columnName in data.GradeSheets.Keys)
            {
                currentCell = currentCell.Offset[0, 1];
                currentCell.Cells[1] = worksheetsIDNameMapping[columnName];
            }

            currentCell = worksheet.get_Range("A2");
            foreach (string studentName in students)
            {
                currentCell.Cells[1] = studentName;
                currentCell = currentCell.Offset[1, 0];
            }
            worksheet.Columns.AutoFit();
        } 

        public void AddCollumn()
        {

        }
    }
}
