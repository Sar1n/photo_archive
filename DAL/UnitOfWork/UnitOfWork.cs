using System;
using DAL.Context;
using DAL.Repository;

namespace DAL.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private PostContext db;
		private PostRepository postRepository;
		public UnitOfWork(string connectionString)
		{
			db = new PostContext(connectionString);
		}
		public PostRepository Post
		{
			get
			{
				if (postRepository == null)
					postRepository = new PostRepository(db);
				return postRepository;
			}
		}
		public void Save()
		{
			db.SaveChanges();
		}
		private bool disposed = false;
		public virtual void Dispose(bool disposing)
		{
			if (!disposed)
				if (disposing)
					db.Dispose();
			disposed = true;
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
