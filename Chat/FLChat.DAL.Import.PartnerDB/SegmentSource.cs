using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import.PartnerDB
{
    public class SegmentSource : IDisposable, IEnumerable<Tuple<int, IEnumerable<string>>>
    {
        //private readonly string _connString;
        private readonly SqlCommand _cmd;
        private SqlDataReader _reader;
        private List<string> _segments;
        private int _userNumberIndex = -1;

        private const string UserNumberFieldName = "Consultant_id";
        private static readonly string[] IgnoreFields = new string[] { "FLUserID", "RowNumber", "CountRows" };

        public SegmentSource(SqlConnection conn, SqlTransaction trans, int consNumber) {
            _cmd = conn.CreateCommand();
            _cmd.Transaction = trans;
            _cmd.CommandType = System.Data.CommandType.StoredProcedure;
            _cmd.CommandText = "ssrs.FlChat_SegmentationConsultant";
            _cmd.CommandTimeout = 5 * 60;
            _cmd.Parameters.AddWithValue("UserSource", consNumber);
        }

        public void Dispose() {
            _reader?.Close();
            _cmd.Dispose();
        }

        public List<string> LoadFields() {
            if (_reader == null)
                _reader = _cmd.ExecuteReader();

            _segments = new List<string>(_reader.FieldCount);
            for (int  i = 0; i < _reader.FieldCount; ++i ) {
                string fn = _reader.GetName(i);
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

            return _segments.Where(s => s != null).ToList();
        }

        public IEnumerator<Tuple<int, IEnumerable<string>>> GetEnumerator() {
            return new SegmentSourceEnumerator(_reader, _segments, _userNumberIndex);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public class SegmentSourceEnumerator : IEnumerator<Tuple<int, IEnumerable<string>>>
    {
        private readonly SqlDataReader _reader;
        private readonly List<string> _segments;
        private readonly int _userNumberIndex;

        public SegmentSourceEnumerator(SqlDataReader reader, List<string> segments, int userNumberIndex) {
            _reader = reader;
            _segments = segments;
            _userNumberIndex = userNumberIndex;
        }

        public Tuple<int, IEnumerable<string>> Current {
            get {
                int number = 0;
                List<string> list = new List<string>();
                for (int i = 0; i < _segments.Count; ++i) {
                    if (i == _userNumberIndex)
                        number = _reader.GetInt32(i);
                    else if (_segments[i] != null) {
                        if (!_reader.IsDBNull(i) && _reader.GetBoolean(i))
                            list.Add(_segments[i]);
                    }                        
                }
                return new Tuple<int, IEnumerable<string>>(number, list);
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose() {            
        }

        public bool MoveNext() {
            return _reader.Read();
        }

        public void Reset() {            
        }
    }
}
