using Bikes.Application.Dto;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Application.Services;

public class BikeService : IBikeService
{
    private readonly IRepository<Bike, int> _bikeRepository;
    private readonly IRepository<BikeModel, int> _bikeModelRepository;

    public BikeService(
        IRepository<Bike, int> bikeRepository,
        IRepository<BikeModel, int> bikeModelRepository)
    {
        _bikeRepository = bikeRepository;
        _bikeModelRepository = bikeModelRepository;
    }

    private static Bike MapToDomain(BikeDto dto, BikeModel model, int id = 0)
    {
        return new Bike
        {
            Id = id,
            SerialNumber = dto.SerialNumber,
            Color = dto.Color,
            Model = model
        };
    }

    public int CreateBike(BikeDto bikeDto)
    {
        var model = _bikeModelRepository.Read(bikeDto.ModelId);
        if (model == null)
            throw new ArgumentException($"BikeModel with id {bikeDto.ModelId} not found");

        var bike = MapToDomain(bikeDto, model);
        return _bikeRepository.Create(bike);
    }

    public List<Bike> GetAllBikes() => _bikeRepository.ReadAll();

    public Bike? GetBikeById(int id) => _bikeRepository.Read(id);

    public Bike? UpdateBike(int id, BikeDto bikeDto)
    {
        var existingBike = _bikeRepository.Read(id);
        if (existingBike == null) return null;

        var model = _bikeModelRepository.Read(bikeDto.ModelId);
        if (model == null) return null;

        var updatedBike = MapToDomain(bikeDto, model, id);
        return _bikeRepository.Update(id, updatedBike);
    }

    public bool DeleteBike(int id) => _bikeRepository.Delete(id);
}