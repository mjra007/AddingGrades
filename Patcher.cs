using AddinGrades.Upgrader;

namespace AddinGrades
{
    public class Patcher
    {
        public static Dictionary<(string oldVersion, string newVersion), Action> UpdaterDictionary = new(){

            { ("v1" , "v1.1"), new UpdateFrom1to1Dot1().Update },
            { ("v1" , "v1.2"), new UpdateFrom1Dot1To1Dot2().Update },
            { ("v1.1", "v1.2"), new UpdateFrom1Dot1To1Dot2().Update }, 
            { ("v1.2", "v1.3"), new UpdateFrom1Dot2To1Dot3().Update }
        };
         

        public static void UpdateWorkbook(string currentSheetVersion)
        {
            try
            {
                foreach (var pair in UpdaterDictionary)
                {
                    if (currentSheetVersion.Equals(pair.Key.oldVersion) && Program.Version.Equals(pair.Key.newVersion))
                    {
                        Program.LoggerPanel.WriteLineToPanel($"Upgrading project from {pair.Key.oldVersion} to {pair.Key.newVersion}");
                        pair.Value.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.LoggerPanel.WriteLineToPanel("[Error] Could not upgrade sheet! ");
            }
        }

    }
}
