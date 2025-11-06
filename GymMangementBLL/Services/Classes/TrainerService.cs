using AutoMapper;
using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.TrainerViewModel;
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
    public class TrainerService : ITrainerService
    {

        #region Fields & Constructor
        private readonly IUnitOfWork _uintOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork uintOfWork, IMapper mapper)
        {
            _uintOfWork = uintOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Get All Trainers
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _uintOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any())
                return Enumerable.Empty<TrainerViewModel>();

            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }
        #endregion

        #region Create Trainer
        // Fix typo: CreateTariner -> CreateTrainer
        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                if (IsEmailExists(createTrainer.Email) || IsPhoneExists(createTrainer.Phone))
                    return false;

                var trainer = _mapper.Map<Trainer>(createTrainer);
                _uintOfWork.GetRepository<Trainer>().Add(trainer);
                return _uintOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--- CREATE TRAINER FAILED: {ex.Message} ---");
                return false;
            }
        }
        #endregion

        #region Get Trainer Details
        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer = _uintOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;
            return _mapper.Map<TrainerViewModel>(trainer);
        }
        #endregion

        #region Get Trainer To Update
        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var trainer = _uintOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;
            return _mapper.Map<TrainerToUpdateViewModel>(trainer);
        }
        #endregion

        #region Update Trainer Details
        // Use TrainerToUpdateViewModel
        public bool UpdateTrainerDetails(int trainerId, TrainerToUpdateViewModel updateTrainer)
        {
            try
            {
                var repo = _uintOfWork.GetRepository<Trainer>();
                var trainerToUpdate = repo.GetById(trainerId);
                if (trainerToUpdate is null) return false;

                // Check for duplicates using helper methods
                if (IsEmailExists(updateTrainer.Email, trainerId) || IsPhoneExists(updateTrainer.Phone, trainerId))
                {
                    return false; // Email or phone taken by another trainer
                }

                _mapper.Map(updateTrainer, trainerToUpdate); // Map changes
                trainerToUpdate.UpdatedAt = DateTime.Now; // Update timestamp

                return _uintOfWork.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--- UPDATE TRAINER FAILED: {ex.Message} ---");
                return false;
            }
        }
        #endregion

        #region Remove Trainer
        public bool RemoveTrainer(int trainerId)
        {
            var Repo = _uintOfWork.GetRepository<Trainer>();
            var TrainerToRemove = Repo.GetById(trainerId);
            if (TrainerToRemove is null || HasActiveSessions(trainerId)) return false;
            Repo.Delete(TrainerToRemove);
            return _uintOfWork.SaveChanges() > 0;
        }
        #endregion

        #region Helper Methods
        public bool IsEmailExists(string Email)
        {
            return _uintOfWork.GetRepository<Member>().GetAll(X => X.Email == Email).Any();
        }

        public bool IsPhoneExists(string Phone)
        {
            return _uintOfWork.GetRepository<Member>().GetAll(X => X.Phone == Phone).Any();
        }

        public bool IsEmailExists(string Email, int memberIdToExclude)
        {
            return _uintOfWork.GetRepository<Member>()
                .GetAll(X => X.Email == Email && X.Id != memberIdToExclude)
                .Any();
        }

        public bool IsPhoneExists(string Phone, int memberIdToExclude)
        {
            return _uintOfWork.GetRepository<Member>()
                .GetAll(X => X.Phone == Phone && X.Id != memberIdToExclude)
                .Any();
        }

        // This method is correct and still needed
        public bool HasActiveSessions(int MemberId)
        {
            var memberSessions = _uintOfWork.GetRepository<MemberSession>()
                .GetAll(x => x.MemberId == MemberId)
                .ToList();

            if (!memberSessions.Any()) return false;

            var sessionIds = memberSessions.Select(s => s.SessionId).ToList();

            bool hasActiveMemberSessions = _uintOfWork.GetRepository<Session>()
                .GetAll(x => sessionIds.Contains(x.Id) && x.StartDate > DateTime.Now)
                .Any();

            return hasActiveMemberSessions;
        }
        #endregion
    }
}


