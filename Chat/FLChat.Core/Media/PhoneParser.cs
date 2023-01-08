using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneNumbers;

namespace FLChat.Core.Media
{
    public interface IPhoneParser
    {
        bool TryParse(string phone, out string result);
    }

    public class PhoneParser : IPhoneParser
    {
        private readonly PhoneNumberUtil _phoneNumberUtil;

        public PhoneParser()
        {
            _phoneNumberUtil = PhoneNumberUtil.GetInstance();
        }

        public bool TryParse(string phone, out string result)
        {
            if (string.IsNullOrWhiteSpace(phone) || phone.Any(char.IsLetter))
            {
                result = null;
                return false;
            }

            try
            {
                string correctPhone = GetCorrectPhoneNumbers(phone);
                PhoneNumber phoneNumber = _phoneNumberUtil.Parse(correctPhone, "");
                
                result = $"{phoneNumber.CountryCode}{phoneNumber.NationalNumber}";
                return true;
            }
            catch (Exception e)
            {
                result = null;
                return false;
            }
        }

        private string GetCorrectPhoneNumbers(string phone)
        {
            string correctPhone = phone.Trim('+', ' ', '-', '(', ')', '.', ',', '_', '*', '!');

            if (correctPhone.StartsWith("8"))
                correctPhone = $"7{correctPhone.Substring(1)}";

            return $"+{correctPhone}";
        }
    }
}
