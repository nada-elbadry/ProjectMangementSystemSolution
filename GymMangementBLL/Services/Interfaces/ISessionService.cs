using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymMangementBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionById(int id);

        bool CreateSession(SessionViewModel CreatedSessionodel);

        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);

        bool UpdateSession(UpdateSessionViewModel updateSession , int sessionId);
        bool RemoveSession(int sessionId);

    }
}
