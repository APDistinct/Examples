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
    
    public partial class MessageToSegment
    {
        public System.Guid MsgId { get; set; }
        public System.Guid SegmentId { get; set; }
        public Nullable<int> MaxDeep { get; set; }
        public long Idx { get; set; }
    
        public virtual Message Message { get; set; }
        public virtual Segment Segment { get; set; }
    }
}
