using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.DataTypes
{
    public class StructureNodeVirtual
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public int Count { set; get; }
        public bool Final { get; set; }
    }
}
