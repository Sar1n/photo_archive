using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
	public class PostWEB
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
	}
}