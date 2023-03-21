using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Session1112API.Models;

namespace Session1112API.data
{
	public class HRContext:IdentityDbContext<ApplicationUsers>
	{
		IConfiguration config;

		public HRContext(IConfiguration _config)
		{
			config = _config;
		}
		public DbSet<Employee> employees { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(config.GetConnectionString("hrConn"));
			base.OnConfiguring(optionsBuilder);
		}
	}
}
