using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LinkAggregatorMVC.Data;
using LinkAggregatorMVC.Models;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Components.Web;
using LinkAggregatorMVC.Helpers;

namespace LinkAggregatorMVC.Controllers
{
    public class LinksController : Controller
    {
        private readonly ApplicationDbContext _context;
       
        IConfiguration? myConfiguration;
        private readonly ILog logger;
        private readonly IWebHostEnvironment webEnv;
        public LinksController(ApplicationDbContext context, ILog Logger, IConfiguration configuration, IWebHostEnvironment webenv)
        {
            _context = context;
            
            myConfiguration = configuration;
            logger = Logger;
            webEnv = webenv;
        }

        // GET: Links
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, int? pageSize)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var mycategory = new List<SelectListItem>();
            foreach (var category in _context.CategoriesLookup)
            {
                SelectListItem s = new SelectListItem { Value = category.ID.ToString(), Text = category.Category };
                mycategory.Add(s);
            }
            ViewData["Categories"] = mycategory;

            IQueryable<Links> links = _context.Links.Include(s => s.LinkImages).Include(s => s.LinkCategories);

            if (!String.IsNullOrEmpty(searchString))
            {
                links = links.Where(s => s.Caption!.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    links = links.OrderByDescending(s => s.Caption);
                    break;
                case "Date":
                    links = links.OrderBy(s => s.LinkPostingDate);
                    break;
                case "date_desc":
                    links = links.OrderByDescending(s => s.LinkPostingDate);
                    break;
                default:
                    links = links.OrderBy(s => s.Caption);
                    break;
            }
            

            if (pageSize==null || pageSize==0)
            {
                pageSize = 20;
            }
            ViewData["LinkPageSize"] = pageSize;
            return View(await PaginatedList<Links>.CreateAsync(links.AsNoTracking(), pageNumber ?? 1, (int) pageSize));
            //return View(await links.AsNoTracking().ToListAsync());
        }

        // GET: Links/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var links = await _context.Links.Include(s => s.LinkImages).Include(s => s.LinkCategories)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (links == null)
            {
                return NotFound();
            }
            var mycategory = new List<SelectListItem>();
            foreach (var category in _context.CategoriesLookup)
            {
                SelectListItem s = new SelectListItem { Value = category.ID.ToString(), Text = category.Category };
                mycategory.Add(s);
            }

            ViewData["Categories"] = mycategory;



            return View(links);
        }

        // GET: Links/Create
        public IActionResult Create()
        {
            var mycategory = new List<SelectListItem>();
            foreach(var category in _context.CategoriesLookup)
            {
                SelectListItem s = new SelectListItem { Value = category.ID.ToString(), Text = category.Category };
                mycategory.Add(s);
            }
            var links = new Links();
            links.LinkPostingDate = DateTime.Now.Date;
            links.Categories = mycategory;
            links.SelectedCategories = Array.Empty<int>();
            return View(links);
        }

        // POST: Links/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Caption,URL,LinkPostingDate,Rating,Importance,Files,SelectedCategories")] Links links)
        {
            if (ModelState.IsValid)
            {
                if (links.Files != null)
                {
                    if (links.Files!.Count > 0)
                    {
                        //string? path = myConfiguration!.GetSection("ImageStorageLocation").Value;

                        string? path = Path.Combine(webEnv.WebRootPath, "images");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path!);
                        // i need to add error handling for the files, i already created a Message property
                        foreach (var file in links.Files)
                        {
                            FileInfo fileInfo = new FileInfo(file.FileName);
                            string fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                            string fileNameWithPath = Path.Combine(path!, fileName);
                            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }

                            LinkImages linkImage = new LinkImages();
                            linkImage.ImagePath = fileName;
                            links.LinkImages?.Add(linkImage);
                        }

                    }
                }
                if (links.SelectedCategories != null)
                {

                    if (links.SelectedCategories!.Length > 0)
                    {
                        foreach (int i in links.SelectedCategories)
                        {
                            LinkCategories linkcategories = new LinkCategories();
                            linkcategories.CategoriesLookup = i;
                            links.LinkCategories!.Add(linkcategories);

                        }
                    }
                }

                _context.Add(links);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(links);
        }

        // GET: Links/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var links = await _context.Links.Include(s => s.LinkImages).Include(x =>x.LinkCategories)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (links == null)
            {
                return NotFound();
            }

            var mycategory = new List<SelectListItem>();
            foreach (var category in _context.CategoriesLookup)
            {
                SelectListItem s = new SelectListItem { Value = category.ID.ToString(), Text = category.Category };
                mycategory.Add(s);
            }
            links.Categories = mycategory;
            List<int> selected = new List<int>();
            foreach(var category in links.LinkCategories!)
            {
                selected.Add((int)category.CategoriesLookup!);
            }

            links.SelectedCategories = selected.ToArray();

            return View(links);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Caption,URL,LinkPostingDate,Rating,Importance,TotalClicks,Files,SelectedCategories")] Links links, List<string> DeleteImageList)
        {
            if (id != links.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                try
                {
                    if (links.Files != null)
                    {
                        if (links.Files!.Count > 0)
                        {
                            //string? path = myConfiguration!.GetSection("ImageStorageLocation").Value;

                            string? path = Path.Combine(webEnv.WebRootPath, "images");

                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path!);
                            // i need to add error handling for the files, i already created a Message property
                            foreach (var file in links.Files)
                            {
                                FileInfo fileInfo = new FileInfo(file.FileName);
                                string fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                                string fileNameWithPath = Path.Combine(path!, fileName);
                                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                }

                                LinkImages linkImage = new LinkImages();
                                linkImage.ImagePath = fileName;
                                links.LinkImages?.Add(linkImage);
                            }

                        }
                    }

                    _context.Update(links);
                    await _context.SaveChangesAsync();

                    var links2 = await _context.Links.Include(s => s.LinkImages).FirstOrDefaultAsync(m => m.ID == id);

                    var linkimages = links2!.LinkImages!.ToArray();

                    for (int i = DeleteImageList.Count - 1; i >= 0; i--)
                    {
                        links2.LinkImages!.Remove(links.LinkImages!.Where(s => s.ID == Convert.ToInt64(DeleteImageList[i])).FirstOrDefault()!);

                    }
                    _context.Update(links2);
                    await _context.SaveChangesAsync();

                    var links3 = await _context.Links.Include(s => s.LinkCategories).FirstOrDefaultAsync(m => m.ID == id);
                    ////delete them all, then just add again
                    var linkcategories = links3!.LinkCategories!.ToArray();
                    for (int i = linkcategories.Length - 1; i >= 0; i--)
                    {
                        links3.LinkCategories!.Remove(linkcategories[i]);
                    }

                    // _context.Links.Remove(links3);
                    if (links.SelectedCategories != null)
                    {
                        if (links.SelectedCategories!.Length > 0)
                        {
                            foreach (int i in links.SelectedCategories)
                            {
                                LinkCategories linkcategories2 = new LinkCategories();
                                linkcategories2.CategoriesLookup = i;
                                links3.LinkCategories!.Add(linkcategories2);

                            }
                        }
                    }

                    _context.Update(links3);



                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LinksExists(links!.ID))
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
            return View(links);
        }

        // GET: Links/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var links = await _context.Links.Include(s => s.LinkImages).Include(s => s.LinkCategories)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (links == null)
            {
                return NotFound();
            }

            return View(links);
        }

        // POST: Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var links = await _context.Links.Include(s => s.LinkImages).Include(s => s.LinkCategories).FirstOrDefaultAsync(m => m.ID == id);
            if (links != null)
            {
                var linkimages = links.LinkImages!.ToArray();
                for(int i=linkimages.Length-1;i>=0;i--)
                {
                    links.LinkImages!.Remove(linkimages[i]);
                }
                var linkcategories = links.LinkCategories!.ToArray();
                for (int i = linkcategories.Length - 1; i >= 0; i--)
                {
                    links.LinkCategories!.Remove(linkcategories[i]);
                }

                _context.Links.Remove(links);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LinksExists(int id)
        {
            return _context.Links.Any(e => e.ID == id);
        }
    }
}
