using Blog.Data;
using Blog.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class PostRepository
    {
        private BlogDbContext _context;
        public PostRepository(BlogDbContext context)
        {
            _context = context;
        }
        public async Task<List<PostDTO>> GetAllPosts()
        {
            var postDTO = _context.Posts.Select(p => new PostDTO()
            {
                Id = p.Id,
                Title = p.Title,
                Body = p.Body,
                PublishedAt = p.PublishedAt,
                AuthorId = p.AuthorId
            });
            var posts = await postDTO.ToListAsync();
            return posts;
        }
    }
}
