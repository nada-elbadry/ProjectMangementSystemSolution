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

        /*
        //private readonly IUnitOfWork _unitOfWork;

        //public TrainerService(UnitOfWork unitOfWork)
        //{
        //    _unitOfWork = uow;
        //}

        //public IEnumerable<TrainerListItemViewModel> GetAll()
        //{
        //    var repo = _uow.GetRepository<Trainer>();
        //    var trainers = repo.GetAll(); // returns IEnumerable<Trainer>
        //    return trainers.Select(t => new TrainerListItemViewModel
        //    {
        //        Id = t.Id,
        //        Name = t.Name,
        //        Email = t.Email,
        //        Phone = t.Phone,
        //        Specialization = t.Specialties.ToString()
        //    });
        //}

        //public TrainerDetailsViewModel? GetById(int id)
        //{
        //    var repo = _uow.GetRepository<Trainer>();
        //    var trainer = repo.GetById(id);
        //    if (trainer == null) return null;

        //    // HireDate stored in CreatedAt per your config
        //    return new TrainerDetailsViewModel
        //    {
        //        Id = trainer.Id,
        //        TrainerName = trainer.Name,
        //        Specialization = trainer.Specialties.ToString(),
        //        Email = trainer.Email,
        //        Phone = trainer.Phone,
        //        DateOfBirth = trainer.DateOfBirth,
        //        AddressDisplay = $"{trainer.Address.BuildingNumber}-{trainer.Address.Street}-{trainer.Address.City}",
        //        HireDate = trainer.CreatedAt
        //    };
        //}

        //public bool Create(CreateTrainerViewModel model)
        //{
        //    var repo = _uow.GetRepository<Trainer>();

        //    // validation: specialty required
        //    if (model.Specialties == 0) return false;

        //    // unique Email and Phone
        //    var emailExists = repo.GetAll(t => t.Email == model.Email).Any();
        //    var phoneExists = repo.GetAll(t => t.Phone == model.Phone).Any();
        //    if (emailExists || phoneExists) return false;

        //    var trainer = new Trainer
        //    {
        //        Name = model.Name,
        //        Email = model.Email,
        //        Phone = model.Phone,
        //        DateOfBirth = model.DateOfBirth,
        //        Address = model.Address,
        //        Specialties = (GymMangementDAL.Entities.Enums.Specialties)model.Specialties,
        //        CreatedAt = DateTime.Now // hire date
        //    };

        //    repo.Add(trainer);
        //    _uow.SaveChanges();
        //    return true;
        //}

        //public bool Update(UpdateTrainerViewModel model)
        //{
        //    var repo = _uow.GetRepository<Trainer>();
        //    var trainer = repo.GetById(model.Id);
        //    if (trainer == null) return false;

        //    // check uniqueness for email/phone excluding current trainer
        //    var emailExists = repo.GetAll(t => t.Email == model.Email && t.Id != model.Id).Any();
        //    var phoneExists = repo.GetAll(t => t.Phone == model.Phone && t.Id != model.Id).Any();
        //    if (emailExists || phoneExists) return false;

        //    trainer.Name = model.Name;
        //    trainer.Email = model.Email;
        //    trainer.Phone = model.Phone;
        //    trainer.DateOfBirth = model.DateOfBirth;
        //    trainer.Specialties = (GymMangementDAL.Entities.Enums.Specialties)model.Specialties;
        //    trainer.Address.BuildingNumber = model.BuildingNumber;
        //    trainer.Address.Street = model.Street;
        //    trainer.Address.City = model.City;
        //    trainer.UpdatedAt = DateTime.Now;

        //    repo.Update(trainer);
        //    _uow.SaveChanges();
        //    return true;
        //}

        //public bool Delete(int id, out string errorMessage)
        //{
        //    errorMessage = string.Empty;
        //    var repo = _uow.GetRepository<Trainer>();
        //    var trainer = repo.GetById(id);
        //    if (trainer == null) { errorMessage = "Trainer not found."; return false; }

        //    // cannot delete trainers with future sessions
        //    var sessionRepo = _uow.GetRepository<Session>();
        //    var hasFuture = sessionRepo.GetAll(s => s.TrainerId == id && s.StartDate > DateTime.Now).Any();
        //    if (hasFuture)
        //    {
        //        errorMessage = "Cannot delete trainer with future sessions.";
        //        return false;
        //    }

        //    repo.Delete(trainer);
        //    _uow.SaveChanges();
        //    return true;
        //}
        */
        private readonly IUnitOfWork _unitOfWork;
        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<Trainer>();
                if (IsEmailExists(createTrainer.Email )|| IsPhoneExists(createTrainer.Phone)) return false;
                var Trainer = new Trainer()
                {
                    Name = createTrainer.Name,
                    Email = createTrainer.Email,
                    Phone = createTrainer.Phone,
                    DateOfBirth = createTrainer.DateOfBirth,
                    Specialties = createTrainer.Specialties,
                    Gender = createTrainer.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createTrainer.BuldingNumber,
                        Street = createTrainer.Street,
                        City = createTrainer.City
                    }
                };
                Repo.Add(Trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainers is null || !Trainers.Any()) return [];
            return Trainers.Select(t => new TrainerViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialization = t.Specialties.ToString(),

            });
        }
        
        public TrainerViewModel? GetTrainerDetails(int TrainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (Trainer is null) return null;
            return new TrainerViewModel()
            {

                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                Specialization = Trainer.Specialties.ToString(),
            };
        }

        public UpdateTrainerViewModel? GetTrainerToUpdate(int TrainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (Trainer is null) return null;
            return new UpdateTrainerViewModel()
            {

                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                Specialties =Trainer.Specialties,
                BuildingNumber = Trainer.Address.BuildingNumber,
                Street = Trainer.Address.Street,
                City = Trainer.Address.City,
            };
        }
        public bool RemoveTrainer(int TrainerId)
        {

            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToRemove = Repo.GetById(TrainerId);
            if (TrainerToRemove is null || HasActiveSessions(TrainerId)) return false;
            Repo.Delete(TrainerToRemove);
            return _unitOfWork.SaveChanges() > 0;
        }

        public bool UpdateTrainerDetails(UpdateTrainerViewModel updateTrainer, int trainerId)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToUpdate = Repo.GetById(trainerId);
            if (TrainerToUpdate is null || IsEmailExists(updateTrainer.Email) || IsPhoneExists(updateTrainer.Phone)) return false;
            TrainerToUpdate.Name = updateTrainer.Name;
            TrainerToUpdate.Email = updateTrainer.Email;
            TrainerToUpdate.Phone = updateTrainer.Phone;
            TrainerToUpdate.Specialties = updateTrainer.Specialties;
            TrainerToUpdate.Address.BuildingNumber = updateTrainer.BuildingNumber;
            TrainerToUpdate.Address.Street = updateTrainer.Street;
            TrainerToUpdate.Address.City = updateTrainer.City;
            TrainerToUpdate.UpdatedAt = DateTime.Now;
            Repo.Update(TrainerToUpdate);
            return _unitOfWork.SaveChanges() > 0;
        }

        #region Helper Methods
        private bool IsEmailExists(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();

        }
        private bool IsPhoneExists(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone).Any();
        }

        private bool HasActiveSessions(int TrainerId)
        {
            var activeSessions = _unitOfWork.GetRepository<Session>()
                .GetAll(s => s.TrainerId == TrainerId && s.StartDate > DateTime.Now).Any();
            return activeSessions;
        }


        #endregion
    }
}


