using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishedAt { get; set; } = DateTime.Now;
        public string[] Tags { get; set; }
        public string Body { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
