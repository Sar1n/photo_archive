using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Ninject.Parameters;
using System.Net;
using AutoMapper;
using BLL.DataTransferObjects;
using WebAPI.Models;
using System.Net.Http;
using System.Web.Http;
using BLL.Services;
using System.Configuration;
using System.Web.Http.Cors;
using System.Text.Json;


namespace API.Controllers
{
	public class PostController : ApiController
	{
		MapperConfiguration WEBtoBLL = new MapperConfiguration(cfg => cfg.CreateMap<PostWEB, PostBLL>());
		MapperConfiguration BLLtoWEB = new MapperConfiguration(cfg => cfg.CreateMap<PostBLL, PostWEB>().ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Url)));
		IPostService service
		{
			get
			{
				string connstr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
				IKernel Kernal = new StandardKernel();
				Kernal.Bind<IPostService>().To<PostService>();
				return Kernal.Get<IPostService>(new ConstructorArgument("connectionString", connstr));
			}
		}
		// GET api/post
		public HttpResponseMessage Get()
		{
			IEnumerable<PostBLL> listOfPosts = service.GetAll();
			var mapper = new Mapper(BLLtoWEB);

			List<PostWEB> listOfPostsWEB = mapper.Map<IEnumerable<PostBLL>, List<PostWEB>>(listOfPosts);

			string responseBody = JsonSerializer.Serialize(listOfPostsWEB);

			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
			response.Content = new StringContent(responseBody);

			return response;
		}

		// GET api/post/searchstring
		public HttpResponseMessage Get(string searchstring)
		{
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
			PostBLL Post = service.GetOne(id);
			var mapper = new Mapper(BLLtoWEB);

			PostWEB PostWEB = mapper.Map<PostBLL, PostWEB>(Post);

			string responseBody = JsonSerializer.Serialize(PostWEB);

			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
			response.Content = new StringContent(responseBody);

			return response;
		}
		// POST api/post/
		public async Task<HttpResponseMessage> Post(PostWEB value)
		{
			if (value == null)
				throw new Exception();
			var mapper = new Mapper(WEBtoBLL);
			PostBLL postBLL = mapper.Map<PostWEB, PostBLL>(value);

			await service.Create(postBLL);

			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

			return response;
		}
	}
}