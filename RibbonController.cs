using ExcelDna.Integration.CustomUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices; 
using ExcelDna.Integration;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using Range = Microsoft.Office.Interop.Excel.Range; 
using AddinGrades.DTO; 

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
            <group id='group1' label='GradeSheet Controls'>
              <button id='gradeSheetButton' imageMso='MicrosoftVisualFoxPro'  label='Make this worksheet a gradesheet' onAction='OnGradeSheetCreatePressed' size='large'/>
              <button id='addCoursework' imageMso='SourceControlAddObjects'  label='Add/Remove coursework' onAction='OnAddCoursework' size='large'/>
              <button id='manageCourseworkWeights' imageMso='FunctionWizard'  label='Manage Coursework Weights' onAction='OnManageCourseworkWeights' size='large'/>            
            </group >
            <group id ='group2' label='Utilities'>
              <button id='UnlockLock' imageMso='Lock'  label='Unlock or Lock sheet' onAction='UnlockSheet' size='large'/>  
              <button id='CopyGradeString' imageMso='Copy' label='Copy grades csv string' onAction='OnCopyGradeString' size='large'/>
            </group>
          </tab>
        </tabs>
      </ribbon>
    </customUI>";
        }
        
        public void OnCopyGradeString(IRibbonControl control)
        {
            var sheet = Utils.GetWorksheetById(Utils.GetCurrentSheetID());
        }


        public void UnlockSheet(IRibbonControl control)
        {
            var sheet = Utils.GetWorksheetById(Utils.GetCurrentSheetID());
            if (sheet.ProtectContents)
            {
                sheet.Unprotect();
            }
            else
            {
                sheet.Protect();
            }
        }

        public void OnManageCourseworkWeights(IRibbonControl control)
        {
            if (Utils.IsEditing(Utils.GetExcelApplication()))
                return;
            ManageCourseworkWeight form = new(Utils.GetCurrentSheetID());
            form.Show();
        }

        public void OnAddCoursework(IRibbonControl control)
        {
            if (Utils.IsEditing(Utils.GetExcelApplication()))
                return;
            AddCoursework form = new(Utils.GetCurrentSheetID()); 
            form.Show();
        }

        public void OnGradeSheetCreatePressed(IRibbonControl control)
        {
            Program.CreationOfGradeSheetInProgress = true;
            if (Utils.IsEditing(Utils.GetExcelApplication()))
                return;
            Application app = ExcelDnaUtil.Application as Application;
            if (app.ActiveWorkbook is not null && app.ActiveSheet is not null && (app.ActiveSheet as Worksheet).GetCustomID() is null)
            {
                WorkbookData data = app.LoadWorkbookData().IfNullCreate();
                //Add custom ID to gradeSheet if not created already
                Worksheet worksheet = app.ActiveSheet as Worksheet;
                string gradeSheetID = worksheet.GetCustomID();
                gradeSheetID ??= worksheet.CreateCustomID();
                //create gradesheet in workbookdata
                GradeSheet gradeSheet = new();
                gradeSheet.CourseworkWeightedTables.Add(new CourseworkWeightedTable("Default", gradeSheet.Coursework, 
                    Enumerable.Repeat(0d,gradeSheet.Coursework.Count).ToArray()));
                data.GradeSheets.Add(gradeSheetID, gradeSheet);
                data.Save(); 
                new GradeTable(gradeSheetID).CreateDefaultTable(data, app);
            }
            else
            {
                Program.LoggerPanel?.WriteLineToPanel("No active workbook or sheet was found.\nPlease create a new excel file before attempting to create a grade sheet.");
            } 
            Program.CreationOfGradeSheetInProgress = false;
        }



       



    }
}
