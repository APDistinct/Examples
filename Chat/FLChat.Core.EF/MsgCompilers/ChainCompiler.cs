using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.MsgCompilers
{
    public class ChainCompiler : IMessageTextCompiler, IEnumerable<IMessageTextCompiler>
    {
        private readonly LinkedList<IMessageTextCompiler> _list = new LinkedList<IMessageTextCompiler>();

        /// <summary>
        /// Create empty Compiler
        /// </summary>
        public ChainCompiler()
        {
        }

        /// <summary>
        /// Create and add routers form <paramref name="list"/>
        /// First item in <paramref name="list"/> become first item in ChainCompiler 
        /// </summary>
        /// <param name="list">List of Compilers</param>
        public ChainCompiler(IEnumerable<IMessageTextCompiler> list)
        {
            foreach (IMessageTextCompiler compiller in list)
                _list.AddLast(compiller);
        }

        /// <summary>
        /// Add Compiler to first position
        /// </summary>
        /// <param name="compiller"></param>
        public void Add(IMessageTextCompiler compiller)
        {
            _list.AddFirst(compiller);
        }

        /// <summary>
        /// Основная процедура
        /// </summary>
        /// <param name="mtu"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public string MakeText(MessageToUser mtu, string text)
        {
            string result = text;
            foreach (IMessageTextCompiler compiller in _list)
            {
                result = compiller.MakeText(mtu, result);                    
            }
            return result;
        }

        public IEnumerator<IMessageTextCompiler> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
