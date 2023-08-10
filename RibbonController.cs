using ExcelDna.Integration.CustomUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using ExcelDna.Integration;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using Range = Microsoft.Office.Interop.Excel.Range;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AddinGrades
{
    [ComVisible(true)]
    public class RibbonController : ExcelRibbon
    {
        public override string GetCustomUI(string RibbonID)
        {
            return @"
      <customUI xmlns='http://schemas.microsoft.com/office/2006/01/customui'>
      <ribbon>
        <tabs>
          <tab id='tab1' label='Grades Plugin'>
            <group id='group1' label='My Group'>
              <button id='gradeSheetButton' imageMso='MicrosoftVisualFoxPro'  label='Make this worksheet a gradesheet' onAction='OnButtonPressed' size='large'/>
              <button id='addCoursework' imageMso='SourceControlAddObjects'  label='Add coursework' onAction='OnButtonPressed' size='large'/>
            </group >
          </tab>
        </tabs>
      </ribbon>
    </customUI>";
        }
         
        public void OnButtonPressed(IRibbonControl control)
        {
            CreateTable();
        } 

        List<string> defaultColumns = new List<string>()
        {
            "Aluno", "Final Grade", "Feedback", "Observations"
        };

        public void CreateTable()
        {
            Application app = ExcelDnaUtil.Application as Application;
            if (app.ActiveWorkbook is not null && app.ActiveSheet is not null)
            {
                Worksheet worksheet = app.ActiveSheet as Worksheet;

                Range currentCell = worksheet.get_Range("A2");
                foreach (string columnName in defaultColumns)
                {
                    currentCell.Cells[1] = columnName;
                    currentCell = currentCell.Offset[0, 1];
                } 
                 
                string lastColumn = Utils.GetExcelColumnName(defaultColumns.Count);
                worksheet.get_Range("A3", $"{lastColumn}100").Locked = false;
                worksheet.get_Range("A2", $"{lastColumn}2").Cells.Font.Size = 13;
                worksheet.get_Range("A2", $"{lastColumn}2").Locked = true;
                worksheet.get_Range("A2", $"{lastColumn}2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGoldenrodYellow);
                worksheet.Columns.AutoFit();
                worksheet.Columns[1].ColumnWidth = 25;
                worksheet.Protect();
            }
             

        }



    }
}
