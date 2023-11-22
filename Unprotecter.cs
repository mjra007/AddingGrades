using Microsoft.Office.Interop.Excel;

namespace AddinGrades
{
    public class Unprotecter : IDisposable
    {
        readonly Worksheet sheet;
        readonly bool currentStatus;
        public Unprotecter(Worksheet sheet)
        {
            this.sheet = sheet;
            this.currentStatus = sheet.ProtectContents;
            if (currentStatus)
            {
                sheet.Unprotect();
            }
        }

        public void Dispose()
        {
            if (currentStatus) sheet.Protect();
        }
    }
}
