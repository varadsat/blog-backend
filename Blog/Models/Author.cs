using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string ImageUrl { get; set; }
       // public ICollection<Post> Posts { get; set; }


    }
}
