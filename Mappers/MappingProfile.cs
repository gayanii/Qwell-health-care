using AutoMapper;
using QWellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWellApp.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDetails>()
            .ForMember(dest => dest.Password, opt => opt.Ignore()); // Ignore sensitive data
        }
    }
}
