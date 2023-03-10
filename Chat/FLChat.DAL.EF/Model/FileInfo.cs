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
    
    public partial class FileInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FileInfo()
        {
            this.Message = new HashSet<Message>();
        }
    
        public System.Guid Id { get; set; }
        public long Idx { get; set; }
        public System.Guid FileOwnerId { get; set; }
        public System.DateTime LoadDate { get; set; }
        public string FileName { get; set; }
        public int MediaTypeId { get; set; }
        public int FileLength { get; set; }
        public Nullable<int> Width { get; set; }
        public Nullable<int> Height { get; set; }
    
        public virtual MediaType MediaType { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Message { get; set; }
    }
}
