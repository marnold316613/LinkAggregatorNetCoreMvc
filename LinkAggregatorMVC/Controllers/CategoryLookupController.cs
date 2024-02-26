using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LinkAggregatorMVC.Data;
using LinkAggregatorMVC.Models;
using log4net;


namespace LinkAggregatorMVC.Controllers
{
    public class CategoryLookupController : Controller
    {
        // GET: CategoryLookupController

        private readonly ApplicationDbContext _context;

        IConfiguration? myConfiguration;
        private readonly ILog logger;
        private readonly IWebHostEnvironment webEnv;
        public CategoryLookupController(ApplicationDbContext context, ILog Logger, IConfiguration configuration, IWebHostEnvironment webenv)
        {
            _context = context;

            myConfiguration = configuration;
            logger = Logger;
            webEnv = webenv;
        }



        public ActionResult Index()
        {
            var catlookup = _context.CategoriesLookup;
            return View(catlookup);
        }

        // GET: CategoryLookupController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryLookup = await _context.CategoriesLookup
                .FirstOrDefaultAsync(m => m.ID == id);
            if (categoryLookup == null)
            {
                return NotFound();
            }
            return View(categoryLookup);
        }

        // GET: CategoryLookupController/Create
        public ActionResult Create()
        {         
            return View();
        }

        // POST: CategoryLookupController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Category)
        {
            if (ModelState.IsValid)
            {
                CategoriesLookup cat = new CategoriesLookup();
                cat.Category = Category;

                _context.Add(cat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: CategoryLookupController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryLookup = await _context.CategoriesLookup
                .FirstOrDefaultAsync(m => m.ID == id);
            if (categoryLookup == null)
            {
                return NotFound();
            }

            return View(categoryLookup);
        }

        // POST: CategoryLookupController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string Category)
        {
            var categoryLookup = await _context.CategoriesLookup
           .FirstOrDefaultAsync(m => m.ID == id);
            if (categoryLookup == null)
            {
                return NotFound();
            }
            categoryLookup.Category = Category;
       
                try
                {
                    _context.Update(categoryLookup);
                    await _context.SaveChangesAsync();
           
                }
                catch (DbUpdateConcurrencyException)
                {
                  
                }
                return RedirectToAction(nameof(Index));  
        }

            // GET: CategoryLookupController/Delete/5
            public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryLookup = await _context.CategoriesLookup
                .FirstOrDefaultAsync(m => m.ID == id);
            if (categoryLookup == null)
            {
                return NotFound();
            }

            return View(categoryLookup);
        }

        // POST: CategoryLookupController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryLookup = await _context.CategoriesLookup
          .FirstOrDefaultAsync(m => m.ID == id);
            if (categoryLookup == null)
            {
                return NotFound();
            }
           

            try
            {
                _context.Remove(categoryLookup);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
