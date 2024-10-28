using Microsoft.EntityFrameworkCore;

using DAL.Models;

namespace DAL.Contexts;

public class ClassroomContext(DbContextOptions<ClassroomContext> options) : DbContext(options)
{
	public DbSet<Classroom> Classrooms { get; set; }
	public DbSet<ShortBuildingInfo> ShortBuildingInfo { get; set; }
}
