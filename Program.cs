using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using System;
using static System.Net.WebRequestMethods;

namespace AddinGrades
{
    public class Program : IExcelAddIn
    {
        public static LoggerPanel? LoggerPanel;
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
        }
    }
}