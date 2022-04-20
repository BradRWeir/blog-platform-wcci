using System.Collections.Generic;

namespace template_csharp_blog.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Post> Posts { get; set; }
    }
}
