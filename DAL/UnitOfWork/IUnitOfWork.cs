using System;
using DAL.Repository;

namespace DAL.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		PostRepository Post { get; }
		void Save();
	}
}
