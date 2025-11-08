using AutoMapper;
using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.MemberViewModels;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Classes;
using GymMangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GymMangementBLL.Services.Classes.MemberService;


namespace GymMangementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {

            private readonly IUnitOfWork _uintOfWork;
            private readonly IMapper _mapper;

            public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _uintOfWork = unitOfWork;
                _mapper = mapper;
            }
            

            #region Get All Members
            public IEnumerable<MemberViewModel> GetAllMbers()
            {
                var Members = _uintOfWork.GetRepository<Member>().GetAll();
                if (Members == null || !Members.Any()) return [];

                var MemberViewModels = _mapper.Map<IEnumerable<MemberViewModel>>(Members);
                return MemberViewModels;
            }
        #endregion

           #region Create Member
           public bool CreateMember(CreatMemberViewModel createMember)
            {
                try
                {
                //If One Of Them Exists , Return False
                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone)) return false;
                //If Not Add Member And Return True If Added Successfully
                var member = _mapper.Map<Member>(createMember);

                    _uintOfWork.GetRepository<Member>().Add(member);
                    return _uintOfWork.SaveChanges() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in CreateMember: {ex.Message}");
                    throw;
                }
            }
            #endregion

            #region Get Member Details
            public MemberViewModel? GetMemberDeails(int MemberId)
            {
                var member = _uintOfWork.GetRepository<Member>().GetById(MemberId);
                if (member == null) return null;

                var viewModel = _mapper.Map<MemberViewModel>(member);

                var activeMemberShip = _uintOfWork.GetRepository<Membership>()
                    .GetAll(x => x.MemberId == MemberId && x.Status == "Active")
                    .FirstOrDefault();

                if (activeMemberShip is not null)
                {
                    viewModel.MembershipStartDate = activeMemberShip.CreatedAt.ToShortDateString();
                    viewModel.MembershipEndDate = activeMemberShip.EndDate.ToShortDateString();
                    var plan = _uintOfWork.GetRepository<Plan>().GetById(activeMemberShip.PlanId);
                    viewModel.PlanName = plan?.Name;
                }

                return viewModel;
            }
            #endregion

            #region Get Health Record
            public HealthRecordViewModel? GetMemberHealthRecordDetails(int Memberid)
            {
                // Based on your ModelSnapshot, HealthRecord is mapped to the Members table.
                var memberHealthRecord = _uintOfWork.GetRepository<HealthRecord>().GetById(Memberid);
                if (memberHealthRecord == null) return null;

                return _mapper.Map<HealthRecordViewModel>(memberHealthRecord);
            }
            #endregion

            #region Get Member for Update
            public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
            {
                var member = _uintOfWork.GetRepository<Member>().GetById(MemberId);
                if (member == null) return null;

                return _mapper.Map<MemberToUpdateViewModel>(member);
            }
        #endregion

        #region Update Member
        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel UpdatedMember)
        {
            //If One Of Them Exists , Return False
            if (IsEmailExists(UpdatedMember.Email) || IsPhoneExists(UpdatedMember.Phone)) return false;
            var MemberRepo = _uintOfWork.GetRepository<Member>();
            var Member = MemberRepo.GetById(Id);
            if (Member == null) return false;
            _mapper.Map(UpdatedMember, Member);
            return _uintOfWork.SaveChanges() > 0;
        }
            #endregion

            #region Remove Member
            public bool RemoveMember(int MemberId)
            {
            var memberRepo = _uintOfWork.GetRepository<Member>();
            var Member = memberRepo.GetById(MemberId);
            if (Member is null) return false;

            var HasActiveBookings = _uintOfWork.GetRepository<MemberSession>()
                .GetAll(x => x.MemberId == MemberId && x.Session.StartDate > DateTime.UtcNow).Any();
            var memberShipsRepo = _uintOfWork.GetRepository<Membership>();
            var MemberShips = memberShipsRepo.GetAll(x => x.MemberId == MemberId);

            try
                {
                if(MemberShips.Any())
                {
                    foreach(var membership in MemberShips)
                        memberShipsRepo.Delete(membership);
                    
                }
                memberRepo.Delete(Member);
                return _uintOfWork.SaveChanges() > 0;
                }
                catch
                {
         
                    return false;
            }
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

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = _uintOfWork.GetRepository<Member>().GetAll() ?? [];
            if (Members is null || !Members.Any()) return [];

            var MemberViewModels = _mapper.Map<IEnumerable<MemberViewModel>>(Members);
            return MemberViewModels;
        }

        #endregion

    }
    }

