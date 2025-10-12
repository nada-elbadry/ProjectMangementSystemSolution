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
    internal class MemberRepository : IMemberRepository
    {
        //public GymDbContext dbContext { get; set; } = new GymDbContext();
        // private readonly GymDbContext _dbContext = new GymDbContext();
        private readonly GymDbContext _dbContext;
        //Ask CLR to inject the instance of GymDbContext
        //DbContext Object Is Injected,Not Creeated Manauly
        public MemberRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(Member member)
        {
            _dbContext.Members.Add(member);
            return _dbContext.SaveChanges();
        }

        public int Delete(int Id)
        {
            var Member = _dbContext.Members.Find(Id);
            if(Member is null ) return 0;
            _dbContext.Members.Remove(Member);
            return _dbContext.SaveChanges();
        }

      
        public IEnumerable<Member> GetAll()=> _dbContext.Members.ToList();
        

        public Member? GetById(int Id) => _dbContext.Members.Find(Id);

      

        public int Update(Member member)
        {
            _dbContext.Members.Update(member);
            return _dbContext.SaveChanges();
        }
        /////////////////////يارب انا تعبت 
    }
}
