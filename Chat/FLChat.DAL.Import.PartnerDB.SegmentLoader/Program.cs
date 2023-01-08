using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using log4net;
using System.Data.SqlClient;
using System.Configuration;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace FLChat.DAL.Import.PartnerDB.SegmentLoader
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger("root");

        static void Main(string[] args) {
            //int 
            try {
                using (ChatEntities entities = new ChatEntities()) {
                    //entities.
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Partner"].ConnectionString)) {
                        conn.Open();
                        using (SqlTransaction trans = conn.BeginTransaction()) {
                            using (SegmentSource src = new SegmentSource(conn, trans, 342052)) {
                                entities.SyncPartnerSegments(src.LoadFields());

                                using (var entTrans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)) {
                                    int cnt = entities.ImportSegments(src);
                                    log.Info($"updated: {cnt.ToString()}");
                                    log.Info("Complete");

                                    entTrans.Commit();
                                }
                            }
                            trans.Commit();
                        }
                    }
                }
            } catch (Exception e) {
                log.Error(e.ToString());
                //Console.ReadKey();
            }
        }
    }
}
