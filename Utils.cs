using AddinGrades.DTO;
using ExcelDna.Integration;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace AddinGrades
{
    public static class Utils
    {
        /// <summary>
        /// Takes in a collumn number starting at 1 and returns A, B, C etc
        /// Useful for these kind of methods     Range range = ws.get_Range("A1", GetExcelColumnName(columnNumber) + "1");
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;
            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }
            return columnName;
        }

        public static WorkbookData LoadWorkbookData(this Microsoft.Office.Interop.Excel.Application application)
        {
            foreach (var item in application.ActiveWorkbook.CustomXMLParts.Cast<CustomXMLPart>())
            {
                    string xml = item.XML;
                    if (xml.Contains("WorkbookData"))
                    {
                        return WorkbookData.Deserialize<WorkbookData>(xml); 
                    }
            }
            return null;
        }

        public static Application GetExcelApplication()
        {
            return ExcelDnaUtil.Application as Microsoft.Office.Interop.Excel.Application;
        }

        public static string? GetCurrentSheetID()
        {
            Worksheet sheet = GetExcelApplication().ActiveSheet as Worksheet;
            if (sheet.CustomProperties.Cast<CustomProperty>().Any(c => c.Name == "CustomID"))
            {
                return sheet.GetProperty("CustomID");
            }
            return null; 
        }

        public static WorkbookData IfNullCreate(this WorkbookData data)
        {
            Application app = ExcelDnaUtil.Application as Application;
            if (data is null)
            {
                app.CreateWorkbookData();
                return app.LoadWorkbookData();
            }
            return data;
        }

        public static void CreateWorkbookData(this Microsoft.Office.Interop.Excel.Application application)
        {
            application.ActiveWorkbook.CustomXMLParts.Add(WorkbookData.Serialize(new WorkbookData()));
        }

        public static string CreateCustomID(this Worksheet worksheet)
        {
            string gradeSheetID;
            worksheet.CustomProperties.Add("CustomID", gradeSheetID = Guid.NewGuid().ToString());
            return gradeSheetID;
        }

        public static string? GetCustomID(this Worksheet sheet)
        {
            if (sheet.CustomProperties.Cast<CustomProperty>().Any(c => c.Name == "CustomID"))
            {
                return sheet.GetProperty("CustomID");
            }
            return null;
        }

        public static string GetProperty(this Worksheet ws, string name)
        {
            foreach (CustomProperty cp in ws.CustomProperties)
                if (cp.Name == name)
                    return cp.Value;
            return null;
        }

        public static Worksheet? GetWorksheetById(string gradeID)
        {
            Application app = GetExcelApplication();
            foreach (Worksheet sheet in app.Worksheets)
            {
                if (sheet.GetCustomID().Equals(gradeID)) return sheet;
            }
            return null;
        }

        public static void SetProperty(this Worksheet ws, string name, string value)
        {
            bool found = false;
            CustomProperties cps = ws.CustomProperties;
            foreach (CustomProperty cp in cps)
            {
                if (cp.Name == name)
                {
                    found = true;
                    cp.Value = value;
                }
            }
            if (!found)
                cps.Add(name, value);
        }

        public static bool IsEditing(Application excelApp)
        {
            if (excelApp.Interactive)
            {
                try
                {
                    excelApp.Interactive = false;
                    excelApp.Interactive = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Please stop cell editing before you continue.");
                    return true;
                }
            }
            return false;
        }

    }
}
