﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DAL.Models;
using DAL.Context;

namespace DAL.Repository
{
	public class PostRepository : IRepository<Post>
	{
		private PostContext db;
		public PostRepository(PostContext context)
		{
			db = context;
		}
		public IEnumerable<Post> GetList() //get
		{
			return db.Posts;
		}
		public Post GetItem(int id) //get
		{
			return db.Posts.Find(id);
		}
		public void Create(Post item) //post
		{
			db.Posts.Add(item);
		}
		public void Update(Post item) //put
		{
			db.Entry(item).State = EntityState.Modified;
		}
		public void Delete(int id) //delete
		{
			Post posttodelete = db.Posts.Find(id);
			if (posttodelete != null)
				db.Posts.Remove(posttodelete);
		}
	}
}
