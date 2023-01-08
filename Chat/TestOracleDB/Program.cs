using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestOracleDB
{
    class Program
    {
        static void Main(string[] args) {
            try {
                using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["Partner"].ConnectionString)) {
                    conn.Open();
                    Console.WriteLine("Connected");


                    CmdToFile(conn, "flo.sflreportconssegment.FlChat_Consultant", "cons");
                    CmdToFile(conn, "flo.sflreportconssegment.FlChat_SegmentationConsultant", "segm");
                    //using (OracleCommand cmd = new OracleCommand() {
                    //    Connection = conn,
                    //    CommandText = "flo.sflreportconssegment.FlChat_Consultant",
                    //    CommandType = System.Data.CommandType.StoredProcedure,
                    //}) {
                    //    object nc = GetConfValue("npCons");
                    //    object ns = GetConfValue("npStart");
                    //    object ne = GetConfValue("npEnd");
                    //    Console.WriteLine($"nc = {nc.ToString()}; s = {ns.ToString()}; e = {ne.ToString()}");
                    //    cmd.Parameters.Add(new OracleParameter("npCons", nc));
                    //    cmd.Parameters.Add(new OracleParameter("npStart", ns));
                    //    cmd.Parameters.Add(new OracleParameter("npEnd", ne));
                    //    OracleParameter pr = cmd.Parameters.Add(new OracleParameter("R", OracleDbType.RefCursor));
                    //    pr.Direction = System.Data.ParameterDirection.Output;

                    //    cmd.ExecuteNonQuery();
                    //    Console.WriteLine("procedure successfully completed");

                    //    CmdToFile(cmd, $"c{nc.ToString()}-{ns.ToString()}-{ne.ToString()}");

                    //}
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("Finished. Press enter...");
            Console.ReadLine();
        }

        private static void Da_FillError(object sender, FillErrorEventArgs e) {
            e.Continue = true;
            Console.WriteLine("ERROR: " + e.Errors.ToString());
            Console.WriteLine("Values: ");
            foreach (var o in e.Values)
                Console.WriteLine("\t" + o?.ToString() ?? "(null)");
        }

        private static object GetConfValue(string name) {
            string s = ConfigurationManager.AppSettings[name];
            if (String.IsNullOrEmpty(s))
                return DBNull.Value;
            else
                return int.Parse(s);
        }

        private static void CmdToFile(OracleConnection conn, string cmdName, string prefix) {
            using (OracleCommand cmd = new OracleCommand() {
                Connection = conn,
                CommandText = cmdName,
                CommandType = System.Data.CommandType.StoredProcedure,
            }) {
                object nc = GetConfValue("npCons");
                object ns = GetConfValue("npStart");
                object ne = GetConfValue("npEnd");
                Console.WriteLine($"nc = {nc.ToString()}; s = {ns.ToString()}; e = {ne.ToString()}");
                cmd.Parameters.Add(new OracleParameter("npCons", nc));
                cmd.Parameters.Add(new OracleParameter("npStart", ns));
                cmd.Parameters.Add(new OracleParameter("npEnd", ne));
                OracleParameter pr = cmd.Parameters.Add(new OracleParameter("R", OracleDbType.RefCursor));
                pr.Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                Console.WriteLine("procedure successfully completed");

                CmdToFile(cmd, $"{prefix}{nc.ToString()}-{ns.ToString()}-{ne.ToString()}");
            }
        }

        private static void CmdToFile(OracleCommand cmd, string fnPrefix) {
            DataTable ds = new DataTable();
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            da.ContinueUpdateOnError = true;
            da.FillError += Da_FillError;
            da.Fill(ds);

            Console.WriteLine("Successfully filled");

            Console.WriteLine("Row count: " + ds.Rows.Count.ToString());
            Console.WriteLine("Column count: " + ds.Columns.Count.ToString());
            using (StreamWriter sw = File.CreateText(fnPrefix + "_columns.txt")) {
                for (int i = 0; i < ds.Columns.Count; ++i) {
                    DataColumn column = ds.Columns[i];
                    String s = $"Column {i.ToString()}: {column.ColumnName} ({column.DataType.Name}) {(column.AllowDBNull ? " nullable" : "")}";
                    Console.WriteLine(s);
                    sw.WriteLine(s);
                }
                sw.Close();
            }
            //Console.WriteLine("Row count: " + ds.);

            string fn = $"{fnPrefix}-{DateTime.Now.ToString("yyyyMMddhhmmss")}.csv";
            Console.WriteLine(fn);
            using (StreamWriter sw = File.CreateText(fn)) {
                for (int i = 0; i < ds.Columns.Count; ++i)
                    sw.Write(ds.Columns[i].ColumnName + ";");
                sw.WriteLine();

                for (int row = 0; row < ds.Rows.Count; ++row) {
                    for (int i = 0; i < ds.Columns.Count; ++i) {
                        sw.Write(ds.Rows[row][i].ToString());
                        sw.Write(";");
                    }
                    sw.WriteLine();
                }
                sw.Close();
            }
            Console.WriteLine("save complete");
        }
    }
}
