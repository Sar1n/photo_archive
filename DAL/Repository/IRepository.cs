using System;
using System.Collections.Generic;

namespace DAL.Repository
{
	public interface IRepository<T> where T : class
	{
		IEnumerable<T> GetList(); //get
		T GetItem (int id); //get
		void Create (T item); //post
		void Update (T item); //put
		void Delete (int id); //delete
	}
}
