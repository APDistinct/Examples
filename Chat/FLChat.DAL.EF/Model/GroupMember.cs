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
    
    public partial class GroupMember
    {
        public System.Guid GroupId { get; set; }
        public System.Guid UserId { get; set; }
        public bool IsAdmin { get; set; }
    
        public virtual Group Group { get; set; }
        public virtual User CreatedBy { get; set; }
    }
}
