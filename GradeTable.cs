using AddinGrades.DTO;
using ExcelDna.Integration;
using Jint;
using Jint.Native;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
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
        public static readonly JsValue JSPredictedGrade = new Engine()
                         .Execute(@"
function CalculatePredictedGrade(atitude, knowledge) {
var finalGrade = atitude * 0.05 + atitude * 0.05 + atitude * 0.05 + knowledge * 0.1 +  knowledge * 0.1 + knowledge * 0.1 + knowledge * 0.2 + knowledge * 0.2+  knowledge * 0.15;
return finalGrade.toFixed(1);  
}")
                         .GetValue("CalculatePredictedGrade");
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
            public const string Feedback = "Sínteses";
            public const string Observations = "Observations";
            public const string StudentImages = " ";
            public const string StudentNumber = " ";

        }

        static readonly List<string> DefaultColumns = new()
        {
            //CollumnName.StudentNumber,
            CollumnName.StudentImages,
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

        public void CreateDefaultTable(Worksheet worksheet, WorkbookData data, Application app, IEnumerable<string> studentNames, string className)
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
  

            currentCell = worksheet.get_Range("C3");
            foreach (string name in studentNames)
            {
                currentCell.Cells[1] = name;
                currentCell = currentCell.Offset[1, 0];  
            }
            worksheet.Columns.AutoFit();

            currentCell  = worksheet.get_Range("A3");

            double rColRow = 6; // Ratio of units of measure: columns widths to row heights
            double rImgColWidth = 5.9; // Ratio of units of measure: image size and column widths

            double lastMaxWidth = 0d;
            foreach (string name in studentNames)
            {
                if (File.Exists(Path.Combine(Program.ExcelAddinPathDir, "StudentImages", $"{className}-{name}.png")))
                {
                    Pictures excelPictures = (Pictures)worksheet.Pictures(Type.Missing);
                    Picture excelPicture = excelPictures.Insert(Path.Combine(Program.ExcelAddinPathDir, "StudentImages", $"{className}-{name}.png"));
                    excelPicture.Top = currentCell.Top;
                    excelPicture.Left = currentCell.Left;
                    if (excelPicture.Width > lastMaxWidth)
                    {
                        currentCell.ColumnWidth = (excelPicture.Width / rImgColWidth);
                        lastMaxWidth = excelPicture.Width;
                    }
                    currentCell.RowHeight = excelPicture.Height;
                    currentCell = currentCell.Offset[1, 0];
                }
            }

            if (!studentNames.Any())
            { 
                worksheet.Columns[2].ColumnWidth = 25;
            }
            if(studentNames.Any())
            { 
                Program.InsertStudentGradeFormulas(worksheet.get_Range("A3", $"A{3 + studentNames.Count() - 1}")); 
            } 

            LockCollumnsAndHeaders();
            ApplyStyles();
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
             
            using(Unprotecter unprotecter = new(worksheet))
            {
                //Unlock all the cells
                worksheet.get_Range("A1", $"{lastColumn}100").Locked = false;
                //Lock headers first
                worksheet.get_Range("A2", $"{lastColumn}2").Locked = true;
                worksheet.get_Range($"{feedbackColumnName}3", $"{feedbackColumnName}100").Locked = true;
                worksheet.get_Range($"{knowledgeCollumnName}3", $"{knowledgeCollumnName}100").Locked = true;
                worksheet.get_Range($"{finalCollumnName}3", $"{finalCollumnName}100").Locked = true;

            } 
        }

        public void ApplyStyles()
        {
            ApplyStyles( Utils.GetWorksheetById(GradeSheetID));
        }

        public static void ApplyStyles(Worksheet worksheet)
        {
            Application app = Utils.GetExcelApplication();
            WorkbookData data = Utils.LoadWorkbookData(app);
            var sheetID = Utils.GetCustomID(worksheet);
            bool protectAtTheEnd = false;
            if (worksheet.ProtectContents)
            {
                protectAtTheEnd = true;
                worksheet.Unprotect();
            }
            if (string.IsNullOrEmpty(sheetID) is false)
            {
                Range range;
                //Style the coursework collumns
                for (int i = 0; i < data.GradeSheets[sheetID].Coursework.Count; i++)
                {
                    Coursework? item = data.GradeSheets[sheetID].Coursework[i];
                    string courseworkColumnName = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(worksheet, item.Name)+1);
                    range = worksheet.get_Range($"{courseworkColumnName}3", $"{courseworkColumnName}100");
                    range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    range.VerticalAlignment = XlVAlign.xlVAlignCenter;
                }

                //Style the final grade, knowledge and feedback
                string studentsColumnName  = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(worksheet, CollumnName.Student) + 1);
                string feedbackColumnName = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(worksheet, CollumnName.Feedback) + 1);
                string knowledgeCollumnName = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(worksheet, CollumnName.Knowledge) + 1);
                string finalCollumnName = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(worksheet, CollumnName.FinalGrade) + 1);
                string atitudesCollumnName = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(worksheet, CollumnName.Atitudes) + 1); 
                string weightedTableCollumnName = Utils.GetExcelColumnName(Utils.GetCollumnByNameIndex(worksheet, CollumnName.CourseworkWeigthtedTable) + 1);
                range = worksheet.get_Range($"{feedbackColumnName}3", $"{feedbackColumnName}100");
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range = worksheet.get_Range($"{knowledgeCollumnName}3", $"{knowledgeCollumnName}100");
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range = worksheet.get_Range($"{finalCollumnName}3", $"{finalCollumnName}100");
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range = worksheet.get_Range($"{atitudesCollumnName}3", $"{atitudesCollumnName}100");
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range = worksheet.get_Range($"{weightedTableCollumnName}3", $"{weightedTableCollumnName}100");
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range = worksheet.get_Range($"{studentsColumnName}3", $"{studentsColumnName}100");
                range.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                range.VerticalAlignment = XlVAlign.xlVAlignCenter;
            }
            if (protectAtTheEnd) 
                worksheet.Protect(); 
        }

        [ExcelFunction()]
        public static double CalculateKnowledge(
             [ExcelArgument(AllowReference = true, Name = "courseworkValue")] object courseworkGradesRange,
             [ExcelArgument(AllowReference = true, Name = "courseworkName")] object courseworkNameRange,
             [ExcelArgument(AllowReference = false, Name = "tableRange")] object table)
        {
            IEnumerable<object> courseworkGrades = GetExcelReferenceAsCollection<object>(courseworkGradesRange);
            IEnumerable<string> courseworkNames = GetExcelReferenceAsCollection<string>(courseworkNameRange);
            string? tableWeightsName = table as string;


            Dictionary<string, object> courseworkGradesZipped = courseworkNames.Zip(courseworkGrades, (key, value) => new { key, value }).ToDictionary(x => x.key, x => x.value);
  
            Worksheet sheet = Utils.GetSheetByExcelReference((ExcelReference)courseworkGradesRange);
            var app = Utils.GetExcelApplication().LoadWorkbookData().GradeSheets[sheet.GetCustomID()];
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

        public static IEnumerable<T> GetExcelReferenceAsCollection<T>(object reference)
        {
            ExcelReference excelRef = (ExcelReference)reference;
          
            if (excelRef.RowLast != excelRef.RowFirst || excelRef.ColumnLast!=excelRef.ColumnFirst)
            {
                return ((object[,])(excelRef).GetValue()).Cast<T>();
            }
            else
            {
                return new List<T>() { (T)excelRef.GetValue() };
            }
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

            double finalGrade = double.Parse(JSPredictedGrade.Invoke(atitudesGrade, knowledgeGrade).AsString()); 
            double finalRounding = Math.Round(finalGrade, 0, MidpointRounding.AwayFromZero);
            return finalRounding;
        }

        public static double toFixed(double number, uint decimals)
        {
            return double.Parse(number.ToString("N" + decimals));
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

            int? indexOfCourseworkFeedbakcColumn = Utils.GetCollumnByNameIndex(cacheFeedbackSheetID, sheetName, "A1");
            if(indexOfCourseworkFeedbakcColumn is null) return string.Empty;

            string courseworkFeedbackCollumn = Utils.GetExcelColumnName(indexOfCourseworkFeedbakcColumn.Value + 1); 
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

        public void InsertFinalGradeAllRows(Worksheet sheet)
        {
            int finalGradeCollumnNumber = GetCollumnByNameIndex(CollumnName.FinalGrade) + 1;
            ExcelReference range = new(3, GradeTable.FindLastStudentRow(sheet), finalGradeCollumnNumber, finalGradeCollumnNumber);
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

        public void InsertKnowledgeFunctionForRows(Worksheet sheet)
        {
            int knowledgeCollumnNumber = GetCollumnByNameIndex(CollumnName.Knowledge) + 1;
            ExcelReference range = new(3, GradeTable.FindLastStudentRow(sheet), knowledgeCollumnNumber, knowledgeCollumnNumber);
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
            int? feedbackCollumnNameIndex = Utils.GetCollumnByNameIndex(GradeSheetID, CollumnName.Feedback);
            if (feedbackCollumnNameIndex.HasValue)
            {
                string feedbackCollumnName = Utils.GetExcelColumnName(feedbackCollumnNameIndex.Value+ 1);
                Worksheet sheet = Utils.GetWorksheetById(GradeSheetID);
                sheet.Unprotect();
                sheet.get_Range($"{feedbackCollumnName}{row}").Formula = $"=GrabFeedbackFor(A{row})";
                sheet.Protect(); 
            }
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


        public static int FindLastStudentRow(Worksheet sheet)
        { 
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
            for (int i = 3; i < FindLastStudentRow(sheet); i++)
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
            for (int i = 3; i < FindLastStudentRow(sheet); i++)
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
            Range currentCell = worksheet.get_Range("B2");
            int counter = 1;

            while ( currentCell.Value2.ToString().Equals(courseworkName) is false)
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
            for (int i = 3; i < FindLastStudentRow(worksheet); i++)
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
            for (int i = 3; i < FindLastStudentRow(worksheet); i++)
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
