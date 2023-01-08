using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Import.PartnerDB
{
    public class UserSource : IEnumerable<ISourceUser>, IDisposable
    {
        //private readonly string _connString;
        private readonly SqlCommand _cmd;

        public UserSource(SqlConnection conn, SqlTransaction trans, int consNumber) {
            _cmd = conn.CreateCommand();
            _cmd.Transaction = trans;
            _cmd.CommandType = System.Data.CommandType.StoredProcedure;
            _cmd.CommandText = "ssrs.FlChat_Consultant";
            _cmd.CommandTimeout = 5 * 60;
            _cmd.Parameters.AddWithValue("UserSource", consNumber);
        }

        public void Dispose() {
            _cmd.Dispose();
        }

        public IEnumerator<ISourceUser> GetEnumerator() {
            return new UserSourceDataReader(_cmd.ExecuteReader());
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public class UserSourceDataReader : IEnumerator<ISourceUser>, ISourceUser
    {
        private readonly SqlDataReader _reader;
        private readonly Dictionary<string, int> _fields = new Dictionary<string, int>();

        public UserSourceDataReader(SqlDataReader reader) {
            _reader = reader;
            LoadFieldNames();
        }        

        public int FLUserNumber => _reader.GetInt32(_fields["FLUserID"]);
        public int? ParentFLUserNumber => GetIntNullable(_fields["ParentFLUserID"]);
        public string Surname => GetStringNullable(_fields["Surname"]);
        public string Name => GetStringNullable(_fields["Name"]);
        public string Partronymic => GetStringNullable(_fields["Patronymic"]);
        public DateTime? Birthday => GetDateTimeNullable(_fields["Birthday"]);
        public string Phone => GetStringNullable(_fields["Mobile"]);
        public string Email => GetStringNullable(_fields["Email"]);
        public string Title => GetStringNullable(_fields["Title"]);
        public string ZipCode => GetStringNullable(_fields["ZIP"]);
        public string Country => GetStringNullable(_fields["Country"]);
        public string Region => GetStringNullable(_fields["Region"]);
        public string City => GetStringNullable(_fields["City"]);
        public DateTime? RegDate => GetDateTimeNullable(_fields["RegistrationDate"]);
        public bool? SmsPermission => GetBoolNullable(_fields["SMSPermission"]);
        public bool? EmailPermission => GetBoolNullable(_fields["EmailPermission"]);
        public bool? IsDirector => GetBoolNullable(_fields["IsDirector"]);
        public DateTime? LastOrder => GetDateTimeNullable(_fields["LastOrderDate"]);
        public decimal? LoScores => GetDecimalNullable(_fields["LO"]);  
        public int? PeriodWoLo => GetIntNullable(_fields["PeriodsWOLO"]);
        public decimal? OlgScores => GetDecimalNullable(_fields["OLG"]);
        public decimal? GoScores => GetDecimalNullable(_fields["GO"]);
        public decimal? CashbackBalance => GetDecimalNullable(_fields["CashbackBalance"]);
        public decimal? FLClubPoints => GetDecimalNullable(_fields["FLClubPoints"]);
        public decimal? FLClubPointsBurn => GetDecimalNullable(_fields["FLClubPointsBurn"]);
        public bool? IsDeleted => null;

        public ISourceUser Current => this;

        object IEnumerator.Current => this;

        public void Dispose() {
            _reader.Close();
        }

        public bool MoveNext() {
            return _reader.Read();
        }

        public void Reset() {
            //throw new NotImplementedException();
        }

        static string[] fields = new string[] {
            "FLUserID",
            "ParentFLUserID",
            "Surname",
            "Name",
            "Patronymic",
            "Birthday",
            "Mobile",
            "Email",
            "Title",
            "ZIP",
            "Country",
            "Region",
            "City",
            "RegistrationDate",
            "EmailPermission",
            "SMSPermission",
            "IsDirector",
            "LastOrderDate",
            "LO",
            "PeriodsWOLO",
            "OLG",
            "GO",
            "CashbackBalance",
            "FLClubPoints",
            "FLClubPointsBurn"
        };

        private void LoadFieldNames() {            
            foreach (string f in fields) {
                try {
                    int index = _reader.GetOrdinal(f);
                    _fields.Add(f, index);
                } catch (IndexOutOfRangeException e) {
                    throw new IndexOutOfRangeException($"Column {f} has bot found in result set", e);
                }
            }
        }

        private int? GetIntNullable(int index) => _reader.IsDBNull(index) ? (int?)null : _reader.GetInt32(index);
        private string GetStringNullable(int index) => _reader.IsDBNull(index) ? null : _reader.GetString(index);
        private DateTime? GetDateTimeNullable(int index) => _reader.IsDBNull(index) ? (DateTime?)null : _reader.GetDateTime(index);
        private bool? GetBoolNullable(int index) => _reader.IsDBNull(index) ? (bool?)null : _reader.GetBoolean(index);
        private decimal? GetDecimalNullable(int index) => _reader.IsDBNull(index) ? (decimal?)null : _reader.GetDecimal(index);
    }
}
