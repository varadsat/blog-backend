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
        private readonly BlogDbContext _context;
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
        public async Task<Post?> GetPostById(int postId)
        {
            var post = await _context.Posts.Include(x => x.Author).FirstOrDefaultAsync(p => p.Id == postId);
            if (post is not null)
                return post;
            return null;
        }
        public async Task AddPost(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return;
        }
        public async Task<Post?> DeletePost(int postId)
        {
            var itemToRemove = await _context.Posts.FirstOrDefaultAsync(x => x.Id == postId);
            if (itemToRemove != null)
            {
                _context.Posts.Remove(itemToRemove);
                await _context.SaveChangesAsync();
            }
            return itemToRemove;
        }
    }
}
