using AutoMapper;
using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.PlanViewModels;
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
    internal class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans == null || !plans.Any()) return Enumerable.Empty<PlanViewModel>();


            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);
        }

        public PlanViewModel? GetPlanDetails(int PlanId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null) return null;


            return _mapper.Map<PlanViewModel>(plan);
        }
       
        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || plan.IsActive == false || HasActiveMembership(PlanId)) return null;

            return _mapper.Map<UpdatePlanViewModel>(plan);
        }
        //Soft Delete
        public bool ToggleStatus(int PlanId)
        {
            var planRepo = _unitOfWork.GetRepository<Plan>();
            var plan = planRepo.GetById(PlanId);

            if (plan is null || HasActiveMembership(PlanId)) return false;

            // Toggle the status
            plan.IsActive = !plan.IsActive;
            try
            {
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdatePlan(int PlanId, UpdatePlanViewModel updatedPlan)
        {
            try
            {
                var planRepo = _unitOfWork.GetRepository<Plan>();
                var plan = planRepo.GetById(PlanId);

                if (plan is null || HasActiveMembership(PlanId)) return false;


                _mapper.Map(updatedPlan, plan);
                plan.UpdatedAt = DateTime.Now;

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
            return _unitOfWork.GetRepository<Membership>()
                .GetAll(x => x.PlanId == PlanId && x.Status == "Active")
                .Any();
        }
        #endregion

    }
}
