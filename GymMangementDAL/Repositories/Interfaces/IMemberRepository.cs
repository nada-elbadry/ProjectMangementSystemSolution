using GymMangementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Repositories.Interfaces
{
    internal interface IMemberRepository
    {
        //GetAll
        IEnumerable<Member> GetAll();
        //Get By Id
        Member? GetById(int Id);
        //Add
        int Add(Member member);
        //Update
        int Update(Member member);
        //Delete
        int Delete(int Id);
    }
}
