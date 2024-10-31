using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using DAL.Models;
using Service;
using RabbitMq;

namespace Controllers;

[ApiController]
public class BuildingController(IBuildingService buildingService, IRabbitMqProducer rabbitMqProducer, IMapper mapper) : ControllerBase
{
	private readonly IBuildingService _buildingService = buildingService;
	private readonly IRabbitMqProducer _rabbitMqProducer = rabbitMqProducer;

	/// <summary>
	/// Вызов всех зданий из базы данных.
	/// </summary>
	[HttpGet("get-all")]
	public async Task<ActionResult<List<ResponseObjects.Building>>> GetBuildings()
	{
		List<Building> buildings = await _buildingService.GetAll();
		List<ResponseObjects.Building> response = buildings.Select(mapper.Map<ResponseObjects.Building>).ToList();

		return Ok(response);
	}

	/// <summary>
	/// Вызов здания из базы данных.
	/// </summary>
	/// <param name="id">Идентификатор здания</param>
	[HttpGet("get")]
	public async Task<ActionResult<ResponseObjects.Building>> GetBuilding([FromQuery] Guid id)
	{
		Building? building = await _buildingService.Get(id);

		if (building is null)
		{
			return NotFound($"Building with id = `{id}` not found.");
		}

		ResponseObjects.Building response = mapper.Map<ResponseObjects.Building>(building);

		return Ok(response);
	}

	/// <summary>
	/// Добавление здания в базу данных.
	/// </summary>
	/// <param name="building">Здание</param>
	[HttpPost("create")]
	public async Task<ActionResult> CreateBuilding(RequestObjects.Building building)
	{
		Building buildingToCreate = mapper.Map<Building>(building);
		Building createdBuilding = await _buildingService.Create(buildingToCreate);

		ResponseObjects.Building response = mapper.Map<ResponseObjects.Building>(createdBuilding);
		await _rabbitMqProducer.SendMessage(response, "create");

		return Ok(response);
	}

	/// <summary>
	/// Изменение существующего здания в базе данных.
	/// </summary>
	/// <param name="id">Идентификатор здания</param>
	/// <param name="building">Здание</param>
	[HttpPut("{id}")]
	public async Task<ActionResult<ResponseObjects.Building>> UpdateBuilding(Guid id, RequestObjects.Building building)
	{
		Building buildingToUpdate = mapper.Map<Building>(building);
		buildingToUpdate.Id = id;

		Building? updatedBuilding = await _buildingService.Update(buildingToUpdate);

		if (updatedBuilding is null)
		{
			return NotFound($"Building with id = `{id}` does not exists.");
		}

		ResponseObjects.Building response = mapper.Map<ResponseObjects.Building>(updatedBuilding);
		await _rabbitMqProducer.SendMessage(response, "update");

		return Ok(response);
	}

	/// <summary>
	/// Удаление здания из базы данных.
	/// </summary>
	/// <param name="id">Идентификатор здания</param>
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteBuilding(Guid id)
	{
		if (!await _buildingService.Delete(id))
		{
			return NotFound();
		}

		ResponseObjects.Building response =
			mapper.Map<ResponseObjects.Building>(new Building { Id = id, Name = "", Address = "", FloorsCount = 0 });
		await _rabbitMqProducer.SendMessage(response, "delete");

		return NoContent();
	}
}
