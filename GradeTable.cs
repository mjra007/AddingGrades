﻿using AddinGrades.DTO;
using ExcelDna.Integration;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualStudio.Services.Common; 
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Application = Microsoft.Office.Interop.Excel.Application;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace AddinGrades
{
    public class GradeTable
    {
        [Flags]
        internal enum XlType12 : uint
        {
            XlTypeNumber = 0x0001,
            XlTypeString = 0x0002,
            XlTypeBoolean = 0x0004,
            XlTypeReference = 0x0008,
            XlTypeError = 0x0010,
            XlTypeArray = 0x0040,
            XlTypeMissing = 0x0080,
            XlTypeEmpty = 0x0100,
            XlTypeInt = 0x0800,     // int16 in XlOper, int32 in XlOper12, never passed into UDF
        }

        public static class CollumnName
        {
            public const string Student = "Aluno";
            public const string Knowledge = "Knowledge";
            public const string Atitudes = "Atitudes";
            public const string CourseworkWeigthtedTable = "Weighted Table";
            public const string FinalGrade = "Final Grade";
            public const string Feedback = "Feedback";
            public const string Observations = "Observations";

        }

        static readonly List<string> DefaultColumns = new()
        {
            CollumnName.Student,
            CollumnName.Knowledge,
            CollumnName.Atitudes,
            CollumnName.FinalGrade, 
            CollumnName.Feedback, 
            CollumnName.Observations,
            CollumnName.CourseworkWeigthtedTable,
        };

        public readonly string GradeSheetID;

        public GradeTable(string gradeSheetID)
        {
            GradeSheetID = gradeSheetID;
        }

        public void CreateDefaultTable(Worksheet worksheet, WorkbookData data, Application app, IEnumerable<string> studentNames)
        { 
            //Create feedback sheet if needed
            if (data.FeedbackSheetID is null)
            {
                Worksheet feedbackWorksheet = app.ActiveWorkbook.Worksheets.Add();
                data.FeedbackSheetID = feedbackWorksheet.CreateCustomID(true);
                data.Save();
                FeedbackTable feedbackTable = new(data.FeedbackSheetID);
                feedbackTable.CreateTable(studentNames);
            }
            else
            {
                Worksheet? feedbackWorksheet = Utils.GetFeedbackSheet();
                if (feedbackWorksheet is not null)
                {
                    string finalGradeColumn = Utils.GetExcelColumnName(GetLastHeaderCollumnWithValue() + 1);
                    string feedbackColumn = Utils.GetExcelColumnName(GetLastHeaderCollumnWithValue() + 2);
                    Range finalGradeCell = feedbackWorksheet.get_Range($"{finalGradeColumn}1");
                    finalGradeCell.Value = "Final Grade";
                    Range feedbackCell = feedbackWorksheet.get_Range($"{feedbackColumn}1");
                    feedbackCell.Value = $"=GetSheetName(\"{GradeSheetID}\")";

                    feedbackCell.ColumnWidth = 25;

                    foreach (string name in studentNames)
                    {
                        finalGradeCell = finalGradeCell.Offset[1, 0];
                        finalGradeCell.Formula =
                            $"=GetFinalGrade({feedbackColumn}{feedbackWorksheet.get_Range($"{feedbackColumn}1").Row},A{finalGradeCell.Row})";
                    }
                }
            }

            worksheet.Select();
            Range currentCell = worksheet.get_Range("A2");

            foreach (string columnName in DefaultColumns)
            {
                currentCell.Cells[1] = columnName;
                currentCell = currentCell.Offset[0, 1];
            }

            string lastColumn = Utils.GetExcelColumnName(DefaultColumns.Count); 

            worksheet.get_Range("A2", $"{lastColumn}2").Cells.Font.Size = 13;
            worksheet.get_Range("A2", $"{lastColumn}2").Interior.Color = ColorTranslator.ToOle(Color.LightGoldenrodYellow);
            LockCollumnsAndHeaders();

            currentCell = worksheet.get_Range("A3");
            foreach (string name in studentNames)
            {
                currentCell.Cells[1] = name;
                currentCell = currentCell.Offset[1, 0];  
            }
            worksheet.Columns.AutoFit();
            if (!studentNames.Any())
            { 
                worksheet.Columns[1].ColumnWidth = 25;
            }
            if(studentNames.Any())
            { 
                Program.InsertStudentGradeFormulas(worksheet.get_Range("A3", $"A{3 + studentNames.Count() - 1}")); 
            }
            
            worksheet.Protect();
        }

        public void LockCollumnsAndHeaders()
        {
            LockCollumnsAndHeaders(Utils.GetWorksheetById(GradeSheetID));
        }

        public static void LockCollumnsAndHeaders(Worksheet worksheet)
        { 
            string lastColumn = Utils.GetExcelColumnName(DefaultColumns.Count);
            string feedbackColumnName = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(worksheet,CollumnName.Feedback) + 1);
            string knowledgeCollumnName = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(worksheet, CollumnName.Knowledge) + 1);
            string finalCollumnName = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(worksheet,CollumnName.FinalGrade) + 1);

            //we dont want to change the protection status so if it is protected unprotect and then protect again
            bool protectAtTheEnd = false;
            if (worksheet.ProtectContents)
            {
                protectAtTheEnd = true;
                worksheet.Unprotect();
            }
            //Unlock all the cells
            worksheet.get_Range("A1", $"{lastColumn}100").Locked = false;
            //Lock headers first
            worksheet.get_Range("A2", $"{lastColumn}2").Locked = true;
            worksheet.get_Range($"{feedbackColumnName}3", $"{feedbackColumnName}100").Locked = true; 
            worksheet.get_Range($"{knowledgeCollumnName}3", $"{knowledgeCollumnName}100").Locked = true; 
            worksheet.get_Range($"{finalCollumnName}3", $"{finalCollumnName}100").Locked = true; 
            worksheet.Protect();
            if (protectAtTheEnd)
            {
                worksheet.Protect();
            }
        }

        [ExcelFunction()]
        public static double CalculateKnowledge(
             [ExcelArgument(AllowReference = true, Name = "courseworkValue")] object courseworkGradesRange,
             [ExcelArgument(AllowReference = true, Name = "courseworkName")] object courseworkNameRange,
             [ExcelArgument(AllowReference = false, Name = "tableRange")] object table)
        {
            IEnumerable<object> courseworkGrades = ((object[,])((ExcelReference)courseworkGradesRange).GetValue()).Cast<object>();
            IEnumerable<string> courseworkNames = ((object[,])((ExcelReference)courseworkNameRange).GetValue()).Cast<string>();
            string? tableWeightsName = table as string;


            Dictionary<string, object> courseworkGradesZipped = courseworkNames.Zip(courseworkGrades, (key, value) => new { key, value }).ToDictionary(x => x.key, x => x.value);

            var app = Utils.GetExcelApplication().LoadWorkbookData().GradeSheets[Utils.GetCurrentSheetID()];
            if(string.IsNullOrEmpty(tableWeightsName) == false  && app.GetWeightedTable(tableWeightsName).IsSuccess)
            {
                SerializableDictionary<Coursework, double> tableWeights = app.GetWeightedTable(tableWeightsName).Object.weights;

                double knowledge = 0d;
                foreach ((string courseworkName, object grade) in courseworkGradesZipped)
                {
                    if (grade is double gradeDouble)
                    {
                        knowledge += Math.Round(gradeDouble, MidpointRounding.AwayFromZero) * tableWeights[app.GetCoursework(courseworkName).Object];
                    }
                }
                return Math.Round(knowledge, 0, MidpointRounding.AwayFromZero);
            }
            return -1;
        }

        [ExcelFunction()]
        public static double CalculateFinalGrade(
             [ExcelArgument(AllowReference = true, Name = "knowledge")] object knowledge,
             [ExcelArgument(AllowReference = true, Name = "atitudes")] object atitude)
        {
            double? knowledgeGradeNullable = ((ExcelReference)knowledge).GetValue() as double?;
            double? atitudesGradeNullable = ((ExcelReference)atitude).GetValue() as double?;
            double knowledgeGrade = knowledgeGradeNullable.HasValue ? knowledgeGradeNullable.Value : 0;
            double atitudesGrade = atitudesGradeNullable.HasValue ? atitudesGradeNullable.Value : 0;
            return Math.Round(Math.Round(knowledgeGrade * 0.85 + Math.Round(atitudesGrade,0) * 0.15, 1, MidpointRounding.ToZero),0, MidpointRounding.AwayFromZero);
        }

        public static string cacheFeedbackSheetID;
        [ExcelFunction( IsMacroType = true )]
        public static string GrabFeedbackFor([ExcelArgument(AllowReference = true, Name = "studentName")] object studentName)
        {
            string studentNameString = ((ExcelReference)studentName).GetValue() as string;
            string sheetName = ((string)XlCall.Excel(XlCall.xlSheetNm, studentName, (int)XlType12.XlTypeString)).Split(']')[1];
            cacheFeedbackSheetID ??= Utils.GetExcelApplication().LoadWorkbookData().FeedbackSheetID; 

            int? rowIndex = Utils.GetRowByNameIndex(cacheFeedbackSheetID, studentNameString, "A");
            if (rowIndex == null) return string.Empty;

            string courseworkFeedbackCollumn = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(cacheFeedbackSheetID, sheetName, "A1")+1);
            dynamic value = Utils.GetWorksheetById(cacheFeedbackSheetID).get_Range($"{courseworkFeedbackCollumn}{rowIndex+1}").Value2;
            if (value is not null)
            {
                return value.ToString();
            }
            else
            {
                return string.Empty;
            } 
        }

        [ExcelFunction()]
        public static string GetSheetName([ExcelArgument( Name = "sheetID")] object sheetID)
        {
            string? sheetIDString = sheetID as string;
            if (string.IsNullOrEmpty(sheetIDString) == false)
            {
                Worksheet sheet = Utils.GetWorksheetById(sheetIDString);
                if(sheet is not null) return sheet.Name;
            }
            return string.Empty;
        }

        [ExcelFunction(IsMacroType =true)]
        public static string GetFinalGrade([ExcelArgument(AllowReference = true, Name = "sheetName")] object sheetName,
            [ExcelArgument(AllowReference = true, Name = "studentName")] object studentName)
        {
            string studentNameString = ((ExcelReference)studentName).GetValue() as string;
            string sheetNameString = ((ExcelReference)sheetName).GetValue() as string;

            foreach (Worksheet sheet in Utils.GetExcelApplication().Worksheets)
            {
                if (sheet.Name.Equals(sheetNameString) && string.IsNullOrEmpty(studentNameString) == false)
                {
                    int? rowIndex = Utils.GetRowByNameIndex(sheet, studentNameString, "A");
                    if (rowIndex.HasValue)
                    {
                        string finalGradeCollumnName = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(sheet, CollumnName.FinalGrade) + 1);
                        Range gradeCell = sheet.get_Range($"{finalGradeCollumnName}{rowIndex+1}");
                        if(gradeCell.Value is not null)
                        {
                            return gradeCell.Value.ToString();
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public void InsertFinalGrade(int row)
        {
            string finalGradeCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.FinalGrade) + 1);
            string knowledgeCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.Knowledge) + 1);
            string atitudesCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.Atitudes) + 1);
            Worksheet sheet = Utils.GetWorksheetById(GradeSheetID);
            sheet.Unprotect();
            sheet.get_Range($"{finalGradeCollumnName}{row}").Formula=$"=CalculateFinalGrade({knowledgeCollumnName}{row}, {atitudesCollumnName}{row})";
            sheet.Protect();
        }

        public string GetFinalGradeForRow(int row)
        {
            string finalGradeCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.FinalGrade) + 1);
            string knowledgeCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.Knowledge) + 1);
            string atitudesCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.Atitudes) + 1);
            return $"=CalculateFinalGrade({knowledgeCollumnName}{row}, {atitudesCollumnName}{row})";
        }

        public void InsertFinalGradeAllRows()
        {
            int finalGradeCollumnNumber = GetCollumnByNameIndex(CollumnName.FinalGrade) + 1;
            ExcelReference range = new(3, GradeTable.FindLastStudentRow(), finalGradeCollumnNumber, finalGradeCollumnNumber);
            var cells = Utils.GetWorksheetById(GradeSheetID).Cells as Range;
            cells.Worksheet.Unprotect();
            for (int i = range.RowFirst; i < range.RowLast; i++)
            {
                (cells[i, range.ColumnFirst] as Range).Formula = GetFinalGradeForRow(i);
            }
            cells.Worksheet.Protect();
        }

        public void InsertKnowledgeFunction(int row)
        { 
            string knowledgeCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.Knowledge) + 1);
            string courseworkWeightedTableName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.CourseworkWeigthtedTable) + 1);
            string lastCourseworkCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.Knowledge));
            string firstCourseworkCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.Student) + 2);
            if (firstCourseworkCollumnName == knowledgeCollumnName) return;
            Worksheet sheet = Utils.GetWorksheetById(GradeSheetID);
            sheet.Unprotect();
            sheet.get_Range($"{knowledgeCollumnName}{row}").Formula = $"=CalculateKnowledge({firstCourseworkCollumnName}{row}:{lastCourseworkCollumnName}{row},{firstCourseworkCollumnName}2:{lastCourseworkCollumnName}2, {courseworkWeightedTableName}{row})";
            sheet.Protect();
        }

        public string GetKnowledgeFunctionForRow(int row)
        {
            string knowledgeCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.Knowledge) + 1);
            string courseworkWeightedTableName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.CourseworkWeigthtedTable) + 1);
            string lastCourseworkCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.Knowledge));
            string firstCourseworkCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.Student) + 2);
            return $"=CalculateKnowledge({firstCourseworkCollumnName}{row}:{lastCourseworkCollumnName}{row},{firstCourseworkCollumnName}2:{lastCourseworkCollumnName}2, {courseworkWeightedTableName}{row})";
        }

        public void InsertKnowledgeFunctionForRows()
        {
            int knowledgeCollumnNumber = GetCollumnByNameIndex(CollumnName.Knowledge) + 1;
            ExcelReference range = new(3, GradeTable.FindLastStudentRow(), knowledgeCollumnNumber, knowledgeCollumnNumber);
            var cells = Utils.GetWorksheetById(GradeSheetID).Cells as Range;
            cells.Worksheet.Unprotect();
            for (int i = range.RowFirst; i < range.RowLast; i++)
            {
                (cells[i, range.ColumnFirst] as Range).Formula = GetKnowledgeFunctionForRow(i);
            }
            cells.Worksheet.Protect();
        }

        public void InsertFeedback(int row)
        { 
            string feedbackCollumnName = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(GradeSheetID, CollumnName.Feedback) + 1);
            Worksheet sheet = Utils.GetWorksheetById(GradeSheetID);
            sheet.Unprotect();
            sheet.get_Range($"{feedbackCollumnName}{row}").Formula = $"=GrabFeedbackFor(A{row})";
            sheet.Protect();
        }
        public void InsertDropdownForWeightedTable(int row)
        {
            Worksheet sheet = Utils.GetWorksheetById(GradeSheetID);
            string collumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.CourseworkWeigthtedTable) + 1);
            sheet.Unprotect();
            Range cell = sheet.get_Range(Cell1: $"{collumnName}{row}");
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
            cell.Validation.Delete();
            cell.Validation.Add(
               XlDVType.xlValidateList,
               XlDVAlertStyle.xlValidAlertInformation,
               XlFormatConditionOperator.xlBetween,
               string.Join(',', data.GradeSheets[GradeSheetID].CourseworkWeightedTables.Select(s => s.name)),
               Type.Missing);
            cell.Validation.IgnoreBlank = true;
            cell.Validation.InCellDropdown = true;
            cell.Value2 = data.GradeSheets[GradeSheetID].CourseworkWeightedTables.First().name;
            sheet.Protect();
        }


        public static int FindLastStudentRow()
        {
            Worksheet sheet = Utils.GetExcelApplication().ActiveSheet;
            Range cell = sheet.get_Range($"A3");
            int lastRow = 3;
            while (string.IsNullOrEmpty(cell.Value2 as string) is false)
            {
                lastRow++;
                cell = cell.Offset[1, 0];
            }

            return lastRow;
        }

        public string CreateGradeString()
        {//todo
            StringBuilder stringBuilder= new StringBuilder();
            Worksheet sheet = Utils.GetWorksheetById(GradeSheetID);
            int weightedTableCollumn = GetCollumnByNameIndex(CollumnName.CourseworkWeigthtedTable) + 1;
            string collumnName = Utils.GetExcelColumnName(weightedTableCollumn); 
            Range cell = sheet.get_Range($"{collumnName}3");
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
            for (int i = 3; i < FindLastStudentRow(); i++)
            {
                cell.Validation.Delete();
                cell.Validation.Add(
                   XlDVType.xlValidateList,
                   XlDVAlertStyle.xlValidAlertInformation,
                   XlFormatConditionOperator.xlBetween,
                   string.Join(',', data.GradeSheets[GradeSheetID].CourseworkWeightedTables.Select(s => s.name)),
                   Type.Missing);
                cell.Validation.IgnoreBlank = true;
                cell.Validation.InCellDropdown = true;
                cell.Value2 = data.GradeSheets[GradeSheetID].CourseworkWeightedTables.First().name;
                cell = cell.Offset[1, 0];
            } 
            return stringBuilder.ToString();
        }

        public void InsertDropdownForWeightedTable()
        {
            Worksheet sheet = Utils.GetWorksheetById(GradeSheetID);
            int weightedTableCollumn = GetCollumnByNameIndex(CollumnName.CourseworkWeigthtedTable) + 1;
            string collumnName = Utils.GetExcelColumnName(weightedTableCollumn);
            sheet.Unprotect();
            Range cell = sheet.get_Range($"{collumnName}3");
            WorkbookData data = Utils.GetExcelApplication().LoadWorkbookData();
            for (int i = 3; i < FindLastStudentRow(); i++)
            {
                cell.Validation.Delete();
                cell.Validation.Add(
                   XlDVType.xlValidateList,
                   XlDVAlertStyle.xlValidAlertInformation,
                   XlFormatConditionOperator.xlBetween,
                   string.Join(',', data.GradeSheets[GradeSheetID].CourseworkWeightedTables.Select(s => s.name)),
                   Type.Missing);
                cell.Validation.IgnoreBlank = true;
                cell.Validation.InCellDropdown = true;
                cell.Value2 = data.GradeSheets[GradeSheetID].CourseworkWeightedTables.First().name;
                cell = cell.Offset[1, 0];
            }
            sheet.Protect();
        }

        public void InsertNewCoursework(params Coursework[] courseworks)
        {
            Worksheet sheet = Utils.GetWorksheetById(GradeSheetID);
            sheet.Unprotect();
            int insertCollumn = GetLastCourseworkCollumn() + 1;

            foreach (Coursework coursework in courseworks)
            {
                Range collumnNameCell = sheet.Range[$"{Utils.GetExcelColumnName(insertCollumn)}2"];
                collumnNameCell.EntireColumn.Insert(XlInsertShiftDirection.xlShiftToRight, XlInsertFormatOrigin.xlFormatFromRightOrBelow);
                //reference needs to be updated so it gets the new inserted cell instead of the moved one
                collumnNameCell = sheet.Range[$"{Utils.GetExcelColumnName(insertCollumn)}2"];
                collumnNameCell.Value2 = coursework.Name;
                insertCollumn++;
            }
            sheet.Columns.AutoFit();
            sheet.Protect();
        }

        public void DeleteCourseworkCollumn(string name)
        {
            Worksheet worksheet = Utils.GetWorksheetById(GradeSheetID);
            worksheet.Unprotect(); 
            string collumnLetter = Utils.GetExcelColumnName(GetCourseworkIndex(name) + 1);
            worksheet.Columns[collumnLetter].Delete(); 
            worksheet.Protect();
        }

        public int GetLastCourseworkCollumn()
        {
            Worksheet worksheet = Utils.GetWorksheetById(GradeSheetID);
            Range currentCell = worksheet.get_Range("A2");
            int counter = 0;
            //Looks for the collumn before knowledge. All coursework collumns should be between Aluno and Knowledge
            while (currentCell.Value2 != CollumnName.Knowledge)
            {
                currentCell = currentCell.Offset[0, 1];
                counter++;
            };
            return counter;
        }

        public int GetCourseworkIndex(string courseworkName)
        {
            Worksheet worksheet = Utils.GetWorksheetById(GradeSheetID);
            Range currentCell = worksheet.get_Range("A2");
            int counter = 0;

            while (currentCell.Value2.ToString().Equals(courseworkName) is false)
            {
                string c = currentCell.Value2;
                currentCell = currentCell.Offset[0, 1];
                counter++;
            };
            return counter;
        }


        public int GetCollumnByNameIndex(string name)
        {
            return GetCourseworkIndex(name);
        }

          
        public static int GetLastHeaderCollumnWithValue(string firstCell = "A1")
        {
            Worksheet worksheet = Utils.GetFeedbackSheet();
            Range currentCell = worksheet.get_Range(firstCell);
            int counter = 0;

            while (currentCell.Value2 is not null)
            {
                string c = currentCell.Value2;
                currentCell = currentCell.Offset[0, 1];
                counter++;
            };
            return counter;
        }

        public static void RunMacro(params object[] args)
        {
            object xlApp = ExcelDnaUtil.Application;
            try
            {
                xlApp.GetType().InvokeMember("Run",
                BindingFlags.InvokeMethod,
                null, xlApp, args);
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
            finally
            {
                Marshal.ReleaseComObject(xlApp);
            }
        }

        public string GenerateGradeString()
        {
            Application app = ExcelDnaUtil.Application as Application;
            Worksheet worksheet = app.ActiveSheet as Worksheet;
            Range currentCell = worksheet.get_Range("A3");
            StringBuilder sb = new();
            string knowledgeColumn = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.Knowledge)+1); 
            string finalGradeColumn = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.FinalGrade)+1); 
            string atitudesColumn = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.Atitudes)+1);
            for (int i = 3; i < FindLastStudentRow(); i++)
            {
                if (string.IsNullOrEmpty(currentCell.Value2) is false)
                {
                    Range atitudesRange = worksheet.get_Range($"{atitudesColumn}{i}");
                    Range knowledgeRange = worksheet.get_Range($"{knowledgeColumn}{i}");
                    Range finalGradeRange = worksheet.get_Range($"{finalGradeColumn}{i}");
                    string atitudesValue = atitudesRange.Value2 is null ? string.Empty : atitudesRange.Value2.ToString();
                    string knowledgeValue = knowledgeRange.Value2 is null ? string.Empty : knowledgeRange.Value2.ToString();
                    string finalGradeValue = finalGradeRange.Value2 is null ? string.Empty : finalGradeRange.Value2.ToString();

                    sb.AppendLine($"{currentCell.Value2.ToString().Trim()},{atitudesValue},{atitudesValue},{atitudesValue},{knowledgeValue},{knowledgeValue},{knowledgeValue},{knowledgeValue},{ knowledgeValue},{knowledgeValue},{finalGradeValue}");
                }
                currentCell = currentCell.Offset[1, 0];
            }
            return sb.ToString();
        }

        public string GenerateFeedbackString()
        {
            Application app = ExcelDnaUtil.Application as Application;
            Worksheet worksheet = app.ActiveSheet as Worksheet;
            Range currentCell = worksheet.get_Range("A3");
            StringBuilder sb = new();
            string feedbackColumn = Utils.GetExcelColumnName(GetCollumnByNameIndex(CollumnName.Feedback)+1);
            for (int i = 3; i < FindLastStudentRow(); i++)
            {
                if (string.IsNullOrEmpty(currentCell.Value2) is false)
                {
                    Range feedbackRange = worksheet.get_Range($"{feedbackColumn}{i}");
                    if(feedbackRange.Value is not null)
                    { 
                        sb.AppendLine($"{currentCell.Value2.ToString().Trim()}|{feedbackRange.Value2.ToString()}");
                    }
                }
                currentCell = currentCell.Offset[1, 0];
            }
            return sb.ToString();
        }

        //public void RecalculateGrades()
        //{
        //    Worksheet sheet = Utils.GetWorksheetById(GradeSheetID);
        //    sheet.Unprotect();
        //    string alunoCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex("Aluno") + 1);
        //    string knowledgeCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex("Knowledge") + 1);
        //    string weightedTableCollumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex("Coursework Weight Table") + 1);
        //    int firstRow = 3;

        //    Range cellTable = sheet.get_Range($"{weightedTableCollumnName}{firstRow}");
        //    var gradeSheet = Utils.GetExcelApplication().LoadWorkbookData().GradeSheets[GradeSheetID];

        //    int currentRow = firstRow;
        //    Range cellAluno = sheet.get_Range($"{alunoCollumnName}{currentRow}");
        //    while (string.IsNullOrEmpty(cellAluno.Value2) == false)
        //    {
        //        Range cellKnowledge = sheet.get_Range($"{knowledgeCollumnName}{currentRow}");
        //        CourseworkWeightedTable table = gradeSheet.GetWeightedTable(cellTable.Value2).Object;
        //        double knowledge = 0d;
        //        foreach ((Coursework coursework, double weight) in table.weights)
        //        {
        //            string collumnName = Utils.GetExcelColumnName(GetCollumnByNameIndex(coursework.Name) + 1);
        //            double courseworkGrade = sheet.get_Range($"{collumnName}{currentRow}").Value2;
        //            knowledge += Math.Round(courseworkGrade) * weight;
        //        }
        //        cellKnowledge.Value2 = Math.Round(knowledge);

        //        currentRow++;
        //        cellAluno = sheet.get_Range($"{alunoCollumnName}{currentRow}");
        //    }
        //    sheet.Protect();
        //}
    }
}
