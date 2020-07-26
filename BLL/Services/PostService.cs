using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DataTransferObjects;
using DAL.Models;
using DAL.UnitOfWork;
using Ninject;
using Ninject.Parameters;
using AutoMapper;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;


namespace BLL.Services
{
	public class PostService : IPostService
	{
		IUnitOfWork db { get; set; }
		MapperConfiguration DALtoBLL = new MapperConfiguration(cfg => cfg.CreateMap<Post, PostBLL>());
		MapperConfiguration BLLtoDAL = new MapperConfiguration(cfg => cfg.CreateMap<PostBLL, Post>());
		public PostService(string connectionString)
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
		public IEnumerable<PostBLL> GetSome(string searchstring) //get
		{
			var mapper = new Mapper(DALtoBLL);
			return mapper.Map<IEnumerable<Post>, List<PostBLL>>(db.Post.GetSome(searchstring));
		}
		public async Task Create(PostBLL post) //post
		{
			if (post == null)
				throw new Exception("Post is null");
			string path;
			var rand = new Random();
			do
			{
				path = @"C:\data\Projects\Finalv2\photo_archive\WebAPI\App_Data\" + rand.Next(1000).ToString() + ".jpg";
			}
			while (File.Exists(path));
			string base64 = post.Content.Substring(post.Content.LastIndexOf(',') + 1);
			
			File.WriteAllBytes(path, Convert.FromBase64String(base64));
			
			var client = new ImgurClient("", "");
			var endpoint = new ImageEndpoint(client);
			IImage image;
			using (var fs = new FileStream(path, FileMode.Open))
			{
				image = await endpoint.UploadImageStreamAsync(fs);
			}
			
			post.Url = image.Link;
			//File.Delete(path);
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
