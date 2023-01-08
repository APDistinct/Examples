using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL
{
    /// <summary>
    /// Keep settings values and provide easy access to it
    /// </summary>
    public class SettingsDict
    {
        /// <summary>
        /// Settings names
        /// </summary>
        public enum SettingNames
        {
            ONLINE_PERIOD_SEC,
            TEXT_CHANGE_MESSAGE_ADDRESSEE
        }

        private readonly Dictionary<string, string> _dict;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dict">dictionary with settings</param>
        public SettingsDict(Dictionary<string, string> dict) {
            _dict = dict;
        }

        /// <summary>
        /// get integer value by name
        /// </summary>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetValue(string index, int defaultValue) {
            if (_dict.TryGetValue(index, out string value)) {
                return int.Parse(value);
            } else
                return defaultValue;
        }

        /// <summary>
        /// get integer value by name
        /// </summary>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetValue(SettingNames index, int defaultValue) => GetValue(index.ToString(), defaultValue);

        /// <summary>
        /// get value as string
        /// </summary>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetValue(string index, string defaultValue) {
            if (_dict.TryGetValue(index, out string value))
                return value;
            else
                return defaultValue;
        }

        /// <summary>
        /// get string value by name
        /// </summary>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetValue(SettingNames index, string defaultValue) => GetValue(index.ToString(), defaultValue);

        public int OnlinePeriodSec => GetValue(SettingNames.ONLINE_PERIOD_SEC, 300);
    }
}
