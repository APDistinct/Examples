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
    
    public partial class LastMessageView
    {
        public System.Guid UserId { get; set; }
        public System.Guid UserOppId { get; set; }
        public System.Guid MsgId { get; set; }
        public long MsgIdx { get; set; }
        public int ToTransportTypeId { get; set; }
        public Nullable<bool> Income { get; set; }
        public long MsgToUserIdx { get; set; }
        public Nullable<int> UnreadCnt { get; set; }
    }
}