using FLChat.DAL.Import;
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
    public class UserSource : SourceDT<IUserInfo> {
        public UserSource() 
            : base("flo.sflreportconssegment.FlChat_Consultant") {
        }

        public override IEnumerator<IUserInfo> GetEnumerator() => new UserSourceDataReader(DataTable);

        public void RemoveUnnessesaryColumns() {
            int i = 0;
            while (i < DataTable.Columns.Count) {
                if (_fields.Contains(DataTable.Columns[i].ColumnName.ToUpper()) == false)
                    DataTable.Columns.RemoveAt(i);
                else
                    i += 1;
            }

            CheckColumns();
        }

        private void CheckColumns() {
            if (_fields.Length != DataTable.Columns.Count)
                throw new IndexOutOfRangeException("Count of data table columns are not equal");

            for (int i = 0; i < _fields.Length; ++i)
                if (_fields[i] != DataTable.Columns[i].ColumnName.ToUpper())
                    throw new Exception($"Column name at position {i.ToString()} are invalid."
                        + $" Actual: {DataTable.Columns[i].ColumnName}; expected: {_fields[i]}");
        }

        private readonly string[] _fields = new string[] {
            "SURNAME",
            "NAME",
            "PATRONYMIC",
            "BIRTHDAY",
            "MOBILE",
            "EMAIL",
            "TITLE",
            "ZIP",
            "COUNTRY",
            "REGION",
            "CITY",
            "REGISTRATIONDATE",
            "EMAILPERMISSION",
            "SMSPERMISSION",
            "ISDIRECTOR",
            "LASTORDERDATE",
            "LO",
            "PERIODSWOLO",
            "OLG",
            "GO",
            "CASHBACKBALANCE",
            "FLCLUBPOINTS",
            "FLCLUBPOINTSBURN",
            "ROWNUMBER",
            "CONSULTANTNUMBER",
            "MENTORNUMBER",
            "CONSULTANTSTATE"
        };
    }

    public class UserSourceDataReader : DataTableEnumerator<IUserInfo>, IUserInfo {
        private readonly Dictionary<string, int> _fields = new Dictionary<string, int>();

        public UserSourceDataReader(DataTable dt) : base(dt) {
            LoadFieldNames();
        }

        public override IUserInfo Current => this;

        #region ISourceUser
        public int FLUserNumber => (int)GetDecimalNullable(_fields["CONSULTANTNUMBER"]).Value;
        public int? ParentFLUserNumber => (int?)GetDecimalNullable(_fields["MENTORNUMBER"]);
        public string Surname => GetStringNullable(_fields["Surname"]);
        public string Name => GetStringNullable(_fields["Name"]);
        public string Partronymic => GetStringNullable(_fields["Patronymic"]);
        public DateTime? Birthday => GetDateTimeNullable(_fields["Birthday"]);
        public string Phone => GetStringNullable(_fields["Mobile"]);
        public string Email => GetStringNullable(_fields["Email"]);
        public string Title => GetStringNullable(_fields["Title"]);
        public string ZipCode => GetStringNullable(_fields["ZIP"]);
        public string Country => GetStringNullable(_fields["Country"])?.Trim();
        public string Region => GetStringNullable(_fields["Region"])?.Trim() ?? GetStringNullable(_fields["City"])?.Trim();
        public string City => GetStringNullable(_fields["City"])?.Trim();
        public DateTime? RegDate => GetDateTimeNullable(_fields["RegistrationDate"]);
        public bool? SmsPermission => GetInt16AsBoolNullable(_fields["SMSPermission"]);
        public bool? EmailPermission => GetInt16AsBoolNullable(_fields["EmailPermission"]);
        public bool? IsDirector => GetInt16AsBoolNullable(_fields["IsDirector"]);
        public DateTime? LastOrder => GetDateTimeNullable(_fields["LastOrderDate"]);
        public decimal? LoScores => GetDecimalNullable(_fields["LO"]);
        public int? PeriodWoLo => (int?)GetDecimalNullable(_fields["PeriodsWOLO"]);
        public decimal? OlgScores => GetDecimalNullable(_fields["OLG"]);
        public decimal? GoScores => GetDecimalNullable(_fields["GO"]);
        public decimal? CashbackBalance => GetDecimalNullable(_fields["CashbackBalance"]);
        public decimal? FLClubPoints => GetDecimalNullable(_fields["FLClubPoints"]);
        public decimal? FLClubPointsBurn => GetDecimalNullable(_fields["FLClubPointsBurn"]);
        public bool? IsDeleted => GetStringNullable(_fields["CONSULTANTSTATE"]) == "Удален";
        #endregion

        static private string[] fields = new string[] {
            "CONSULTANTNUMBER",
            "MENTORNUMBER",
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
            "FLClubPointsBurn",
            "CONSULTANTSTATE"
        };

        private void LoadFieldNames() {
            foreach (string f in fields) {
                try {
                    int index = DataTable.Columns.IndexOf(f.ToUpper());
                    _fields.Add(f, index);
                } catch (IndexOutOfRangeException e) {
                    throw new IndexOutOfRangeException($"Column {f} has not found in result set", e);
                }
            }
        }

        private int? GetIntNullable(int index) => CurrentRow[index] == DBNull.Value
            ? (int?)null
            : (int)CurrentRow[index];//(CurrentRow[index].GetType() == typeof(int) 
                //? (int)CurrentRow[index] 
                //: int.Parse(CurrentRow[index].ToString()));
        private bool? GetInt16AsBoolNullable(int index) => CurrentRow[index] == DBNull.Value
            ? (bool?)null
            : (short)CurrentRow[index] != 0;
        private string GetStringNullable(int index) => CurrentRow[index] == DBNull.Value ? null : (string)CurrentRow[index];
        private DateTime? GetDateTimeNullable(int index) => CurrentRow[index] == DBNull.Value ? (DateTime?)null : (DateTime)CurrentRow[index];
        private bool? GetBoolNullable(int index) => CurrentRow[index] == DBNull.Value ? (bool?)null : (bool)CurrentRow[index];
        private decimal? GetDecimalNullable(int index) => CurrentRow[index] == DBNull.Value ? (decimal?)null : (decimal)CurrentRow[index];

    }
}
