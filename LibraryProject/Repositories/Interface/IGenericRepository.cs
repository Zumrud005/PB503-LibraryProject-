using LibraryProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Repositories.Interface
{
    public interface IGenericRepository<T> where T : BaseEntity, new()
    {
        void Add(T entity);
        void Remove(T entity);
        T GetById(int id);
        List<T> GetAll();
        void Commit();

    }
}
