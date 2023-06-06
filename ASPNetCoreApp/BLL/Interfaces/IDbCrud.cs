using ASPNetCoreApp.DAL.Models;
using ASPNetCoreApp.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    // Определяем интерфейс для работы с базой данных
    public interface IDbCrud
    {
        // Получение всех клиентов из базы данных
        IEnumerable<Клиент> GetAllКлиент();
        // Получение клиента по его идентификатору
        Клиент GetКлиент(int clvId);
        // Создание нового клиента в базе данных
        void CreateКлиент(Клиент cl);
        // Обновление информации о клиенте в базе данных
        void UpdateКлиент(Клиент cl);
        // Удаление клиента из базы данных по его идентификатору
        void DeleteКлиент(int id);

        // Получение всех тарифов из базы данных
        IEnumerable<Тариф> GetAllТариф();
        // Получение тарифа по его идентификатору
        Тариф GetТариф(int trfId);
        // Создание нового тарифа в базе данных
        void CreateТариф(Тариф trf);
        // Обновление информации о тарифе в базе данных
        void UpdateТариф(Тариф trf);
        // Удаление тарифа из базы данных по его идентификатору
        void DeleteТариф(int id);

        // Получение всех договоров из базы данных
        IEnumerable<Dogovor> GetAllDogovor();
        // Получение договора по его идентификатору
        Dogovor GetDogovor(int dgvId);
        // Создание нового договора в базе данных
        void CreateDogovor(Dogovor dgv);
        // Обновление информации о договоре в базе данных
        void UpdateDogovor(Dogovor dgv);
        // Удаление договора из базы данных по его идентификатору
        void DeleteDogovor(int id);
        // Сохранение изменений в базе данных
        int Save();
    }
}