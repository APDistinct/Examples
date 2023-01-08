using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OfficeOpenXml;
using System.Globalization;
using System.Collections;

namespace FLChat.DAL.Import.Excel
{
    public class SourceUser : ISourceUser, IEnumerator<ISourceUser>
    {
        private readonly IFormatProvider _format;
        private readonly ExcelWorksheet _ws;
        private readonly IColumnIndexes _ind;
        private readonly int _rowCount;

        public SourceUser(ExcelWorksheet ws, IColumnIndexes ind, int startRow = 1) {
            _ws = ws;
            _ind = ind;
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture;//.Clone();
            //ci.NumberFormat.NumberDecimalSeparator = ",";
            _format = ci;
            _rowCount = _ws.Dimension.End.Row;
            RowIndex = startRow;
        }

        internal int RowIndex { get; set; }

        public int FLUserNumber => int.Parse(Get(_ind.FLUserNumber));
        public int? ParentFLUserNumber => GetInt(_ind.ParentFLUserNumber);
        public string Surname => Get(_ind.Surname);
        public string Name => Get(_ind.Name);
        public string Partronymic => Get(_ind.Partronymic);
        public DateTime? Birthday => GetDateTime(_ind.Birthday);
        public string Phone => Get(_ind.Phone);
        public string Email => Get(_ind.Email);
        public string Title => Get(_ind.Title);
        public string ZipCode => Get(_ind.ZipCode);
        public string Country => Get(_ind.Country);
        public string Region => Get(_ind.Region);
        public string City => Get(_ind.City);
        public DateTime? RegDate => GetDateTime(_ind.RegDate);
        public bool? SmsPermission => GetBool(_ind.SmsPermission);
        public bool? EmailPermission => GetBool(_ind.EmailPermission);
        public bool? IsDirector => GetBool(_ind.IsDirector);
        public DateTime? LastOrder => GetDateTime(_ind.LastOrder);
        public decimal? LoScores => GetDecimal(_ind.LoScores);
        public int? PeriodWoLo => GetInt(_ind.PeriodWoLo);
        public decimal? OlgScores => GetDecimal(_ind.OlgScores);
        public decimal? GoScores => GetDecimal(_ind.GoScores);
        public decimal? CashbackBalance => GetDecimal(_ind.CashbackBalance);
        public decimal? FLClubPoints => GetDecimal(_ind.FLClubPoints);
        public decimal? FLClubPointsBurn => GetDecimal(_ind.FLClubPointsBurn);
        public bool? IsDeleted => null;


        private string Get(int? col) {
            if (col.HasValue == false)
                return null;

            object obj = _ws.Cells[RowIndex, col.Value].Text;
            string tmp = obj?.ToString().Trim();
            if (tmp == "#VALUE!")
                return null;
            if (String.IsNullOrEmpty(tmp))
                return null;
            else
                return tmp;
        }

        private int? GetInt(int? col) {
            string tmp = Get(col);            
            if (tmp == null)
                return null;
            else {
                try {
                    return int.Parse(tmp);
                } catch (Exception e) {
                    throw new FormatException($"{RowIndex.ToString()}:{col.ToString()}: value '{tmp}' is invalid: {e.Message.ToString()}", e);
                }
            }
        }

        private bool? GetBool(int? col) {
            string tmp = Get(col);
            if (tmp == null)
                return null;
            else {
                try {
                    return int.Parse(tmp) != 0;
                } catch (Exception e) {
                    throw new FormatException($"{RowIndex.ToString()}:{col.ToString()}: value '{tmp}' is invalid: {e.Message.ToString()}", e);
                }
            }
        }

        private double? GetDouble(int? col) {
            string tmp = Get(col);
            if (tmp == null)
                return null;
            else {
                try {
                    return double.Parse(tmp, _format);
                } catch (Exception e) {
                    throw new FormatException($"{RowIndex.ToString()}:{col.ToString()}: value '{tmp}' is invalid: {e.Message.ToString()}", e);
                }
            }
        }

        private decimal? GetDecimal(int? col) {
            string tmp = Get(col);
            if (tmp == null)
                return null;
            else {
                try {
                    return decimal.Parse(tmp, _format);
                } catch (Exception e) {
                    throw new FormatException($"{RowIndex.ToString()}:{col.ToString()}: value '{tmp}' is invalid: {e.Message.ToString()}", e);
                }
            }
        }

        private DateTime? GetDateTime(int? col) {
            string tmp = Get(col);
            if (tmp == null)
                return null;
            else {
                try {
                    return DateTime.ParseExact(tmp, "dd.MM.yyyy", _format);
                } catch (Exception e) {
                    throw new FormatException($"{RowIndex.ToString()}:{col.ToString()}: value '{tmp}' is invalid: {e.Message.ToString()}", e);
                }
            }
        }

        #region IEnumerator
        public ISourceUser Current => this;

        object IEnumerator.Current => this;

        public void Dispose() {            
        }

        public bool MoveNext() {
            RowIndex += 1;
            return (RowIndex <= _rowCount);
        }

        public void Reset() {
            RowIndex = 1;
        }
        #endregion
    }
}
