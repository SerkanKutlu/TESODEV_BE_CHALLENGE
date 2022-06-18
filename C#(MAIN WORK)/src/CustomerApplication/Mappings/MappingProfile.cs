using System;
using AutoMapper;
using CustomerApplication.Models.DTO;
using CustomerDomain.Entities;

namespace CustomerApplication.Mappings
{
    public class MappingProfile : Profile
    {
        
        //Creating AutoMapper Mapping Profiles

        public MappingProfile()
        {
            //Date information is set here.
            CreateMap<CustomerForCreation,Customer>().
                ForMember(dest=>dest.CreatedAt,operation=>operation
                .MapFrom(source=>DateTime.Now.ToUniversalTime())).
                ForMember(dest=>dest.UpdatedAt,operation=>operation
                    .MapFrom(dest=>DateTime.Now.ToUniversalTime()));
            CreateMap<CustomerForUpdate, Customer>().ForMember(dest => dest.UpdatedAt, operation => operation
                .MapFrom(source => DateTime.Now.ToUniversalTime()));
        }

    }
}