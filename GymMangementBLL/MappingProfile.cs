using AutoMapper;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using GymMangementBLL.ViewModels.SessionViewModels;
using GymMangementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Session,SessionViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest=>dest.TrainerName,opt=>opt.MapFrom(src=>src.TrainerName))
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
            //CreateMap<UpdateSessionViewModel,Session>();
        }
    }
}
