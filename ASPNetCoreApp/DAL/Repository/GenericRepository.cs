using ASPNetCoreApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace ASPNetCoreApp.DAL.Repository
{
    // Класс, реализующий универсальный репозиторий для работы с сущностями
    // TEntity - тип сущности, с которой работает репозиторий
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal OperatorContext context; // Контекст базы данных
        internal DbSet<TEntity> dbSet; // Набор объектов сущности

        // Конструктор класса, принимающий контекст базы данных
        public GenericRepository(OperatorContext context)
        {
            try
            {
                this.context = context;
                this.dbSet = context.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error initializing GenericRepository", ex);
            }
        }

        // Метод для получения коллекции сущностей с возможностью фильтрации, сортировки и подгрузки связанных сущностей
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null, // Фильтр для выборки сущностей
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, // Функция для сортировки сущностей
            string includeProperties = "") // Список связанных сущностей, которые нужно подгрузить
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                if (filter != null)
                {
                    query = query.Where(filter); // Применяем фильтр к коллекции
                }

                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty); // Подгружаем связанные сущности
                }

                if (orderBy != null)
                {
                    return orderBy(query).ToList(); // Сортируем сущности и возвращаем результат
                }
                else
                {
                    return query.ToList(); // Возвращаем неотсортированный список сущностей
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Get method of GenericRepository", ex);
            }
        }

        // Метод для получения сущности по ее идентификатору
        public virtual TEntity GetByID(object id)
        {
            try
            {
                return dbSet.Find(id); // Ищем сущность по ее идентификатору
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetByID method of GenericRepository", ex);
            }
        }

        // Метод для добавления новой сущности в коллекцию
        public virtual void Insert(TEntity entity)
        {
            try
            {
                dbSet.Add(entity); // Добавляем новую сущность в коллекцию
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Insert method of GenericRepository", ex);
            }
        }

        // Метод для удаления сущности из коллекции по ее идентификатору
        public virtual void Delete(object id)
        {
            try
            {
                TEntity entityToDelete = dbSet.Find(id); // Ищем сущность по ее идентификатору
                Delete(entityToDelete); // Удаляем найденную сущность
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Delete method of GenericRepository", ex);
            }
        }

        // Метод для удаления сущности из базы данных
        public virtual void Delete(TEntity entityToDelete)
        {
            try
            {
                // Проверяем, не отсоединена ли сущность от контекста
                if (context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    // Если отсоединена, то присоединяем её
                    dbSet.Attach(entityToDelete);
                }
                // Удаляем сущность из набора
                dbSet.Remove(entityToDelete);
            }
            catch (Exception ex)
            {
                // Если произошла ошибка, то  исключение с подробной информацией
                throw new Exception("Error in Delete method of GenericRepository", ex);
            }
        }

        // Метод для обновления сущности в базе данных
        public virtual void Update(TEntity entityToUpdate)
        {
            try
            {
                // Присоединяем сущность к контексту
                dbSet.Attach(entityToUpdate);
                // Устанавливаем состояние сущности на изменённое
                context.Entry(entityToUpdate).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                // Если произошла ошибка, то  исключение с подробной информацией
                throw new Exception("Error in Update method of GenericRepository", ex);
            }
        }
    }
}