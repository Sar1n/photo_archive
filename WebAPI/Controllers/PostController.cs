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
		//MapperConfiguration IEBLLtoWEB = new MapperConfiguration(cfg => cfg.CreateMap<IEnumerable<PostBLL>, IEnumerable<PostWEB>>());
		IPostService service;

		// GET api/post
		//public IEnumerable<string> Get() //IEnumerable<string>
		//{
		//	string connstr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
		//	IKernel Kernal = new StandardKernel();
		//	Kernal.Bind<IPostService>().To<PostService>();
		//	service = Kernal.Get<IPostService>(new ConstructorArgument("connectionString", connstr));

		//	IEnumerable<PostBLL> listofposts = service.GetAll();
		//	string respond = "";
		//	var mapper = new Mapper(BLLtoWEB);
		//	List<string> strlist = new List<string>();
		//	foreach (PostBLL i in listofposts)
		//	{
		//		PostWEB postWEB = mapper.Map<PostBLL, PostWEB>(i);
		//		respond = postWEB.Id.ToString() + " - " + postWEB.Content + " ";
		//		strlist.Add(respond);
		//	}
		//	string[] strres;
		//	strres = (from i in strlist select i).ToArray();
		//	return strres;
		//}
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
			//response.Headers.Add("Access-Control-Allow-Origin", "*");

			return response;
		}

		// GET api/post/5
		public string Get(int id)
		{
			string connstr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
			IKernel Kernal = new StandardKernel();
			Kernal.Bind<IPostService>().To<PostService>();
			service = Kernal.Get<IPostService>(new ConstructorArgument("connectionString", connstr));
			
			PostBLL find = service.GetOne(id);
			var mapper = new Mapper(BLLtoWEB);
			PostWEB postWEB = mapper.Map<PostBLL, PostWEB>(find);
			string respond = postWEB.Content;
			return respond;
		}

		// POST api/post
		public HttpResponseMessage Post(HttpRequestMessage value)
		{
			//throw new Exception("BENIS");
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