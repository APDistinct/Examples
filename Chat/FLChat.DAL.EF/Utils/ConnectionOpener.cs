using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Utils {

    /// <summary>
    /// Ensure connection is open and open it if nessesary. Close connection after use if connection has opened by that class
    /// </summary>
    public class ConnectionOpener : IDisposable
    {
        private ChatEntities _entities;
        private bool _needClose;

        public ConnectionOpener(ChatEntities entities) {
            _entities = entities;
            
            if (entities.Database.Connection.State == ConnectionState.Closed) {
                entities.Database.Connection.Open();
                _needClose = true;
            } else
                _needClose = false;
        }

        public void Dispose() {
            if (_needClose)
                _entities.Database.Connection.Close();
        }
    }
}
