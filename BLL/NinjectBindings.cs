using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using DAL.UnitOfWork;

namespace BLL
{
	class NinjectBindings : NinjectModule
	{
		public override void Load()
		{
			Bind<IUnitOfWork>().To<UnitOfWork>();
		}
	}
}
