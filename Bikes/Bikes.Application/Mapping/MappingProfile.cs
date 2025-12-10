using AutoMapper;
using Bikes.Contracts.Dto;
using Bikes.Domain.Models;

namespace Bikes.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Bike, BikeGetDto>();
        CreateMap<BikeCreateUpdateDto, Bike>();

        CreateMap<BikeModel, BikeModelGetDto>();
        CreateMap<BikeModelCreateUpdateDto, BikeModel>();

        CreateMap<Renter, RenterGetDto>();
        CreateMap<RenterCreateUpdateDto, Renter>();

        CreateMap<Rent, RentGetDto>();
        CreateMap<RentCreateUpdateDto, Rent>();
    }
}