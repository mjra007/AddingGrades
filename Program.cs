using AddinGrades.DTO;
using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace AddinGrades
{
    public class Program : IExcelAddIn
    {
        public static LoggerPanel? LoggerPanel;
        public static bool CreationOfGradeSheetInProgress = false;
        public static string Version = "v1.1";

        //Cache stuff that should be its own class tbh
        public static string CacheFileName = Path.Combine(Assembly.GetExecutingAssembly().Location, "StudentCache.xml");
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
            Utils.GetExcelApplication().WorkbookActivate += OpenWorkbook;
            Utils.GetExcelApplication().WorkbookBeforeClose += BeforeCloseWorkbook;
            if (File.Exists(CacheFileName))
            {
                StudentsCache = WorkbookData.Deserialize<StudentsCache>(File.ReadAllText(CacheFileName));
            } 
        }

        private void BeforeCloseWorkbook(Workbook Wb, ref bool Cancel)
        {
            Utils.GetExcelApplication().ActiveWorkbook.SheetChange -= OnSheetChange;
        }

        private void OpenWorkbook(Workbook Wb)
        {
            WorkbookData data = Utils.LoadWorkbookData(Utils.GetExcelApplication());
            if (data is not null)
            {
                Patcher.UpdateWorkbook(data.Version);
            }
            Utils.GetExcelApplication().ActiveWorkbook.SheetChange += OnSheetChange;
        }

        public static void OnSheetChange(object Sh, Range Target)
        { 
            if (Utils.IsFeedback() is false && Utils.GetCurrentSheetID() != null && CreationOfGradeSheetInProgress is false &&
                Target.Column == 1 && Target.Row == 1)//This is a change in the alunos column
            {
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
                GradeTable table = new GradeTable(Utils.GetCurrentSheetID());
                table.InsertKnowledgeFunction(Target.Row);
                table.InsertDropdownForWeightedTable(Target.Row);
                table.InsertFinalGrade(Target.Row); 
                table.InsertFeedback(Target.Row);
            }
        }
    }
}