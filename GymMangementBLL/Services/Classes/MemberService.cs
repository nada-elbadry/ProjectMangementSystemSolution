using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.MemberViewModels;
using GymMangementDAL.Entities;
using GymMangementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Classes
{
    internal class MemberService : IMemberService
    {
        private readonly IGenaricRepository<Member> _memberRepository;
        private readonly IGenaricRepository<Membership> _membershipRepo;
        private readonly IPlanRepository _planRepository;
        private readonly IGenaricRepository<HealthRecord> _healthRecordRepository;

        //Ask CLR For Creating Object From Service
        // CLR Will Inject Adress Of Object in Constructor

        public MemberService(IGenaricRepository<Member> memberRepository,
            IGenaricRepository<Membership> membershipRepo ,
            IPlanRepository planRepository ,
            IGenaricRepository<HealthRecord> healthRecordRepository)
        {
            _memberRepository = memberRepository;
            _membershipRepo = membershipRepo;
            _planRepository = planRepository;
            _healthRecordRepository = healthRecordRepository;
        }

        public bool CreateMember(CreatMemberViewModel createMember)
        {
          
            try
            {
                ////Check If Email Is Exists
                //var emailExists = _memberRepository.GetAll(m => m.Email == createMember.Email).Any();

                ////Check If Phone Is Exists
                //var phoneExists = _memberRepository.GetAll(X => X.Phone == createMember.Phone).Any();
                //if (emailExists || phoneExists) return false;
                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone)) return false;
                //If Not Add Member And Return True If Added Successfully
                var member = new Member()
                {
                    Email = createMember.Email,
                    Name = createMember.Name,
                    Phone = createMember.Phone,
                    Gender = createMember.Gender,
                    DateOfBirth = createMember.DateOfBirth,
                    Address = new Address()
                    {
                        BuildingNumber = createMember.BuldingNumber,
                        City = createMember.City,
                        Street = createMember.Street
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createMember.HealthRecordViewModel.Height,
                        Weight = createMember.HealthRecordViewModel.Weight,
                        BloodType = createMember.HealthRecordViewModel.BloodType,
                        Note = createMember.HealthRecordViewModel.Note,
                    },

                };
                return _memberRepository.Add(member) > 0;
            }
            catch (Exception)
            {
                return false;
            }
            //If One Of Them Exists Return False


        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            //var Members = _memberRepository.GetAll() ?? [];
            var Members = _memberRepository.GetAll();
            if (Members is null || !Members.Any())return Enumerable.Empty<MemberViewModel>();
            var MemberViewModels = Members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString()
            });
            

            return MemberViewModels;
        }

        public MemberViewModel? GetMemberById(int MemberId)
        {
            var member = _memberRepository.GetById(MemberId);
            if (member is null) return null;
            var ViewModel= new MemberViewModel()
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber}-{member.Address.Street}-{member.Address.City}",
              
            };
            //Active Membership
            var ActivememberShip = _membershipRepo.GetAll(m => m.MemberId == MemberId && m.Status=="Active").FirstOrDefault();
            if(ActivememberShip is not null)
            {
                ViewModel.MembershipStartDate = ActivememberShip.CreatedAt.ToShortDateString();
                ViewModel.MembershipEndDate = ActivememberShip.EndDate.ToShortDateString();
                
                var plan = _planRepository.GetById(ActivememberShip.PlanId);
            }
            return ViewModel;

        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var Member = _memberRepository.GetById(MemberId);
            if (Member is null) return null;
            return new MemberToUpdateViewModel()
            {
                Name = Member.Name,
                Photo = Member.Photo,
                Email = Member.Email,
                Phone = Member.Phone,
                BuldingNumber = Member.Address.BuildingNumber,
                Street = Member.Address.Street,
                City = Member.Address.City
            };
        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            
                var MemberHealthRecord = _healthRecordRepository.GetById(MemberId);
                if (MemberHealthRecord is null) return null;
                return new HealthRecordViewModel()
                {
                    Height = MemberHealthRecord.Height,
                    Weight = MemberHealthRecord.Weight,
                    BloodType = MemberHealthRecord.BloodType,
                    Note = MemberHealthRecord.Note
                };
        }

        public bool UpldateMemberDetails(int MemberId, MemberToUpdateViewModel UpdatedMember)
        {
            try 
            {
            var EmailExists = _memberRepository.GetAll(m => m.Email == UpdatedMember.Email).Any();
                var PhoneExists = _memberRepository.GetAll(m => m.Phone == UpdatedMember.Phone).Any();
                if(IsEmailExists(UpdatedMember.Email) || IsPhoneExists(UpdatedMember.Phone)) return false;
                var Member = _memberRepository.GetById(MemberId);
                if (Member is null) return false;
                Member.Email = UpdatedMember.Email;
                Member.Phone = UpdatedMember.Phone;
                Member.Address.BuildingNumber = UpdatedMember.BuldingNumber;
                Member.Address.Street = UpdatedMember.Street;
                Member.Address.City = UpdatedMember.City;
                Member.UpdatedAt = DateTime.Now;
               return _memberRepository.Update(Member)>0;

            }
            catch
            {
                return false;
            }
        }

        #region Helper Methods
        private bool IsEmailExists(string email)
        {
            return _memberRepository.GetAll(m => m.Email == email).Any();
            
        }
        private bool IsPhoneExists(string phone)
        {
            return _memberRepository.GetAll(m => m.Phone == phone).Any();
        }
        #endregion
    }
}

