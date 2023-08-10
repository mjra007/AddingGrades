using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using System;
using static System.Net.WebRequestMethods;

namespace AddinGrades
{
    public class Program : IExcelAddIn
    {
        static CustomTaskPane ctp;
        static void Main(string[] args)
        {
        }

        public void AutoClose()
        { 
        }
        public void AutoOpen()
        {

            var ctp = CustomTaskPaneFactory.CreateCustomTaskPane(typeof(CustomPanel), "Grades addin console");
            ctp.Visible = true;
            ctp.DockPosition = MsoCTPDockPosition.msoCTPDockPositionTop;
            ctp.Height = 200;
        }
    }
}