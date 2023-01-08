using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Model
{
    public partial class FileInfo
    {
        public static readonly string UrlTemplate 
            = Settings.Values.GetValue("MAINSERVER_NAME", "https://rvprj.ru:8443/FLChat/") 
            + Settings.Values.GetValue("COMMAND_GETFILE", "file/%id%");
        public const string IdPattern = "%id%";

        /// <summary>
        /// Public url to file
        /// </summary>
        public string Url => UrlTemplate.Replace(IdPattern, Id.ToString());
    }
}
