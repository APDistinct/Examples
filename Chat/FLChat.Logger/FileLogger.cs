using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Logger
{
    public class FileLogger
    {
        private string _path;
        public string FileName => _path;

        private static object _lock = new object();

        public FileLogger(string path = null)
        {
            _path = path;
        }

        public void LogObject(string name, object obj)
        {
        }

        public void Log(string str)
        {
        }

        // Продумать разделение
        // Включение-отключение и т.п.


        //public FileLogger(string path = null)
        //{
        //    _path = string.IsNullOrWhiteSpace(path)
        //        //? Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "log.txt")
        //        ? Path.Combine(Directory.GetCurrentDirectory(), "log.txt")
        //        : path;
        //    if (!File.Exists(_path))
        //    {
        //        try
        //        {
        //            File.Create(_path);
        //            //new FileInfo(_path).Directory.Create();
        //        }
        //        catch (Exception e)
        //        {
        //            throw new FileLogExceptions($"Can't create log file ({_path}).   {e.Message}");
        //        }
        //    }
        //}

        //public void LogObject(string name, object obj)
        //{
        //    string str = null;
        //    try
        //    {
        //        str = JsonConvert.SerializeObject(obj);
        //        Log($"{name}: {str}");
        //    }
        //    catch (Exception e)
        //    {
        //        //throw new FileLogExceptions($"Can't serialize object ({obj.GetType().Name})  {e.Message}");
        //    }
        //}

        //public void Log(string str)
        //{
        //    var time = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
        //    lock (_lock)
        //    {
        //        File.AppendAllText(_path, $"{time}\t{str}{Environment.NewLine}");
        //    }
        //}
    }


}
