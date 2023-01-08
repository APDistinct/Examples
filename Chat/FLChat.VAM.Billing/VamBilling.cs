using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.VAM.Billing
{    
    public class VamBilling
    {
        private string _mainId;

        public VamBilling(string mainId = null)
        {
            _mainId = mainId ?? "";
        }

        /// <summary>
        /// Create new user in VAM
        /// </summary>
        /// <param name="userId">user's Id</param>
        /// <returns></returns>
        private bool CreateUser(string userId)
        {
            bool ret = true;
            return ret;
        }

        /// <summary>
        /// Get user's balance in VAM
        /// </summary>
        /// <param name="userId">user's Id</param>
        /// <param name="balance">user's balance in VAM</param>
        /// <returns></returns>
        public bool GetBalance(string userId, out int balance)
        {
            balance = 0;
            bool ret = true;
            return ret;
        }

        public bool Hold(string userId, string transactionId, int sum)
        {
            bool ret = true;
            return ret;
        }

        public bool UnHold(string transactionId)
        {
            bool ret = true;
            return ret;
        }

        public bool Transfer(string transactionId, string toUserId,  string description = null)
        {
            bool ret = true;
            return ret;
        }
        

    }
}
