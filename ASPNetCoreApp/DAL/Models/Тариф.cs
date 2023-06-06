using Microsoft.Extensions.Hosting;
using ASPNetCoreApp.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASPNetCoreApp.DAL.Models
{
    public class Тариф
    {
        public decimal? Минута_межгород_стоимость { get; set; }

        public decimal Минута_международная_стоимость { get; set; }

        public string Название_тарифа { get; set; } = null!;

        public decimal Стоимость_перехода { get; set; }

        public int Код_типа_тарифа_FK { get; set; }
        [Key]
        public int Код_тарифа { get; set; }

        public string Статус { get; set; }

        public DateTime Дата_открытия { get; set; }
        public Тариф()
        {
            Dogovor = new HashSet<Dogovor>();
        }
        public virtual ICollection<Dogovor>? Dogovor { get; set; }














    }
}
