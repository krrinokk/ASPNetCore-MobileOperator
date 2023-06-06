namespace ASPNetCoreApp.DAL.Models
{
  
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class Клиент
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Клиент()
        {
            Dogovor = new HashSet<Dogovor>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Номер_клиента { get; set; }

        [StringLength(50)]
        public string ФИО { get; set; }

        [Column(TypeName = "money")]
        public decimal? Баланс { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dogovor>? Dogovor { get; set; }
    }
}
