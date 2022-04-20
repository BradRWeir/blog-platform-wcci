using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using template_csharp_blog;
using template_csharp_blog.Models;

namespace template_csharp_blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var homePage = new List<(Post, List<Tag>)>();

            var categories = _context.Categories.ToList();

            foreach (var category in categories)
            {
                var post = _context.Posts
                    .Where(p => p.CategoryId == category.Id)
                    .OrderByDescending(p => p.Date)
                    .FirstOrDefault();

                if (post != null) {
                    var tagIds = _context.PostTags
                        .Where(pt => pt.PostId == post.Id)
                        .Select(pt => pt.TagId)
                        .ToList();

                    var tags = _context.Tags
                        .Where(t => tagIds.Contains(t.Id))
                        .ToList();

                    homePage.Add((post, tags));
                }
            }

            /*var post = await _context.Posts
                .Include(p => p.Category)
                .OrderByDescending(p => p.Date)
                .FirstOrDefaultAsync();*/
/*            if (post == null)
            {
                return NotFound();
            }*/

            /*var tagsIds = _context.PostTags.Where(pt => pt.PostId == post.Id).Select(pt => pt.TagId).ToList();

            ViewData["SelectedTagNames"] = _context.Tags.Where(t => tagsIds.Contains(t.Id)).Select(t => t.Name).ToList();*/

            return View(homePage);
        }
    }
}
