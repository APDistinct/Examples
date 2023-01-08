using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;

namespace FLChat.Core
{
    public class FakeInputFile : IInputFile
    {
        public MediaGroupKind Type { get; set; } = MediaGroupKind.Document;

        public string Media { get; set; } = "www.someurl.net";

        public string FileName { get; set; } = Guid.NewGuid().ToString() + ".txt";

        public string MediaType { get; set; }
    }
}
