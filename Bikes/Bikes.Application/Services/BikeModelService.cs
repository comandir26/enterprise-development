using AutoMapper;
using Bikes.Contracts.Dto;
using Bikes.Domain.Models;
using Bikes.Domain.Repositories;

namespace Bikes.Application.Services;

/// <summary>
/// A class that implements the interface of the BikeModelService class
/// </summary>
public class BikeModelService(
    IRepository<BikeModel, int> bikeModelRepository,
    IMapper mapper) : IBikeModelService
{
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="bikeModelDto">DTO object</param>
    /// <returns>ID of the created object</returns>
    public int CreateBikeModel(BikeModelDto bikeModelDto)
    {
        var bikeModel = mapper.Map<BikeModel>(bikeModelDto);

        return bikeModelRepository.Create(bikeModel);
    }

    /// <summary>
    /// Returns all existing objects
    /// </summary>
    /// <returns>List of existing objects</returns>
    public List<BikeModelDto> GetAllBikeModels()
    {
        var models = bikeModelRepository.ReadAll();

        return models.Select(model =>
        {
            var dto = mapper.Map<BikeModelDto>(model);
            return dto;
        }).ToList();
    }

    /// <summary>
    /// Returns object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BikeModelDto? GetBikeModelById(int id)
    {
        var model = bikeModelRepository.Read(id);
        if (model == null) return null;

        return mapper.Map<BikeModelDto>(model);
    }

    /// <summary>
    /// Updates an existing object
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="bikeModelDto">DTO object</param>
    /// <returns>Object if exist</returns>
    public BikeModelDto? UpdateBikeModel(int id, BikeModelDto bikeModelDto)
    {
        var existingModel = bikeModelRepository.Read(id);
        if (existingModel == null) return null;

        mapper.Map(bikeModelDto, existingModel);

        existingModel.Id = id;

        var updatedModel = bikeModelRepository.Update(id, existingModel);
        if (updatedModel == null) return null;

        return mapper.Map<BikeModelDto>(updatedModel);
    }

    /// <summary>
    /// Deletes an existing object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or false? result of deleting</returns>
    public bool DeleteBikeModel(int id) => bikeModelRepository.Delete(id);
}