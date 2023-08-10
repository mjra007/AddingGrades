using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
