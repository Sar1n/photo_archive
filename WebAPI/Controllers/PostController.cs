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

using System.Data.Entity;

namespace WebAPI.Controllers
{
    public class PostController : ApiController
    {
		MapperConfiguration WEBtoBLL = new MapperConfiguration(cfg => cfg.CreateMap<PostWEB, PostBLL>());
		MapperConfiguration BLLtoWEB = new MapperConfiguration(cfg => cfg.CreateMap<PostBLL, PostWEB>());
		IPostService service;
		/*public PostController()
		{

		}*/
		// GET api/post
		public IEnumerable<string> Get() //IEnumerable<string>
		{
			string connstr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
			IKernel Kernal = new StandardKernel();
			Kernal.Bind<IPostService>().To<PostService>();
			service = Kernal.Get<IPostService>(new ConstructorArgument("connectionString", connstr));

			IEnumerable<PostBLL> listofposts = service.GetAll();
			string respond = "";
			var mapper = new Mapper(BLLtoWEB);
			//string[] strres = new string[1];
			List<string> strlist = new List<string>();
			foreach (PostBLL i in listofposts)
			{
				PostWEB postWEB = mapper.Map<PostBLL, PostWEB>(i);
				respond = postWEB.Id.ToString() + " - " + postWEB.Content + " ";
				strlist.Add(respond);
				//strres[1] = postWEB.Content;
			}
			string[] strres; //= new string[] { x => foreach i in strlist x = i };
			strres = (from i in strlist select i).ToArray();
			//string respond = postWEB.Content;
			return strres;
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
		public void Post([FromBody]string value)
		{
			string connstr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
			IKernel Kernal = new StandardKernel();
			Kernal.Bind<IPostService>().To<PostService>();
			service = Kernal.Get<IPostService>(new ConstructorArgument("connectionString", connstr));
			PostWEB post = new PostWEB();
			post.Content = value;
			var mapper = new Mapper(WEBtoBLL);
			PostBLL postBLL = mapper.Map<PostWEB, PostBLL>(post);
			service.Create(postBLL);
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