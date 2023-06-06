
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.Linq;
using ASPNetCoreApp.DAL.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

using static System.Net.Mime.MediaTypeNames;

using System.Reflection.Metadata;
using ASPNetCoreApp.DAL.Models;

namespace ASPNetCoreApp.Data
{
    public static class OperatorContextSeed
    {
        public static async Task SeedAsync(OperatorContext context)
        {
            try
            {



                var тарифs = new ASPNetCoreApp.DAL.Models.Тариф[]
                {
                new ASPNetCoreApp.DAL.Models.Тариф {    Дата_открытия=DateTime.Today,
                                   Код_тарифа=1,
                                     Код_типа_тарифа_FK=1,
                                      Минута_межгород_стоимость=1,
                                       Минута_международная_стоимость=10,
                                        Название_тарифа="Black",
                                         Статус="Locked",
                                          Стоимость_перехода=100, },

                };
                foreach (ASPNetCoreApp.DAL.Models.Тариф т in тарифs)
                {
                    context.Тариф.Add(т);
                }
                await context.SaveChangesAsync();
                var клиентs = new ASPNetCoreApp.DAL.Models.Клиент[]
              {
                new ASPNetCoreApp.DAL.Models.Клиент {     Баланс=1,
                                   Номер_клиента = 1,
                                     ФИО = "Иванов И.И." },

              };
                foreach (ASPNetCoreApp.DAL.Models.Клиент к in клиентs)
                {
                    context.Клиент.Add(к);
                }
                await context.SaveChangesAsync();
                var dogovors = new ASPNetCoreApp.DAL.Models.Dogovor[]
                {
new ASPNetCoreApp.DAL.Models.Dogovor {  Дата_расторжения=DateTime.Today,
                      Дата_заключения = DateTime.Today,
                      Код_тарифа_FK=1,
                     Номер_договора=1,
                        Номер_клиента_FK=1,
                         Номер_телефона="1111",
                          Серийный_номер_сим_карты="1111"
    }
            };
                foreach (ASPNetCoreApp.DAL.Models.Dogovor d in dogovors)
                {
                    context.Dogovor.Add(d);
                }
                await context.SaveChangesAsync();
            }

            catch

            {
 throw;
            }
        }
    }
}