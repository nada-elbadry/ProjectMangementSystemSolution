using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.AnalysticsViewModel;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public AnalyticsViewModel GetAnalyticsData()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAll();
            return new AnalyticsViewModel
            {
                ActiveMembers = _unitOfWork.GetRepository<Membership>().GetAll(x=>x.Status == "Active").Count(),
                TotalMembers = _unitOfWork.GetRepository<Membership>().GetAll().Count(),
                TotalTrainers = _unitOfWork.GetRepository<Session>().GetAll().Count(),
                UpcomingSessions= Sessions.Count(x => x.StartDate > DateTime.Now),
                OngoingSessions = Sessions.Count(x=>x.StartDate<=DateTime.Now && x.EndDate>=DateTime.Now),
                CompletedSessions = Sessions.Count(X => X.EndDate < DateTime.Now),
            };

        }
    }
}
