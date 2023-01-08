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
    
    public partial class StructureNode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StructureNode()
        {
            this.Childs = new HashSet<StructureNode>();
            this.NodeSegments = new HashSet<StructureNodeSegment>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> ParentNodeId { get; set; }
        public bool IsShowSegments { get; set; }
        public bool IsShowParentUsers { get; set; }
        public short Order { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StructureNode> Childs { get; set; }
        public virtual StructureNode Parent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StructureNodeSegment> NodeSegments { get; set; }
    }
}
