using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;
using Z.EntityFramework.Plus;

namespace FLChat.WebService.Handlers.File
{
    public class PhonesGetter : IDisposable, IPhonesGetter
    {
        private readonly DataTable _stringList;

        public PhonesGetter()
        {
            _stringList = new DataTable();
            _stringList.Columns.Add("[String]", typeof(string));
        }

        public /*IEnumerable<Guid>*/ Guid[] GetMatchedPhones(ChatEntities entities, Guid userId, IEnumerable<string> phones)
        {
            _stringList.Rows.Clear();

            foreach (string phone in phones)
            {
                _stringList.Rows.Add(phone);
            }
            var ret = 
            entities.Database.SqlQuery<Guid>(
                "select * from [Usr].[User_GetChildsWithPhone] (@usersPhone, @userId) ",
                new SqlParameter("@usersPhone", SqlDbType.Structured)
                {
                    TypeName = "[dbo].[StringList]",
                    Value = _stringList,
                    Direction = ParameterDirection.Input
                },
                new SqlParameter("@userId", userId)
                //new SqlParameter("@userId", SqlDbType.Structured)
                //{
                //    TypeName = "[uniqueidentifier]",
                //    Value = userId,
                //    Direction = ParameterDirection.Input
                //}
                ).ToArray/*.AsEnumerable*/();
            return ret;
        }

        public void Dispose()
        {
            _stringList.Dispose();
        }
    }
}
