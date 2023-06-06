using ASPNetCoreApp.DAL.Models;

namespace ASPNetCoreApp.DAL.Repository
{
    // Определяем интерфейс IDbRepos
    public interface IDbRepos
    {
        // Задаем свойство КлиентRepository типа GenericRepository с параметром Клиент
        GenericRepository<Клиент> КлиентRepository { get; }
        // Задаем свойство ТарифRepository типа GenericRepository с параметром Тариф
        GenericRepository<Тариф> ТарифRepository { get; }
        // Задаем свойство DogovorRepository типа GenericRepository с параметром Dogovor
        GenericRepository<Dogovor> DogovorRepository { get; }
        // Задаем метод Save для сохранения изменений в базе данных
        int Save();
    }
}
