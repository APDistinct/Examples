using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Import.Excel.ColumnIndexes;

namespace FLChat.DAL.Import.Excel
{
    public class ExcelUserEnumerable : IEnumerable<ISourceUser>, IDisposable
    {
        private readonly ExcelPackage _package;
        private readonly ExcelWorksheet _ws;
        private readonly IColumnIndexes _ind;
        private readonly int _rowCount;
        private readonly int _startRow;

        public ExcelUserEnumerable(string fileName, IColumnIndexes ci, int startRow = 1) {
            _package = new ExcelPackage(new System.IO.FileInfo(fileName));
            _ws = _package.Workbook.Worksheets[1];
            _rowCount = _ws.Dimension.End.Row;
            _ind = ci ?? new FixedColumnIndexes();
            _startRow = startRow;
        }

        public int RowCount => _rowCount;

        public void Dispose() {
            _ws.Dispose();
            _package.Dispose();
        }

        public IEnumerator<ISourceUser> GetEnumerator() => new SourceUser(_ws, _ind, _startRow);       

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
