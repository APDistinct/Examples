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
    
    public partial class ExternalTransportButton
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string Command { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public bool HideForTemporary { get; set; }
    }
}