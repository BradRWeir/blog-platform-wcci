using System;
using System.Collections.Generic;

namespace template_csharp_blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public DateTime? EditDate { get; set; }
        
        public int CategoryId { get; set; } //FK CategoryId
        public virtual Category Category { get; set; }

        //public virtual ICollection<Tag> Tags { get; set; }
        public virtual IList<PostTag> PostTags { get; set; }

/*        public virtual string TagsDisplay
        {
            get
            {
                foreach (Tag tag in Tags)
                {

                }
            }
        }*/

        //TODO: Identity
        //public BlogId BlogId; //FK BlogId
        //public virtual Blog Blog; //virtual Blog
    }
}
