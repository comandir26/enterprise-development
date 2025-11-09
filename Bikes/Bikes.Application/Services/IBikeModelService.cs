using Bikes.Application.Dto;
using Bikes.Domain.Models;

namespace Bikes.Application.Services;

public interface IBikeModelService
{
    public int CreateBikeModel(BikeModelDto bikeModelDto);
    public List<BikeModel> GetAllBikeModels();
    public BikeModel? GetBikeModelById(int id);
    public BikeModel? UpdateBikeModel(int id, BikeModelDto bikeModelDto);
    public bool DeleteBikeModel(int id);
}