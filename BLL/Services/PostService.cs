using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DataTransferObjects;
using DAL.Models;
using DAL.UnitOfWork;
using Ninject;
using Ninject.Parameters;
using AutoMapper;

namespace BLL.Services
{
	public class PostService : IPostService
	{
		IUnitOfWork db { get; set; }
		MapperConfiguration DALtoBLL = new MapperConfiguration(cfg => cfg.CreateMap<Post, PostBLL>());
		MapperConfiguration BLLtoDAL = new MapperConfiguration(cfg => cfg.CreateMap<PostBLL, Post>());
		public PostService(string connectionString)//, IKernel Kernal
		{
			IKernel Kernal = new StandardKernel();
			Kernal.Bind<IUnitOfWork>().To<UnitOfWork>();
			db = Kernal.Get<IUnitOfWork>( new ConstructorArgument("connectionString", connectionString));
		}
		public IEnumerable<PostBLL> GetAll() //get
		{
			var mapper = new Mapper(DALtoBLL);
			return mapper.Map<IEnumerable<Post>, List<PostBLL>>(db.Post.GetList());
		}
		public PostBLL GetOne(int id) //get
		{
			var post = db.Post.GetItem(id);
			if (post == null)
				throw new Exception("Post not found");
			var mapper = new Mapper(DALtoBLL);
			return mapper.Map<Post, PostBLL>(post);
		}
		public void Create(PostBLL post) //post
		{
			if (post == null)
				throw new Exception("Post to post is null");
			var mapper = new Mapper(BLLtoDAL);
			Post postDAL = mapper.Map<PostBLL, Post>(post);
			db.Post.Create(postDAL);
			db.Save();
		}
		public void Update(PostBLL post) //put
		{
			if (post == null)
				throw new Exception("Post to put is null");
			var mapper = new Mapper(BLLtoDAL);
			Post postDAL = mapper.Map<PostBLL, Post>(post);
			db.Post.Update(postDAL);
		}
		public void Delete(int id) //delete
		{
			db.Post.Delete(id);
		}
		public void Dispose()
		{
			db.Dispose();
		}
	}
}
