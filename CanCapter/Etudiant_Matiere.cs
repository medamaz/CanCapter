//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CanCapter
{
    using System;
    using System.Collections.Generic;
    
    public partial class Etudiant_Matiere
    {
        public int Id_EM { get; set; }
        public int id_E { get; set; }
        public int id_M { get; set; }
    
        public virtual Etudiant Etudiant { get; set; }
        public virtual Matiere Matiere { get; set; }
    }
}
