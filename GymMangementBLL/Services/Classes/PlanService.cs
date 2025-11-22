using AutoMapper;
using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.PlanViewModel;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Classes;
using GymMangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _uintOfWork;
        private readonly IMapper _mapper;


        public PlanService(IUnitOfWork uintOfWork, IMapper mapper)
        {
            _uintOfWork = uintOfWork;
            _mapper = mapper;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _uintOfWork.GetRepository<Plan>().GetAll();
            if (plans == null || !plans.Any()) return Enumerable.Empty<PlanViewModel>();


            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);
        }

        public PlanViewModel? GetPlanDetails(int PlanId)
        {
            var plan = _uintOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null) return null;


            return _mapper.Map<PlanViewModel>(plan);
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
            var plan = _uintOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || plan.IsActive == false || HasActiveMemberShip(PlanId)) return null;

            return _mapper.Map<UpdatePlanViewModel>(plan);
        }

        public bool UpdatePlan(int PlanId, UpdatePlanViewModel updatedPlan)
        {
            try
            {
                var planRepo = _uintOfWork.GetRepository<Plan>();
                var plan = planRepo.GetById(PlanId);

                if (plan is null || HasActiveMemberShip(PlanId)) return false;


                _mapper.Map(updatedPlan, plan);
                plan.UpdatedAt = DateTime.Now;

                return _uintOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool ToggleStatus(int PlanId)
        {
            var planRepo = _uintOfWork.GetRepository<Plan>();
            var plan = planRepo.GetById(PlanId);

            if (plan is null || HasActiveMemberShip(PlanId)) return false;

            // Toggle the status
            plan.IsActive = !plan.IsActive;
            try
            {
                return _uintOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        #region Helper Method
        private bool HasActiveMemberShip(int PlanId)
        {
            return _uintOfWork.GetRepository<Membership>()
                .GetAll(x => x.PlanId == PlanId && x.Status == "Active")
                .Any();
        }
        #endregion
    }
}
