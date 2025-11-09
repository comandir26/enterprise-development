using Bikes.Application.Dto;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Application.Services;

public class BikeModelService : IBikeModelService
{
    private readonly IRepository<BikeModel, int> _bikeModelRepository;

    public BikeModelService(IRepository<BikeModel, int> bikeModelRepository)
    {
        _bikeModelRepository = bikeModelRepository;
    }

    private static BikeModel MapToDomain(BikeModelDto dto, int id = 0)
    {
        return new BikeModel
        {
            Id = id,
            Type = dto.Type,
            WheelSize = dto.WheelSize,
            MaxPassengerWeight = dto.MaxPassengerWeight,
            Weight = dto.Weight,
            BrakeType = dto.BrakeType,
            Year = dto.Year,
            RentPrice = dto.RentPrice
        };
    }

    public int CreateBikeModel(BikeModelDto bikeModelDto)
    {
        var bikeModel = MapToDomain(bikeModelDto);
        return _bikeModelRepository.Create(bikeModel);
    }

    public List<BikeModel> GetAllBikeModels() => _bikeModelRepository.ReadAll();

    public BikeModel? GetBikeModelById(int id) => _bikeModelRepository.Read(id);

    public BikeModel? UpdateBikeModel(int id, BikeModelDto bikeModelDto)
    {
        var existingModel = _bikeModelRepository.Read(id);
        if (existingModel == null) return null;

        var updatedModel = MapToDomain(bikeModelDto, id);
        return _bikeModelRepository.Update(id, updatedModel);
    }

    public bool DeleteBikeModel(int id) => _bikeModelRepository.Delete(id);
}