using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL
{
    /// <summary>
    /// Keep scenario values and provide easy access to it
    /// </summary>
    public class ScenarioDict
    {

        private readonly Dictionary<string, int> _dict;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="dict">dictionary with settings</param>
        public ScenarioDict(Dictionary<string, int> dict)
        {
            _dict = dict;
        }

        /// <summary>
        /// get integer value by name
        /// </summary>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetValue(string index, int defaultValue)
        {
            if (_dict.TryGetValue(index, out int value))
            {
                return value;
            }
            else
                return defaultValue;
        }

        /// <summary>
        /// get value as string
        /// </summary>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        //public string GetValue(string index, string defaultValue)
        //{
        //    if (_dict.TryGetValue(index, out string value))
        //        return value;
        //    else
        //        return defaultValue;
        //}

    }

}