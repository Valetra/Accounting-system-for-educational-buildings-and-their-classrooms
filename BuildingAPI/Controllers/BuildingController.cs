using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using DAL.Models;
using Service;
using RabbitMq;

namespace Controllers;

[ApiController]
public class BuildingController(IBuildingService buildingService, IRabbitMqProducer rabbitMqService, IMapper mapper) : ControllerBase
{
	private readonly IBuildingService _buildingService = buildingService;
	private readonly IRabbitMqProducer _rabbitMqService = rabbitMqService;

	[HttpGet("get-all")]
	public async Task<ActionResult<List<ResponseObjects.Building>>> GetBuildings()
	{
		List<Building> buildings = await _buildingService.GetAll();
		List<ResponseObjects.Building> response = buildings.Select(mapper.Map<ResponseObjects.Building>).ToList();

		return Ok(response);
	}

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

	[HttpPost("create")]
	public async Task<ActionResult> CreateBuilding(RequestObjects.Building building)
	{
		Building buildingToCreate = mapper.Map<Building>(building);
		Building createdBuilding = await _buildingService.Create(buildingToCreate);

		ResponseObjects.Building response = mapper.Map<ResponseObjects.Building>(createdBuilding);
		await _rabbitMqService.SendMessage(response, "create");

		return Ok(response);
	}

	[HttpPut("{buildingId}")]
	public async Task<ActionResult<ResponseObjects.Building>> UpdateBuilding(Guid buildingId, RequestObjects.Building building)
	{
		Building buildingToUpdate = mapper.Map<Building>(building);
		buildingToUpdate.Id = buildingId;

		Building? updatedBuilding = await _buildingService.Update(buildingToUpdate);

		if (updatedBuilding is null)
		{
			return NotFound($"Building with id = `{buildingId}` does not exists.");
		}

		ResponseObjects.Building response = mapper.Map<ResponseObjects.Building>(updatedBuilding);
		await _rabbitMqService.SendMessage(response, "update");

		return Ok(response);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteBuilding(Guid id)
	{
		if (!await _buildingService.Delete(id))
		{
			return NotFound();
		}

		await _rabbitMqService.SendMessage(id, "delete");

		return NoContent();
	}
}
