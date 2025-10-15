using GymMangementDAL.Data.Contexts;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext _dbContext;

        public PlanRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Plan> GetAll()=> _dbContext.Plans.ToList();
     

        public Plan? GetById(int id)=> _dbContext.Plans.Find(id);
        

        //public int Add(Plan plan)
        //{
        //    _dbContext.Plans.Add(plan);
        //    return _dbContext.SaveChanges();
        //}

        public int Update(Plan plan)
        {
            _dbContext.Plans.Update(plan);
            return _dbContext.SaveChanges();
        }

        //public int Delete(int id)
        //{
        //    var plan = _dbContext.Plans.Find(id);
        //    if (plan == null) return 0;

        //    _dbContext.Plans.Remove(plan);
        //    return _dbContext.SaveChanges();
        //}
    }
}
