using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using DAL.Models;
using Service;
using Exceptions;

namespace Controllers;

[ApiController]
public class ClassroomController(IClassroomService classroomService, IMapper mapper) : ControllerBase
{
	private readonly IClassroomService _classroomService = classroomService;

	/// <summary>
	/// Вызов всех аудитории из базы данных.
	/// </summary>
	[HttpGet("get-all")]
	public async Task<ActionResult<List<ResponseObjects.Classroom>>> GetClassrooms()
	{
		List<Classroom> classrooms = await _classroomService.GetAll();
		List<ResponseObjects.Classroom> response = [];

		foreach (Classroom classroom in classrooms)
		{
			try
			{
				ShortBuildingInfo? shortBuildingInfo = await _classroomService.GetShortBuildingInfo(classroom.BuildingId) ?? throw new NotExistedBuildingException();
				ResponseObjects.Classroom responseClassroom = mapper.Map<ResponseObjects.Classroom>(classroom);
				responseClassroom.ShortBuildingInfo = shortBuildingInfo;

				response.Add(responseClassroom);

			}
			catch (NotExistedBuildingException)
			{
				return BadRequest($"There is no building with Id = {classroom.BuildingId}");
			}
		}
		return Ok(response);
	}

	/// <summary>
	/// Вызов аудитории из базы данных.
	/// </summary>
	/// <param name="id">Идентификатор аудитории</param>
	[HttpGet("get")]
	public async Task<ActionResult<ResponseObjects.Classroom>> GetClassroom([FromQuery] Guid id)
	{
		Classroom? classroom = await _classroomService.Get(id);

		if (classroom is null)
		{
			return NotFound($"Classroom with id = `{id}` not found.");
		}

		ResponseObjects.Classroom response = mapper.Map<ResponseObjects.Classroom>(classroom);

		try
		{
			ShortBuildingInfo? shortBuildingInfo = await _classroomService.GetShortBuildingInfo(classroom.BuildingId) ?? throw new NotExistedBuildingException();
			response.ShortBuildingInfo = shortBuildingInfo;
		}
		catch (NotExistedBuildingException)
		{
			return BadRequest($"There is no building with Id = {classroom.BuildingId}");
		}

		return Ok(response);
	}

	/// <summary>
	/// Добавление аудитории в базу данных.
	/// </summary>
	/// Комментарии:
	/// 1) Использование идентификатора здания("buildingId") разрешено только из уже существующих/созданных(не удалённых) зданий.
	/// 2) Допустимые значения поля "type" = ["Lecture", "ForPractice", "Gym", "Other"]
	/// <param name="classroom">Аудитория</param>
	[HttpPost("create")]
	public async Task<ActionResult> CreateClassroom(RequestObjects.Classroom classroom)
	{
		Classroom classroomToCreate = mapper.Map<Classroom>(classroom);

		try
		{
			Classroom createdClassroom = await _classroomService.Create(classroomToCreate);
			ResponseObjects.Classroom response = mapper.Map<ResponseObjects.Classroom>(createdClassroom);

			ShortBuildingInfo? shortBuildingInfo = await _classroomService.GetShortBuildingInfo(createdClassroom.BuildingId);
			response.ShortBuildingInfo = shortBuildingInfo is not null ? shortBuildingInfo : throw new NotExistedBuildingException();

			return Ok(response);
		}
		catch (NotExistedBuildingException)
		{
			return BadRequest($"There is no building with Id = {classroom.BuildingId}");
		}
	}

	/// <summary>
	/// Изменение аудитории в базе данных.
	/// </summary>
	/// Комментарии:
	/// 1) Использование идентификатора здания("buildingId") разрешено только из уже существующих/созданных(не удалённых) зданий.
	/// 2) Допустимые значения поля "type" = ["Lecture", "ForPractice", "Gym", "Other"]
	/// <param name="id">Идентификатор аудитории</param>
	/// <param name="classroom">Аудитория</param>
	[HttpPut("{id}")]
	public async Task<ActionResult<ResponseObjects.Classroom>> UpdateBuilding(Guid id, RequestObjects.Classroom classroom)
	{
		Classroom classroomToUpdate = mapper.Map<Classroom>(classroom);
		classroomToUpdate.Id = id;

		Classroom? updatedClassroom = await _classroomService.Update(classroomToUpdate);

		if (updatedClassroom is null)
		{
			return NotFound($"Classroom with id = `{id}` does not exists.");
		}

		ResponseObjects.Classroom response = mapper.Map<ResponseObjects.Classroom>(updatedClassroom);

		return Ok(response);
	}

	/// <summary>
	/// Изменение флага 'IsDeleted' в значение 'true' у существующей аудитории в базе данных.
	/// </summary>
	/// <param name="id">Идентификатор аудитории</param>
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteClassroom(Guid id) =>
		!await _classroomService.Delete(id) ? NotFound() : NoContent();
}
