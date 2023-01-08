using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace TestDB
{
    class Program
    {
        static void Main(string[] args) {
            using (ChatEntities entities = new ChatEntities()) {
                MessageType []list = entities.MessageType.ToArray();
                foreach (MessageType mt in list) {
                    Console.WriteLine($"ID = {mt.Id}; Name = {mt.Name}");
                }
                Console.ReadLine();
            }
        }
    }
}
