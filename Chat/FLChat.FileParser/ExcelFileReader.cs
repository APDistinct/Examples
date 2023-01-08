using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace FLChat.FileParser
{
    public class ExcelFileReader : IFileReader
    {
        public List<string> Parse(string base64EncodedString)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            byte[] bytes = Convert.FromBase64String(base64EncodedString);
            var contents = new MemoryStream(bytes);
            using (var excelPackage = new ExcelPackage(contents))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                var result = new List<string>();

                for (int row = 1; row <= rowCount; row++)
                {
                    result.Add(worksheet.Cells[row, 1].Value?.ToString().Trim());
                }

                return result;
            }
        }
    }
}
