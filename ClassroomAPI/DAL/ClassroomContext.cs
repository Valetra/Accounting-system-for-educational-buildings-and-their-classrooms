using Microsoft.EntityFrameworkCore;

using DAL.Models;

namespace DAL.Contexts;

public class ClassroomContext(DbContextOptions<ClassroomContext> options) : DbContext(options)
{
	public DbSet<Classroom> Classrooms { get; set; }
	public DbSet<ShortBuildingInfo> ShortBuildingInfo { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) =>
		modelBuilder.Entity<Classroom>()
			.HasOne(x => x.ShortBuildingInfo).WithMany(x => x.Classrooms)
			.HasForeignKey(x => x.BuildingId)
			.OnDelete(DeleteBehavior.Cascade);
}
