using System;
using System.Collections.Generic;
using FLChat.DAL.Model;

namespace FLChat.WebService.Handlers.File
{
    public interface IPhonesGetter
    {
        /*IEnumerable<Guid>*/Guid[] GetMatchedPhones(ChatEntities entities, Guid userId, IEnumerable<string> phones);
    }
}