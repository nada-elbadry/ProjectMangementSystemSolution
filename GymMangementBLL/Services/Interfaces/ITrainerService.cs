
using GymMangementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Interfaces
{
    public interface ITrainerServices
    {
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
