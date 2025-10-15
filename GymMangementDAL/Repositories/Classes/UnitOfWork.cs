using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangementDAL.Data.Contexts;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces; // Add this using directive if IUnitOfWork is in this namespace

namespace GymMangementDAL.Repositories.Classes
{
    public class UnitOfWork:IUnitOfWork 
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly GymDbContext _dbContext;

        public UnitOfWork(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenaricRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            //TEntity = Member
            var EntityType = typeof(TEntity);
            if(_repositories.TryGetValue(EntityType,out var Repo))
                return (IGenaricRepository<TEntity>) Repo;
            var NewRepo = new GenaricRepository<TEntity>(_dbContext);
            _repositories[EntityType] = NewRepo;
            return NewRepo;

        }
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
