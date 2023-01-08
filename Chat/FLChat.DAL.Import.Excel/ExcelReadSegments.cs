using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import.Excel
{
    public class ExcelReadSegments : IDisposable
    {
        private readonly int _flUserNumberIndex;
        private List<string> _segments;
        private readonly int _headerRow;
        private readonly string[] _excludeColumns = new string[] { "FLUSERID", "ROWNUMBER", "COUNTROWS" };

        private readonly ExcelPackage _package;
        private readonly ExcelWorksheet _ws;

        public struct MessageArg
        {
            public int Row { get; set; }
            public string Message { get; set; }
        }

        public event EventHandler<MessageArg> OnError;

        public ExcelReadSegments(string fileName, int flUserNumberIndex, int headerRow = 1) {
            _flUserNumberIndex = flUserNumberIndex;
            _headerRow = headerRow;

            _package = new ExcelPackage(new System.IO.FileInfo(fileName));
            _ws = _package.Workbook.Worksheets[1];
        }



        public IEnumerable<string> LoadSegmentNames() {
            _segments = new List<string>();
            _segments.AddRange(Enumerable.Repeat<string>(null, _ws.Dimension.End.Column));
            for (int col = 1; col < _ws.Dimension.End.Column; ++col) {
                if (col == _flUserNumberIndex)
                    continue;
                string tmp = _ws.Cells[_headerRow, col].Text;
                if (_excludeColumns.Contains(tmp))
                    continue;

                if (!String.IsNullOrWhiteSpace(tmp))
                    _segments[col] = tmp;
            }
            return _segments.Where(s => s != null);
        }

        public IEnumerable<Tuple<int, IEnumerable<string>>> Load() {
            int rowCount = _ws.Dimension.End.Row;
            for (int row = _headerRow + 1; row < rowCount; ++row) {
                string tmp = _ws.Cells[row, _flUserNumberIndex].Text;
                if (!int.TryParse(tmp, out int userNumber)) {
                    OnError?.Invoke(this, new MessageArg() {
                        Row = row,
                        Message = $"Incorrect user number <{tmp}>"
                    });
                    continue;
                }

                List<string> activeSegments = new List<string>();
                for (int col = 1; col < _segments.Count; ++col) {
                    if (_segments[col] != null) {
                        tmp = _ws.Cells[row, col].Text;
                        if (!String.IsNullOrEmpty(tmp)) {
                            if (int.TryParse(tmp, out int val)) {
                                if (val > 0)
                                    activeSegments.Add(_segments[col]);
                            } else
                                OnError?.Invoke(this, new MessageArg() {
                                    Row = row,
                                    Message = $"Incorrect flag at column <{col}>, value <{tmp}>"
                                });
                        }
                    }
                }
                yield return new Tuple<int, IEnumerable<string>>(userNumber, activeSegments);
            }


            //yield break;
        }

        public void Dispose() {
            _ws.Dispose();
            _package.Dispose();
        }
    }
}
