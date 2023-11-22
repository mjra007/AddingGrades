using ExcelDna.Integration;
using Microsoft.Office.Core;
using Microsoft.VisualStudio.Services.Common; 
using System.Xml.Serialization;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace AddinGrades.DTO
{
    public class WorkbookData
    {

        //Key is the worksheet code name and the value is the gradesheet object
        public SerializableDictionary<string, GradeSheet> GradeSheets = new();
        public string FeedbackSheetID;
        public string Version="v1";
        //public Queue<string> History = new(10);

        public WorkbookData() {
        }

        public GradeSheet CreateGradeSheet(string codeName)
        {
            GradeSheet sheet;
            GradeSheets.Add(codeName, sheet = new());
            return sheet;
        }

        public static T Deserialize<T>(string data)
        {
            XmlSerializer xmlSerializer = new(typeof(T));
            using TextReader reader = new StringReader(data);
            return (T)xmlSerializer.Deserialize(reader);
        }

        public static string Serialize<T>(T objectS)
        {
            XmlSerializer xmlSerializer = new(typeof(T));
            using var writer = new StringWriter();
            xmlSerializer.Serialize(writer, objectS);
            return writer.ToString();
        }  

        public void Save()
        {
            Application app = ExcelDnaUtil.Application as Application;
            foreach (CustomXMLPart item in app.ActiveWorkbook.CustomXMLParts.Cast<CustomXMLPart>())
            {
                string oldSavedXML = item.XML;
                if (oldSavedXML.Contains("WorkbookData"))
                {
/*                    //Only save in history if there is a diff
                    if (History.Last().Equals(oldSavedXML) == false)
                    {
                        //if history is full dequeue
                        if(History.Count == 10)
                        {
                            History.Dequeue();
                        }
                        History.Enqueue(oldSavedXML);
                    }*/
                    item.Delete(); 
                    app.ActiveWorkbook.CustomXMLParts.Add(WorkbookData.Serialize(this)); 
                }
            }
        }
         

    }
}
