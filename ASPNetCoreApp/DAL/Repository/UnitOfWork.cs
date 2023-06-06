using ASPNetCoreApp.DAL.Models;
using ASPNetCoreApp.DAL.Repository;


namespace ASPNetCoreApp.DAL.Repository
{
    // Класс UnitOfWork реализует интерфейс IDbRepos и предоставляет доступ к репозиториям Dogovor, Клиент и Тариф.
    public class UnitOfWork : IDbRepos
    {
        private OperatorContext context = new OperatorContext();
        private GenericRepository<Dogovor> _dogovorRepository;
        private GenericRepository<Клиент> _клиентRepository;
        private GenericRepository<Тариф> _тарифRepository;

        private readonly ILogger<UnitOfWork> _logger;

        // При создании экземпляра UnitOfWork передается ILogger, который используется для логирования информации о создании экземпляра и создании репозиториев.
        public UnitOfWork(ILogger<UnitOfWork> logger)
        {
            _logger = logger;
            _logger.LogInformation("Creating UnitOfWork instance");
        }

        // Каждый репозиторий представлен свойством с методом get, который при первом обращении создает экземпляр GenericRepository с соответствующим типом и контекстом.
        public GenericRepository<Dogovor> DogovorRepository
        {
            get
            {
                try
                {
                    if (this._dogovorRepository == null)
                    {
                        _logger.LogInformation("Creating DogovorRepository instance");
                        this._dogovorRepository = new GenericRepository<Dogovor>(context);
                    }
                    return _dogovorRepository;
                }
                catch (Exception ex)
                {
                    // Если при создании репозитория возникает исключение, оно логируется и выбрасывается новое исключение с сообщением об ошибке.
                    _logger.LogError(ex, "Error creating DogovorRepository");
                    throw new Exception("Error creating DogovorRepository", ex);
                }
            }
        }

        public GenericRepository<Клиент> КлиентRepository
        {
            get
            {
                try
                {
                    if (this._клиентRepository == null)
                    {
                        _logger.LogInformation("Creating КлиентRepository instance");
                        this._клиентRepository = new GenericRepository<Клиент>(context);
                    }
                    return _клиентRepository;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating КлиентRepository");
                    throw new Exception("Error creating КлиентRepository", ex);
                }
            }
        }

        public GenericRepository<Тариф> ТарифRepository
        {
            get
            {
                try
                {
                    if (this._тарифRepository == null)
                    {
                        _logger.LogInformation("Creating ТарифRepository instance");
                        this._тарифRepository = new GenericRepository<Тариф>(context);
                    }
                    return _тарифRepository;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating ТарифRepository");
                    throw new Exception("Error creating ТарифRepository", ex);
                }
            }
        }



        // Метод Save() сохраняет изменения в контексте данных в базу данных.
        public int Save()
        {
            try
            {
                _logger.LogInformation("Saving changes to the database");
                context.SaveChanges();
                return 0;
            }
            catch (Exception ex)
            {
                // Если при сохранении изменений возникает исключение, оно логируется и выбрасывается новое исключение с сообщением об ошибке.
                _logger.LogError(ex, "Error saving changes to the database");
                throw new Exception("Error saving changes to the database", ex);
            }
        }

        // Метод Dispose освобождает ресурсы, используемые контекстом данных, и вызывается при уничтожении объекта UnitOfWork.
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Освобождение ресурсов контекста данных.
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            // Сигнализация сборщику мусора о том, что объект уже освобожден.
            GC.SuppressFinalize(this);
        }
    }
}
