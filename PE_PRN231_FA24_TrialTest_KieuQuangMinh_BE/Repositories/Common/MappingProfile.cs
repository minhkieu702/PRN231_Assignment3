using AutoMapper;
using Repositories.Models;
using Repositories.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonResponseModel>()
                .ForMember(dest => dest.Viruses, opt => opt.MapFrom(src => src.PersonViruses));

            CreateMap<PersonVirus, VirusResponseModel>()
                .ForMember(dest => dest.VirusName, opt => opt.MapFrom(src => src.Virus.VirusName))
                .ForMember(dest => dest.ResistanceRate, opt => opt.MapFrom(src => src.ResistanceRate));
        }
    }
}
