using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import.Oracle
{
    public abstract class DataTableEnumerator<T> : IEnumerator<T>
    {
        private readonly DataTable _dt;
        private int _row;

        protected DataTable DataTable => _dt;
        protected DataRow CurrentRow => _dt.Rows[_row];

        public DataTableEnumerator(DataTable dt) {
            _dt = dt;
            _row = -1;            
        }

        public abstract T Current { get; }

        object IEnumerator.Current => this;

        public void Dispose() {
        }

        public bool MoveNext() {
            _row += 1;
            return _row < _dt.Rows.Count;
        }

        public void Reset() {
            _row = -1;
        }
    }
}
