using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        public DateTime PublishedAt { get; set; } = DateTime.Now;
        public string Body { get; set; }
        public int AuthorId { get; set; }
    }
}
