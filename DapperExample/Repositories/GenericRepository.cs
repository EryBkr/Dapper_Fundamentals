using Dapper.Contrib.Extensions;
using DapperExample.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample.Repositories
{
    public class GenericRepository<T> where T:class,new()
    {
        public List<T> GetAll()
        {
            var con = Db.GetConnection();
            return con.GetAll<T>().ToList();
        }

        public T GetById(int id)
        {
            var con = Db.GetConnection();
            return con.Get<T>(id);
        }

        public void Add(T entity)
        {
            var con = Db.GetConnection();
            con.Insert(entity);
        }

        public void Update(T entity)
        {
            var con = Db.GetConnection();
            con.Update(entity);
        }

        public void Delete(T entity)
        {
            var con = Db.GetConnection();
            con.Delete(entity);
        }
    }
}
