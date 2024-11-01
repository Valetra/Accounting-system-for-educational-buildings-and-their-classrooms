using AutoMapper;

using DAL.Models;
using DAL.Repository;
using RabbitMq;
using Exceptions;

namespace Service;

public class BuildingService(IBuildingRepository buildingRepository, IRabbitMqProducer rabbitMqProducer, IMapper mapper) : IBuildingService
{
	public Task<List<Building>> GetAll() => Task.FromResult(buildingRepository.GetAll().ToList());

	public Task<Building?> Get(Guid id) => buildingRepository.Get(id);

	public async Task<Building> Create(Building model)
	{
		Building createdBuilding = await buildingRepository.Create(model);

		MessageContracts.Building buildingMessage = mapper.Map<MessageContracts.Building>(createdBuilding);
		await rabbitMqProducer.SendMessage(buildingMessage, "create");

		return createdBuilding;
	}


	public async Task<Building> Update(Building model)
	{
		Building? updatedBuilding = await buildingRepository.Update(model) ?? throw new NotExistedBuildingException(model.Id);

		MessageContracts.Building buildingMessage = mapper.Map<MessageContracts.Building>(updatedBuilding);
		await rabbitMqProducer.SendMessage(buildingMessage, "update");

		return updatedBuilding;
	}

	public async Task<bool> Delete(Guid id)
	{
		bool isDeleted = await buildingRepository.Delete(id);

		if (isDeleted)
		{
			MessageContracts.Building buildingMessage = mapper.Map<MessageContracts.Building>(new MessageContracts.Building { Id = id });
			await rabbitMqProducer.SendMessage(buildingMessage, "delete");
		}

		return isDeleted;
	}
}
