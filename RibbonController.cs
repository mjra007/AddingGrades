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
using static AddinGrades.GradeTable;
using System.Text;

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
              <button id='gradeSheetButton' imageMso='MicrosoftVisualFoxPro' label='Make this worksheet a gradesheet' onAction='OnGradeSheetCreatePressed' size='large'/>
              <button id='addCoursework' imageMso='SourceControlAddObjects' label='Add/Remove coursework' onAction='OnAddCoursework' size='large'/>
              <button id='manageCourseworkWeights' imageMso='FunctionWizard' label='Manage Coursework Weights' onAction='OnManageCourseworkWeights' size='large'/>            
            </group >
            <group id ='group2' label='Utilities'>
              <button id='UnlockLock' imageMso='Lock'  label='Unlock or Lock sheet' onAction='UnlockSheet' size='large'/>  
              <button id='CopyGradeString' imageMso='Copy' label='Copy grades csv string' onAction='OnCopyGradeString' size='large'/>
              <button id='CopyFeedbackString' imageMso='Copy' label='Copy feedback csv string' onAction='OnCopyFeedbackString' size='large'/>
            </group>
          </tab>
        </tabs>
      </ribbon>
    </customUI>";
        }

        public void OnCopyGradeString(IRibbonControl control)
        {
            if (Utils.IsEditing(Utils.GetExcelApplication()))
                return;
            if(Utils.GetCurrentSheetID() is null)
            {
                Program.LoggerPanel.WriteLineToPanel("This is not a gradesheet");
                return;
            }
            if (Utils.IsFeedback())
            {
                Program.LoggerPanel.WriteLineToPanel("This is not a gradesheet");
                return;
            }
            GradeTable gradeSheet = new(Utils.GetCurrentSheetID());
            string gradeString = gradeSheet.GenerateGradeString();
            if (gradeString is not null && string.IsNullOrEmpty(gradeString) == false)
            { 
                Clipboard.SetText(gradeString);
            }
        }
        public void OnCopyFeedbackString(IRibbonControl control)
        {
            if (Utils.IsEditing(Utils.GetExcelApplication()))
                return;
            if (Utils.GetCurrentSheetID() is null)
            {
                Program.LoggerPanel.WriteLineToPanel("This is not a gradesheet");
                return;
            }
            if (Utils.IsFeedback())
            {
                Program.LoggerPanel.WriteLineToPanel("This is not a gradesheet");
                return;
            }
            GradeTable gradeSheet = new(Utils.GetCurrentSheetID());
            string gradeString = gradeSheet.GenerateFeedbackString();
            if (gradeString is not null && string.IsNullOrEmpty(gradeString) == false)
            {
                Clipboard.SetText(gradeString);
            }
        }

        public void UnlockSheet(IRibbonControl control)
        {
            if (Utils.GetCurrentSheetID() is null)
            {
                Program.LoggerPanel.WriteLineToPanel("This is not a gradesheet");
                return;
            }
            if (Utils.IsFeedback())
            {
                Program.LoggerPanel.WriteLineToPanel("This is not a gradesheet");
                return;
            }
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
            if (Utils.GetCurrentSheetID() is null)
            {
                Program.LoggerPanel.WriteLineToPanel("This is not a gradesheet");
                return;
            }
            if (Utils.IsFeedback())
            {
                Program.LoggerPanel.WriteLineToPanel("This is not a gradesheet");
                return;
            }
            ManageCourseworkWeight form = new(Utils.GetCurrentSheetID());
            form.Show();
        }

        public void OnAddCoursework(IRibbonControl control)
        {
            if (Utils.IsEditing(Utils.GetExcelApplication()))
                return;
            if (Utils.GetCurrentSheetID() is null)
            {
                Program.LoggerPanel.WriteLineToPanel("This is not a gradesheet");
                return;
            }
            if (Utils.IsFeedback())
            {
                Program.LoggerPanel.WriteLineToPanel("This is not a gradesheet");
                return;
            }
            AddCoursework form = new(Utils.GetCurrentSheetID());
            form.Show();
        }

        public void OnGradeSheetCreatePressed(IRibbonControl control)
        {
            if (Program.CreationOfGradeSheetInProgress)
                return;
            if (Utils.IsEditing(Utils.GetExcelApplication()))
                return;
            Application app = ExcelDnaUtil.Application as Application;
            if (app.ActiveWorkbook is null || app.ActiveSheet is null)
            {
                Program.LoggerPanel?.WriteLineToPanel("No active workbook or sheet was found.");
                return;
            } 
            if ((app.ActiveSheet as Worksheet).GetCustomID() is not null)
            {
                Program.LoggerPanel?.WriteLineToPanel("This worksheet is already a grade sheet!");
                return;
            }
            CreateGradeSheet form = new();
            form.Show(); 
        } 
         
    }
}
