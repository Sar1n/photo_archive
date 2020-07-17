using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DataTransferObjects;

namespace BLL.Services
{
	public interface IPostService
	{
		IEnumerable<PostBLL> GetAll (); //get
		PostBLL GetOne (int id); //get
		IEnumerable<PostBLL> GetSome(string searchstring); //get
		void Create (PostBLL Post); //post
		void Update (PostBLL Post); //put
		void Delete (int id); //delete
		void Dispose();
	}
}
