using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Collections;

namespace FLChat.DAL.Import.Oracle
{
    public class SegmentSource : SourceDT<Tuple<int, IEnumerable<string>>>
    {
        private List<string> _segments;
        private int _userNumberIndex = -1;

        private const string UserNumberFieldName = "CONSULTANTNUMBER";
        private static readonly string[] IgnoreFields = new string[] { "FLUSERID", "ROWNUMBER", "COUNTROWS" };

        public SegmentSource()
            : base("flo.sflreportconssegment.FlChat_SegmentationConsultant") {
            
        }

        public override void Load(OracleConnection conn, int consNumber, int? start = null, int? end = null) {
            //if (start.HasValue || end.HasValue)
            //    throw new NotSupportedException("Partial segment loading has not support");
            base.Load(conn, consNumber, start, end);
            if (_segments == null)
                LoadFields();
        }

        public override IEnumerator<Tuple<int, IEnumerable<string>>> GetEnumerator() {
            return new SegmentSourceEnumerator(DataTable, _segments, _userNumberIndex);
        }

        public IEnumerable<string> SegmentNames => _segments.Where(s => s != null);

        private void LoadFields() {
            if (_segments == null) {
                _segments = new List<string>(DataTable.Columns.Count);
                for (int i = 0; i < DataTable.Columns.Count; ++i) {
                    string fn = DataTable.Columns[i].ColumnName;
                    if (IgnoreFields.Contains(fn)) {
                        _segments.Add(null);
                    } else if (fn == UserNumberFieldName) {
                        _segments.Add(null);
                        _userNumberIndex = i;
                    } else {
                        _segments.Add(fn);
                    }
                }

                if (_userNumberIndex == -1)
                    throw new KeyNotFoundException($"Field {UserNumberFieldName} has not found");
            }

            //return _segments.Where(s => s != null).ToList();
        }
    }

    public class SegmentSourceEnumerator : DataTableEnumerator<Tuple<int, IEnumerable<string>>>
    {
        private readonly int _userNumberIndex;
        private readonly List<string> _segments;

        public SegmentSourceEnumerator(DataTable dt, List<string> segments, int userNumberIndex) : base(dt) {
            _segments = segments;
            _userNumberIndex = userNumberIndex;
        }

        public override Tuple<int, IEnumerable<string>> Current {
            get {
                int number = 0;
                List<string> list = new List<string>();
                for (int i = 0; i < _segments.Count; ++i) {
                    if (i == _userNumberIndex)
                        number = (int)((decimal)CurrentRow[_userNumberIndex]);
                    else if (_segments[i] != null) {
                        if (CurrentRow[i] != DBNull.Value && (decimal)CurrentRow[i] != 0)
                            list.Add(_segments[i]);
                    }
                }
                return new Tuple<int, IEnumerable<string>>(number, list);
            }
        }
    }

}
