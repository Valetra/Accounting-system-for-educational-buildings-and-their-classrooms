using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using DAL.Models;
using Service;
using Exceptions;

namespace Controllers;

[ApiController]
public class BuildingController(IBuildingService buildingService, IMapper mapper) : ControllerBase
{
	/// <summary>
	/// Вызов всех зданий из базы данных.
	/// </summary>
	[HttpGet("get-all")]
	public async Task<ActionResult<List<ResponseObjects.Building>>> GetBuildings()
	{
		List<Building> buildings = await buildingService.GetAll();
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
		Building? building = await buildingService.Get(id);

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
		Building createdBuilding = await buildingService.Create(buildingToCreate);

		ResponseObjects.Building response = mapper.Map<ResponseObjects.Building>(createdBuilding);
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

		try
		{
			Building? updatedBuilding = await buildingService.Update(buildingToUpdate);
			ResponseObjects.Building response = mapper.Map<ResponseObjects.Building>(updatedBuilding);

			return Ok(response);
		}
		catch (NotExistedBuildingException e)
		{
			return NotFound($"Building with id = `{e.Id}` does not exists.");
		}
	}

	/// <summary>
	/// Удаление здания из базы данных.
	/// </summary>
	/// <param name="id">Идентификатор здания</param>
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteBuilding(Guid id) =>
		await buildingService.Delete(id) ? NoContent() : NotFound();
}
