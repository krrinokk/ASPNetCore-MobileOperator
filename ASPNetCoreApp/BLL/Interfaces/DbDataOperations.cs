using BLL.Interfaces;
using DAL;
using System.Collections.Generic;
using System.Linq;
using ASPNetCoreApp.DAL.Models;
using System.Collections.ObjectModel;
using ASPNetCoreApp.DAL.Models;
using ASPNetCoreApp.DAL.Repository;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BLL.Interfaces
{
    // Класс для работы с данными из базы данных
    public class DbDataOperations : IDbCrud
    {
        IDbRepos db;

        // Конструктор класса, принимающий интерфейс IDbRepos
        public DbDataOperations(IDbRepos repos)
        {
            this.db = repos;
        }

        // Получение договора по ID
        public Dogovor GetDogovor(int id)
        {
            return db.DogovorRepository.GetByID(id);
        }

        // Получение всех договоров
        public IEnumerable<Dogovor> GetAllDogovor()
        {
            return db.DogovorRepository.Get();
        }

        // Создание нового договора
        public void CreateDogovor(Dogovor dogovor)
        {
            db.DogovorRepository.Insert(dogovor);
            Save();
        }

        // Обновление информации о договоре
        public void UpdateDogovor(Dogovor dogovor)
        {
            db.DogovorRepository.Update(dogovor);
            Save();
        }

        // Удаление договора по ID
        public void DeleteDogovor(int id)
        {
            db.DogovorRepository.Delete(id);
            Save();
        }

        // Получение клиента по ID
        public Клиент GetКлиент(int id)
        {
            return db.КлиентRepository.GetByID(id);
        }

        // Получение всех клиентов
        public IEnumerable<Клиент> GetAllКлиент()
        {
            return db.КлиентRepository.Get();
        }

        // Создание нового клиента
        public void CreateКлиент(Клиент клиент)
        {
            db.КлиентRepository.Insert(клиент);
            Save();
        }

        // Обновление информации о клиенте
        public void UpdateКлиент(Клиент клиент)
        {
            db.КлиентRepository.Update(клиент);
            Save();
        }

        // Удаление клиента по ID
        public void DeleteКлиент(int id)
        {
            db.КлиентRepository.Delete(id);
            Save();
        }

        // Получение тарифа по ID
        public Тариф GetТариф(int id)
        {
            return db.ТарифRepository.GetByID(id);
        }

        // Получение всех тарифов
        public IEnumerable<Тариф> GetAllТариф()
        {
            return db.ТарифRepository.Get();
        }

        // Создание нового тарифа
        public void CreateТариф(Тариф тариф)
        {
            db.ТарифRepository.Insert(тариф);
            Save();
        }

        // Обновление информации о тарифе
        public void UpdateТариф(Тариф тариф)
        {
            db.ТарифRepository.Update(тариф);
            Save();
        }

        // Удаление тарифа по ID
        public void DeleteТариф(int id)
        {
            db.ТарифRepository.Delete(id);
            Save();
        }

        // Сохранение изменений в базе данных
        public int Save()
        {
            if (db.Save() > 0) return 0;
            return 1;
        }
    }
}