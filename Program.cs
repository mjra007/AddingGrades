﻿using AddinGrades.DTO;
using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace AddinGrades
{
    public class Program : IExcelAddIn
    {
        public static LoggerPanel? LoggerPanel;
        public static bool CreationOfGradeSheetInProgress = false;
        public static string Version = "v1.3";
        public static string ExcelAddinPathDir = Path.GetDirectoryName((string)XlCall.Excel(XlCall.xlGetName));
        //Cache stuff that should be its own class tbh
        public static string CacheFileName = Path.Combine(ExcelAddinPathDir, "StudentCache.xml");
        public static StudentsCache? StudentsCache;
        static void Main(string[] args)
        {
        }

        public void AutoClose()
        { 
        }

        public void AutoOpen()
        {
            LoggerPanel = Activator.CreateInstance(typeof(LoggerPanel)) as LoggerPanel;
            var ctp = CustomTaskPaneFactory.CreateCustomTaskPane(LoggerPanel, "Grades addin console");
            ctp.Visible = true;
            ctp.DockPosition = MsoCTPDockPosition.msoCTPDockPositionTop;
            ctp.Height = 80;
            Utils.GetExcelApplication().WorkbookOpen += OpenWorkbook; 
            Utils.GetExcelApplication().WorkbookBeforeClose += BeforeCloseWorkbook;
            if (File.Exists(CacheFileName))
            {
                StudentsCache = WorkbookData.Deserialize<StudentsCache>(File.ReadAllText(CacheFileName));
            } 
        }

        private void BeforeCloseWorkbook(Workbook wb, ref bool Cancel)
        {
            wb.SheetChange -= OnSheetChange;
        }

        private void OpenWorkbook(Workbook wb)
        {
            WorkbookData data = Utils.LoadWorkbookData(Utils.GetExcelApplication());
            if (data is not null)
            {
                Patcher.UpdateWorkbook(data.Version);
                wb.SheetChange += OnSheetChange;
            }
        }

        public static void OnSheetChange(object Sh, Range Target)
        { 
            if (Utils.IsFeedback() is false && Utils.GetCurrentSheetID() != null && CreationOfGradeSheetInProgress is false &&
                Target.Column == 2 && Target.Row != 1)//This is a change in the alunos column
            {
                //Empty clean row maybe
                if(Target.Value is null)
                {
                    
                }
                InsertStudentGradeFormulas(Target);
            }
        }

        public static void InsertStudentGradeFormulas(Range Target)
        {
            if (Target.Value2 is object[,])
            {
                GradeTable table = new(Utils.GetCurrentSheetID());

                foreach (Range range in Target.Rows)
                {
                    table.InsertDropdownForWeightedTable(range.Row);
                    table.InsertKnowledgeFunction(range.Row);
                    table.InsertFinalGrade(range.Row);
                    table.InsertFeedback(range.Row);
                }
            }
            else if (Target.Value2 is string && string.IsNullOrEmpty(Target.Value2) is false)
            {
                Program.CreationOfGradeSheetInProgress = true;
                GradeTable table = new GradeTable(Utils.GetCurrentSheetID());
                table.InsertKnowledgeFunction(Target.Row);
                table.InsertDropdownForWeightedTable(Target.Row);
                table.InsertFinalGrade(Target.Row); 
                table.InsertFeedback(Target.Row);
                Program.CreationOfGradeSheetInProgress = false;
                Utils.GetExcelApplication().ActiveWorkbook.SheetChange += Program.OnSheetChange;
            }
        }
    }
}