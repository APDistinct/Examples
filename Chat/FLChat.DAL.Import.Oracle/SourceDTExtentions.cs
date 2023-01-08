using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import.Oracle
{
    public static class SourceDTExtentions
    {
        public static void Load<T>(this SourceDT<T> source, string connString, ref OracleConnection conn,
            int consNumber, int? start = null, int? end = null, 
            Func<OracleException, int, bool> onException = null) where T : class {
            int tryCount = 0;
            do {
                try {
                    if (conn == null) {
                        conn = new OracleConnection(connString);
                        conn.Open();
                    }
                    source.Load(conn, consNumber, start, end);
                    return;
                } catch (OracleException e) {
                    //may be network error
                    if ((onException?.Invoke(e, tryCount) ?? true) == false)
                        throw;

                    if (tryCount > 5)
                        throw;

                    Task.Delay(GetDelayTime(tryCount)).Wait();
                    tryCount += 1;

                    conn.Dispose();
                    conn = null;
                }
            } while (true);
        }

        private static TimeSpan GetDelayTime(int tryCount) {
            switch (tryCount) {
                case 0: return TimeSpan.FromMinutes(1);
                case 1: return TimeSpan.FromMinutes(5);
                default: return TimeSpan.FromMinutes(10);
            }
        }
    }
}
