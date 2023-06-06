
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DAL.Interfaces;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class DogovorRepository : IRepository<Dogovor>
    {
        private OperatorContext db;

        public DogovorRepository(OperatorContext dbcontext)
        {
            this.db = dbcontext;
        }

        public List<Dogovor> GetList()
        {
            return db.Dogovor.ToList();
        }

        public Dogovor GetItem(int id)
        {
            return db.Dogovor.Find(id);
        }

        public void Create(Dogovor dogovor)
        {
            db.Dogovor.Add(dogovor);
        }

        public void Update(Dogovor dogovor)
        {
            db.Entry(dogovor).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Dogovor item = db.Dogovor.Find(id);
            if (item != null)
                db.Dogovor.Remove(item);
        }

        public int Save()
        {
            return db.SaveChanges();
        }

    }
}