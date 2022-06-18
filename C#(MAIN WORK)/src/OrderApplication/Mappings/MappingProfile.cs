using System;
using System.Linq;
using AutoMapper;
using OrderApplication.Models.DTO;
using OrderDomain.Entities;

namespace OrderApplication.Mappings
{
    public class MappingProfile:Profile
    {

        public MappingProfile()
        {
            CreateMap<OrderForCreation, Order>().
                ForMember(dest=>dest.CreatedAt,operation=>operation
                    .MapFrom(source=>DateTime.Now.ToUniversalTime())).
                ForMember(dest=>dest.UpdatedAt,operation=>operation
                    .MapFrom(source=>DateTime.Now.ToUniversalTime()))
                .ForMember(dest=>dest.Quantity,operation=>operation.
                    MapFrom(source=>source.ProductIds.Count));
            CreateMap<OrderForUpdate, Order>().
                ForMember(dest => dest.UpdatedAt, operation => operation
                .MapFrom(source => DateTime.Now.ToUniversalTime()))
                .ForMember(dest=>dest.Quantity,operation=>operation.
                    MapFrom(source=>source.ProductIds.Count));
            CreateMap<ProductForCreation, Product>();
            CreateMap<ProductForUpdate, Product>();
            
        }
        
    }
}