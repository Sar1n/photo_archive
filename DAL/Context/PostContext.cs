using System.Data.Entity;
using DAL.Models;

namespace DAL.Context
{
	public class PostContext : DbContext
	{
		public PostContext(string connectionString) : base(connectionString)
		{
		}
		public DbSet<Post> Posts { get; set; }
	}
}
