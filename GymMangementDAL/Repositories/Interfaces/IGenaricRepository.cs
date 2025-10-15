using GymMangementDAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Repositories.Interfaces
{
    public interface IGenaricRepository<TEntity> where TEntity : BaseEntity , new()
    {
        TEntity? GetById(int id);
        IEnumerable<TEntity> GetAll(Func<TEntity,bool>?condition = null);
         int Add(TEntity entity);
        int Update(TEntity entity);
        int Delete(TEntity entity);
    }
}
