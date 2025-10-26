using GymMangementBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Interfaces
{
    internal interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();

        bool CreateMember(CreatMemberViewModel creatMember);

        MemberViewModel? GetMemberById(int MemberId);

        HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId);

        MemberToUpdateViewModel? GetMemberToUpdate(int MemberId);
        bool UpdateMemberDetails(int MemberId, MemberToUpdateViewModel UpdatedMember);

        bool RemoveMember(int MemberId);    

    }
}
