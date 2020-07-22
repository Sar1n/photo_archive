using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Ninject.Parameters;
using System.Net;
using AutoMapper;
using BLL.DataTransferObjects;
using WebAPI.Models;
using System.Net.Http;
using System.Web.Http;
using BLL.Services;
using System.Configuration; //web.config
using System.Web.Http.Cors; //cors
using System.Text.Json;

using System.Data.Entity;

namespace WebAPI.Controllers
{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
	public class PostController : ApiController
    {
		MapperConfiguration WEBtoBLL = new MapperConfiguration(cfg => cfg.CreateMap<PostWEB, PostBLL>());
		MapperConfiguration BLLtoWEB = new MapperConfiguration(cfg => cfg.CreateMap<PostBLL, PostWEB>());
		IPostService service;
		
		public HttpResponseMessage Get()
		{
			string connstr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
			IKernel Kernal = new StandardKernel();
			Kernal.Bind<IPostService>().To<PostService>();
			service = Kernal.Get<IPostService>(new ConstructorArgument("connectionString", connstr));

			IEnumerable<PostBLL> listOfPosts = service.GetAll();
			var mapper = new Mapper(BLLtoWEB);

			List<PostWEB> listOfPostsWEB = mapper.Map<IEnumerable<PostBLL>, List<PostWEB>>(listOfPosts);

			string responseBody = JsonSerializer.Serialize(listOfPostsWEB);

			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
			response.Content = new StringContent(responseBody);

			return response;
		}

		// GET api/post/5
		public HttpResponseMessage Get(string searchstring)
		{
			string connstr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
			IKernel Kernal = new StandardKernel();
			Kernal.Bind<IPostService>().To<PostService>();
			service = Kernal.Get<IPostService>(new ConstructorArgument("connectionString", connstr));

			IEnumerable<PostBLL> listOfPosts = service.GetSome(searchstring);
			var mapper = new Mapper(BLLtoWEB);

			List<PostWEB> listOfPostsWEB = mapper.Map<IEnumerable<PostBLL>, List<PostWEB>>(listOfPosts);

			string responseBody = JsonSerializer.Serialize(listOfPostsWEB);

			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
			response.Content = new StringContent(responseBody);

			return response;
		}
		// GET api/post/5
		public HttpResponseMessage Get(int id)
		{
			string connstr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
			IKernel Kernal = new StandardKernel();
			Kernal.Bind<IPostService>().To<PostService>();
			service = Kernal.Get<IPostService>(new ConstructorArgument("connectionString", connstr));

			PostBLL Post = service.GetOne(id);
			var mapper = new Mapper(BLLtoWEB);

			PostWEB PostWEB = mapper.Map<PostBLL, PostWEB>(Post);

			string responseBody = JsonSerializer.Serialize(PostWEB);

			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
			response.Content = new StringContent(responseBody);

			return response;
		}

		// POST api/post
		public HttpResponseMessage Post(HttpRequestMessage value)
		{
			string connstr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
			IKernel Kernal = new StandardKernel();
			Kernal.Bind<IPostService>().To<PostService>();
			service = Kernal.Get<IPostService>(new ConstructorArgument("connectionString", connstr));
			string res;
			if (value == null)
				throw new Exception();
			else
			{
				res = value.Content.ReadAsStringAsync().Result;
			}
			PostWEB post = JsonSerializer.Deserialize<PostWEB>(res);
			var mapper = new Mapper(WEBtoBLL);
			PostBLL postBLL = mapper.Map<PostWEB, PostBLL>(post);
			service.Create(postBLL);


			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Access-Control-Allow-Origin", "*");
		
			return response;
		}

		// PUT api/post/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/post/5
		public void Delete(int id)
		{
		}
	}
}