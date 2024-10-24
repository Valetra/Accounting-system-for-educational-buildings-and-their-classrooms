using Microsoft.EntityFrameworkCore;

using DAL.Models;

namespace DAL.Contexts;

public class BuildingContext(DbContextOptions<BuildingContext> options) : DbContext(options)
{
	public DbSet<Building> Buildings { get; set; }
}
