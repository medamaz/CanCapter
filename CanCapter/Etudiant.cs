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
    
    public partial class Etudiant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Etudiant()
        {
            this.Etudiant_Matiere = new HashSet<Etudiant_Matiere>();
            this.Paiements = new HashSet<Paiement>();
            this.Recus = new HashSet<Recu>();
        }
    
        public int Id_E { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public int telephone { get; set; }
        public int telephone_P { get; set; }
        public int telephone_M { get; set; }
        public System.DateTime date_I { get; set; }
        public int id_F { get; set; }
        public bool Statut { get; set; }
    
        public virtual Filier Filier { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Etudiant_Matiere> Etudiant_Matiere { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Paiement> Paiements { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recu> Recus { get; set; }
    }
}
