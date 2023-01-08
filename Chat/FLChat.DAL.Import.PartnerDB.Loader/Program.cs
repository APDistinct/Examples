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

namespace FLChat.DAL.Import.PartnerDB.Loader
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
                            using (UserSource src = new UserSource(conn, trans, 342052)) {
                                //TotalCount = excel.RowCount;
                                using (var entTrans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)) {
                                    ImportResult result = entities.FullImport(src, new Listener());
                                    log.Info($"updated: {result.UpdatedCount.ToString()}");
                                    log.Info($"new    : {result.NewbeCount.ToString()}");
                                    int deleted = entities.DisableUsers(342052, result.Users);
                                    log.Info($"deleted: {deleted.ToString()}");
                                    log.Info("Complete");

                                    User head = entities.User.Where(u => u.FLUserNumber == 342052).SingleOrDefault();
                                    if (!result.Users.Contains(head.Id))
                                        log.Warn("Result set does not include header consultant");

                                    entTrans.Commit();
                                }
                                //Console.ReadKey();
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

        private class Listener : IListener
        {
            public void Progress(int stage, int index, bool updated) {
                log.Info($"{stage.ToString()}: {index.ToString()} {(updated ? "upd" : "ins")}");
            }

            public void Warning(int index, string text) {
                log.Warn($"{index.ToString()}:  {text}");
            }
        }
    }
}
