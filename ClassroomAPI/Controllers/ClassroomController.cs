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
		List<ResponseObjects.Classroom> response = classrooms.Select(mapper.Map<ResponseObjects.Classroom>).ToList();

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

		return Ok(response);
	}

	/// <summary>
	/// Добавление аудитории в базу данных.
	/// </summary>
	/// <remarks>
	/// Комментарии:
	/// 1) Использование идентификатора здания("buildingId") разрешено только из уже существующих/созданных(не удалённых) зданий.
	/// 2) Допустимые значения поля "type" = ["Lecture", "ForPractice", "Gym", "Other"]
	/// </remarks>
	/// <param name="classroom">Аудитория</param>
	[HttpPost("create")]
	public async Task<ActionResult> CreateClassroom(RequestObjects.Classroom classroom)
	{
		Classroom classroomToCreate = mapper.Map<Classroom>(classroom);

		Classroom createdClassroom;

		try
		{
			createdClassroom = await _classroomService.Create(classroomToCreate);
		}
		catch (NotExistedBuildingException e)
		{
			return BadRequest($"There is no building with Id = {e.BuildingId}");
		}

		ResponseObjects.Classroom response = mapper.Map<ResponseObjects.Classroom>(createdClassroom);

		return Ok(response);

	}

	/// <summary>
	/// Изменение аудитории в базе данных.
	/// </summary>
	/// <remarks>
	/// Комментарии:
	/// 1) Использование идентификатора здания("buildingId") разрешено только из уже существующих/созданных(не удалённых) зданий.
	/// 2) Допустимые значения поля "type" = ["Lecture", "ForPractice", "Gym", "Other"]
	/// </remarks>
	/// <param name="id">Идентификатор аудитории</param>
	/// <param name="classroom">Аудитория</param>
	[HttpPut("{id}")]
	public async Task<ActionResult<ResponseObjects.Classroom>> UpdateClassroom(Guid id, RequestObjects.Classroom classroom)
	{
		Classroom classroomToUpdate = mapper.Map<Classroom>(classroom);
		classroomToUpdate.Id = id;

		Classroom? updatedClassroom;

		try
		{
			updatedClassroom = await _classroomService.Update(classroomToUpdate);
		}
		catch (NotExistedClassroomException e)
		{
			return NotFound($"Classroom with id = `{e.ClassroomId}` does not exists.");
		}
		catch (NotExistedBuildingException e)
		{
			return BadRequest($"Building with Id = `{e.BuildingId}`  does not exists");
		}

		ResponseObjects.Classroom response = mapper.Map<ResponseObjects.Classroom>(updatedClassroom);

		return Ok(response);
	}

	/// <summary>
	/// Удаление аудитории из базы данных.
	/// </summary>
	/// <param name="id">Идентификатор аудитории</param>
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteClassroom(Guid id) =>
		!await _classroomService.Delete(id) ? NotFound() : NoContent();
}
