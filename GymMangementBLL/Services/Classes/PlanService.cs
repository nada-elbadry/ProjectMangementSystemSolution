using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.PlanViewModels;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Classes
{
    internal class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any()) return [];
            return plans.Select(p=> new PlanViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                IsActive = p.IsActive,
                Price = p.Price,
            });
        }

        public PlanViewModel? GetPlanById(int PlanId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null) return null;
            return new PlanViewModel()
            {
                Id = Plan.Id,
                Name = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                IsActive = Plan.IsActive,
                Price = Plan.Price,
            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
       var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null || Plan.IsActive == false || HasActiveMembership(PlanId)) return null;
            return new UpdatePlanViewModel()
            {
                PlanName = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                Price = Plan.Price,
            };
        }
        //Soft Delete
        public bool ToggleStatus(int PlanId)
        {
            var Repo = _unitOfWork.GetRepository<Plan>();
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null || HasActiveMembership(PlanId)) return false;
            Plan.IsActive = Plan.IsActive == true ? false : true;
            Plan.UpdatedAt = DateTime.Now;
            try 
            {
                Repo.Update(Plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdatePlan(int PlanId, UpdatePlanViewModel updatedPlan)
        {
           var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan is null || HasActiveMembership(PlanId)) return false;
           
            try
            {
                (plan.Description, plan.DurationDays, plan.Price, plan.UpdatedAt)
         = (updatedPlan.Description, updatedPlan.DurationDays, updatedPlan.Price, DateTime.Now);
                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        #region Helper

        private bool HasActiveMembership(int PlanId)
        {
            var activeMembership = _unitOfWork.GetRepository<Membership>()
                .GetAll(m => m.PlanId == PlanId && m.Status=="Active");


            return activeMembership .Any();
        }
        #endregion

    }
}
