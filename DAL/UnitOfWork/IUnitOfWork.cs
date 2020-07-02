using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repository;

namespace DAL.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		PostRepository Post { get; }
		void Save();
	}
}
