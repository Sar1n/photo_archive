using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.Services;
using Ninject.Modules;

namespace WebAPI
{
	public class NinjectBindings : NinjectModule
	{
		public override void Load()
		{
			Bind<IPostService>().To<PostService>();
		}
	}
}