using GymMangementDAL.Data.Contexts;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Repositories.Classes
{
    public class GenaricRepository<TEntity> : IGenaricRepository<TEntity> where TEntity : BaseEntity , new()
    {
        private readonly GymDbContext _dbContext;

        public GenaricRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(TEntity entity) => _dbContext.Set<TEntity>().Add(entity);
          

       

        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? Condition = null)
        {
            if (Condition is null)
                return _dbContext.Set<TEntity>().AsNoTracking().ToList();
            else
        return _dbContext.Set<TEntity>().AsNoTracking().Where(Condition).ToList();
        }

        //public IEnumerable<TEntity> GetAll()=> _dbContext.Set<TEntity>().AsNoTracking().ToList();


        public TEntity? GetById(int id)=>_dbContext.Set<TEntity>().Find(id);

        public void Update(TEntity entity)=> _dbContext.Set<TEntity>().Update(entity);
         
    }
}
