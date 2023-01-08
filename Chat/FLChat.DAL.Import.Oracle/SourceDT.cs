using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import.Oracle
{
    public abstract class SourceDT<T> : IImportSource<T>, IDisposable where T : class
    {
        private readonly string _procedureName;

        public event EventHandler<FillErrorEventArgs> OnFillError;
        public DataTable DataTable { get; private set; } = null;

        public int TopUserNumber { get; private set; }
        public int RowCount => DataTable.Rows.Count;

        public SourceDT(string procedureName) {
            _procedureName = procedureName;
        }

        public virtual void Load(OracleConnection conn, int consNumber, int? start = null, int? end = null) {
            TopUserNumber = consNumber;
            DataTable?.Dispose();
            DataTable = LoadToDataTable(conn, _procedureName, consNumber, start, end);
        }

        public void Dispose() {
            DataTable?.Dispose();
        }

        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private DataTable LoadToDataTable(OracleConnection conn, string cmdName, int consNumber, int? start = null, int? end = null) {
            using (OracleCommand cmd = new OracleCommand() {
                Connection = conn,
                CommandText = cmdName,
                CommandType = System.Data.CommandType.StoredProcedure,
            }) {
                cmd.Parameters.Add(new OracleParameter("npCons", consNumber));
                cmd.Parameters.Add(new OracleParameter("npStart", start.HasValue ? (object)start.Value : DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("npEnd", end.HasValue ? (object)end.Value : DBNull.Value));
                OracleParameter pr = cmd.Parameters.Add(new OracleParameter("R", OracleDbType.RefCursor));
                pr.Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.ContinueUpdateOnError = true;
                da.FillError += Da_FillError;
                da.Fill(dt);

                return dt;
            }
        }

        private void Da_FillError(object sender, FillErrorEventArgs e) {
            e.Continue = true;
            OnFillError?.Invoke(this, e);
        }

    }
}
