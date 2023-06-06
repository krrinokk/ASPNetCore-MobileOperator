using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
namespace DAL.Repository
{
    public class DbReposSQL : IDbRepos
    {
        private OperatorContext dbContext;

        private КлиентRepository клиентRepository;
        private ТарифRepositorySQL тарифRepository;
        private DogovorRepository dogovorRepository;

        public DbReposSQL()
        {
            dbContext = new OperatorContext();
        }
        public IRepository<Клиент> Клиент
        {
            get
            {
                if (клиентRepository == null)
                    клиентRepository = new КлиентRepository(dbContext);
                return клиентRepository;
            }
        }

      

        public IRepository<Dogovor> Dogovor
        {
            get
            {
                if (dogovorRepository == null)
                    dogovorRepository = new DogovorRepository(dbContext);
                return dogovorRepository;
            }
        }



        public IRepository<Тариф> Тариф
        {
            get
            {
                if (тарифRepository == null)
                    тарифRepository = new ТарифRepositorySQL(dbContext);
                return тарифRepository;
            }
        }

        public int Save()
        {
            return dbContext.SaveChanges();
        }
    }
}
