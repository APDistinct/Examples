//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FLChat.DAL.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class AuthToken
    {
        public long Id { get; set; }
        public System.Guid UserId { get; set; }
        public string Token { get; set; }
        public System.DateTime IssueDate { get; set; }
        public int ExpireBy { get; set; }
    
        public virtual User User { get; set; }
    }
}
