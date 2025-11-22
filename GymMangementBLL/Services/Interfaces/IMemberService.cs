using GymMangementBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Interfaces
{
    public interface IMemberService
    { 
        #region Main CRUD Methods
        IEnumerable<MemberViewModel> GetAllMembers();
        bool CreateMember(CreatMemberViewModel createMember);
        MemberViewModel? GetMemberDeails(int MemberId);
 
        MemberToUpdateViewModel? GetMemberToUpdate(int MemberId);
        bool UpdateMemberDetails(int Id, MemberToUpdateViewModel memberToUpdate);
        bool RemoveMember(int MemberId);
    

        HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId);
        #endregion

        #region Validation & Helper Methods
        bool IsEmailExists(string Email);
        bool IsPhoneExists(string Phone);
        bool IsEmailExists(string Email, int memberIdToExclude);
        bool IsPhoneExists(string Phone, int memberIdToExclude);
        bool HasActiveSessions(int MemberId);
        #endregion

    }
}
