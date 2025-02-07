using LibraryProject.Data;
using LibraryProject.Models;
using LibraryProject.Repositories.Interface;

namespace LibraryProject.Repositories.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        public readonly AppDbContext _appDbContext;
        public GenericRepository()
        {
            _appDbContext = new AppDbContext();
        }
        public void Add(T entity)
        => _appDbContext.Set<T>().Add(entity);

        public void Commit()
        {
           
            try
            {
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eror: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Eror: {ex.InnerException.Message}");
                }
                throw;
            }
        }


            public List<T> GetAll()
            => _appDbContext.Set<T>().ToList();

            public T GetById(int id)
            => _appDbContext.Set<T>().FirstOrDefault(x => x.Id == id);

            public void Remove(T entity)
            => _appDbContext.Set<T>().Remove(entity);
        }
    }
