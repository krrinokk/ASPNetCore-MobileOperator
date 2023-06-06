using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using ASPNetCoreApp.DAL.Models;
namespace ASPNetCoreApp.DAL.Models
{
    public partial class Dogovor
    {
      
      
        public DateTime Дата_заключения { get; set; }
        [Key]
        public int Номер_договора { get; set; }

        public string Номер_телефона { get; set; } = null!;

        public string Серийный_номер_сим_карты { get; set; } = null!;

        public DateTime Дата_расторжения { get; set; }

        public int Код_тарифа_FK { get; set; }

        public int Номер_клиента_FK { get; set; }

      public virtual Тариф? Тариф { get; set; } = null!;
       public virtual Клиент? Клиент { get; set; } = null!;

    }
}
