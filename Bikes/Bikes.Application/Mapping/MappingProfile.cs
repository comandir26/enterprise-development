using AutoMapper;
using Bikes.Contracts.Dto;
using Bikes.Domain.Models;

namespace Bikes.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Bike, BikeGetDto>()
            .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.ModelId));

        CreateMap<BikeCreateUpdateDto, Bike>()
            .ForMember(dest => dest.Model, opt => opt.Ignore());

        CreateMap<BikeModel, BikeModelGetDto>();
        CreateMap<BikeModelCreateUpdateDto, BikeModel>();

        CreateMap<Renter, RenterGetDto>();
        CreateMap<RenterCreateUpdateDto, Renter>();

        CreateMap<Rent, RentGetDto>()
            .ForMember(dest => dest.RenterId, opt => opt.MapFrom(src => src.RenterId))
            .ForMember(dest => dest.BikeId, opt => opt.MapFrom(src => src.BikeId));

        CreateMap<RentCreateUpdateDto, Rent>()
            .ForMember(dest => dest.Renter, opt => opt.Ignore())
            .ForMember(dest => dest.Bike, opt => opt.Ignore());
    }
}