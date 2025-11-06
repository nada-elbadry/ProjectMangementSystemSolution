using GymMangementBLL.ViewModels.TrainerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        /*
           // Get all trainers for Index page
          IEnumerable<TrainerListItemViewModel> GetAll();

          // Get specific trainer details by id
          TrainerDetailsViewModel? GetById(int id);

          // Create new trainer
          bool Create(CreateTrainerViewModel model);

          // Update trainer data
          bool Update(UpdateTrainerViewModel model);

          // Delete trainer (returns false + error message if not allowed)
          bool Delete(int id, out string errorMessage);

          */
        //bool CreateTrainer(CreateTrainerViewModel createTrainer);
        //UpdateTrainerViewModel? GetTrainerToUpdate(int TrainerId);
        //bool UpdateTrainerDetails(UpdateTrainerViewModel updateTrainer , int trainerId);

        //bool RemoveTrainer(int TrainerId);
        //IEnumerable<TrainerViewModel> GetAllTrainers();
        //TrainerViewModel? GetTrainerDetails(int TrainerId);
        #region Main CRUD Methods
        IEnumerable<TrainerViewModel> GetAllTrainers();

        bool CreateTrainer(CreateTrainerViewModel createTrainer);
        TrainerViewModel? GetTrainerDetails(int trainerId);
        TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId);

        bool UpdateTrainerDetails(int trainerId, TrainerToUpdateViewModel updateTrainer);
        bool RemoveTrainer(int trainerId);
        #endregion

        #region Validation & Helper Methods

        bool IsEmailExists(string email);
        bool IsPhoneExists(string phone);
        bool IsEmailExists(string email, int trainerIdToExclude);
        bool IsPhoneExists(string phone, int trainerIdToExclude);
        bool HasActiveSessions(int trainerId);
        #endregion
    }
}
