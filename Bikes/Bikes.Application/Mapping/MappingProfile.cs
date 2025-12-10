using AutoMapper;
using Bikes.Contracts.Dto;
using Bikes.Domain.Models;

namespace Bikes.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Renter, RenterDto>().ReverseMap();
        CreateMap<Bike, BikeDto>().ReverseMap();
        CreateMap<BikeModel, BikeModelDto>().ReverseMap();
        CreateMap<Rent, RentDto>().ReverseMap();
    }
}