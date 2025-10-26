using AutoMapper;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.SessionViewModels;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Classes
{
    public class SessionService11 : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService11(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(SessionViewModel CreatedSession)
        {
            try
            {

                //Check if Trainer is exists
                if (!IsTrainerExists(CreatedSession.Id))
                    return false;
                //Check if Category is exists
                if (!IsCategoryExists(CreatedSession.Id))
                    return false;
                //Check if StartDate is before EndDate
                if (!IsDateTimeValid(CreatedSession.StartDate, CreatedSession.EndDate))
                    return false;

                if (CreatedSession.Capacity > 25 || CreatedSession.Capacity < 0) return false;
                var SessionEntity = _mapper.Map<Session>(CreatedSession);
                _unitOfWork.GetRepository<Session>().Add(SessionEntity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create Session Failed{ex}");
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionWithTrainerAndCategory();
            if (!Sessions.Any()) return [];
            /*
            //return Sessions.Select(s => new SessionViewModel
            //{
            //    Id= s.Id,
            //    Description= s.Description,
            //    StartDate = s.StartDate,
            //    EndDate = s.EndDate,
            //    Capacity= s.Capacity,
            //   TrainerName= s.TrainerName,
            //   CategoryName=s.Category.CategoryName,
            //   AvailableSlots=s.Capacity-_unitOfWork.SessionRepository.GetCountOfBookedSlots(s.Id),
            //    //TrainerName=s.TrainerName,//Related Data
            //  //  AvailableSlots --Computed [Capacity - CountOg Booking For Session]

            //});
            */
            var MappedSessions = _mapper.Map<IEnumerable<Session>,IEnumerable<SessionViewModel>>(Sessions);
            foreach (var session in MappedSessions)
             session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
                return MappedSessions;
        }

        public SessionViewModel? GetSessionById(int sessionid)
        {
            var Session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionid);
            if (Session is null) return null;
             var MappedSession = _mapper.Map<Session, SessionViewModel>(Session);
            MappedSession.AvailableSlots = MappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(MappedSession.Id);
            return MappedSession;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {

            var Session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (IsSessionAvailableForUpdating(Session!)) return null;

            return _mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(UpdateSessionViewModel updatedSession, int sessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForUpdating(Session!)) return false;
                if(!IsTrainerExists(updatedSession.TrainerId)) return false;
                if(!IsDateTimeValid(updatedSession.StartDate , updatedSession.EndDate)) return false;
                _mapper.Map(updatedSession, Session);
                Session!.UpdatedAt = DateTime.Now;
                _unitOfWork.SessionRepository.Update(Session);
                return _unitOfWork.SaveChanges()>0;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Update Session Failed : {ex}");
                return false;
            }
        }

        public bool RemoveSession(int sessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Remove Session Failed : {ex}");
            }
        }


        #region Helper Method
        private bool IsSessionAvailableForUpdating(Session session)
        {
            if(session is null) return false;
            // if session completed - no updated allowed
            if(session.EndDate<DateTime.Now)return false;
            //if Session Started - no updated allowed
            if(session.StartDate<=DateTime.Now) return false;
            //if sessionhas active booking - no updated allowed
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id)>0;
            if(HasActiveBooking) return false;
             return true;
        }

        private bool IsSessionAvailableForDeleting(Session session)
        {
            if (session is null) return false;
            // if session completed - no delete allowed
            if (session.EndDate < DateTime.Now) return false;
            //if Session Started - no delete allowed
            if (session.StartDate <= DateTime.Now) return false;
            //if sessionhas active booking - no delete allowed
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBooking) return false;
            return true;
        }
        private bool IsTrainerExists(int TrainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        }

        private bool IsCategoryExists(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }

        private bool IsDateTimeValid(DateTime StartDate,DateTime EndDate)
        {
            return StartDate < EndDate;
        }

       

        #endregion
    }
}
