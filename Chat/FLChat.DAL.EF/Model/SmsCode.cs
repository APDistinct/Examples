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
    
    public partial class SmsCode
    {
        public System.Guid UserId { get; set; }
        public int Code { get; set; }
        public System.DateTime IssueDate { get; set; }
        public int ExpireBySec { get; set; }
    
        public virtual User User { get; set; }
    }
}
