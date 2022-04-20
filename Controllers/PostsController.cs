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
    public class PostsController : Controller
    {
        private readonly ApplicationContext _context;

        public PostsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Posts
                .Include(p => p.Category)
                .OrderByDescending(p => p.Date);

            return View(await applicationContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            var tagsIds = _context.PostTags.Where(pt => pt.PostId == post.Id).Select(pt => pt.TagId).ToList();

            ViewData["SelectedTags"] = _context.Tags.Where(t => tagsIds.Contains(t.Id)).ToList();

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Name");
            return View(new Post() { });
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int[] tags, [Bind("Id,Title,Content,Date,EditDate,CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Date = DateTime.Now;
                _context.Add(post);
                await _context.SaveChangesAsync();

                var postTagsToAdd = new List<PostTag>();

                foreach (var tagId in tags)
                {
                    postTagsToAdd.Add(new PostTag() { PostId = post.Id, TagId = tagId });
                }

                _context.AddRange(postTagsToAdd);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);

            var selectedTagIds = _context.PostTags.Where(pt => pt.PostId == post.Id).Select(pt => pt.TagId).ToList();
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Name", selectedTagIds);
            
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            var selectedTagIds = _context.PostTags.Where(pt => pt.PostId == post.Id).Select(pt => pt.TagId).ToList();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Name", selectedTagIds);
            
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int[] tags, [Bind("Id,Title,Content,Date,EditDate,CategoryId")] Post post)
        {
            post.EditDate = DateTime.Now;
            if (id != post.Id)
            {
                return NotFound();
            }


            var postTagsToRemove = _context.PostTags
                .Where(pt => pt.PostId == post.Id).ToList();

            _context.PostTags.RemoveRange(postTagsToRemove);
            await _context.SaveChangesAsync();

            var postTagsToAdd = new List<PostTag>();

            foreach(var tagId in tags)
            {
                postTagsToAdd.Add(new PostTag() { PostId = post.Id, TagId = tagId });
            }

            _context.AddRange(postTagsToAdd);
            await _context.SaveChangesAsync();

            if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);

            var selectedTagIds = _context.PostTags.Where(pt => pt.PostId == post.Id).Select(pt => pt.TagId).ToList();
            ViewData["TagIds"] = new MultiSelectList(_context.Tags, "Id", "Name", selectedTagIds);

            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
