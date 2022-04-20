using System.Collections.Generic;

namespace template_csharp_blog.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //public virtual ICollection<Post> Posts { get; set; }
        public virtual IList<PostTag> PostTags { get; set; }
    }
}
