using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using COMP2139_Labs.Areas.ProjectManagement.Models;
using COMP2139_Labs.Data;
using Microsoft.AspNetCore.Authorization;
using COMP2139_Labs.Services;


namespace COMP2139_Labs.Areas.ProjectManagement.Controllers
{
    /*
     * Explain...
     */
    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class ProjectsController : Controller
    {
        // Lab 3
        // AppDbContext class inherits from DbContext,
        //      which creates session w db and lets you
        //      create queries
        private readonly AppDbContext _db;
        //private readonly ILogger<ProjectsController> _logger; // Week 14
        //private readonly ISessionService _sessionService; // Week 14


        /* March 13, 2024
         * Controllers get an instance of the database indicated in the connection string in the appsettings.json
         * because the controllers are the classes that take care of the HTTP requests (i.e., CRUD operations) so they need to
         * interact with the database
         */
        public ProjectsController(AppDbContext db)
        {
            _db = db;
            //_logger = logger; // Week 14
            //_sessionService = sessionService; // Week 14
        }


        /*
         * GET: Projects
         * Accessible at /Projects
         */
        // actions are synonymous to methods inside controllers
        // This index action calls on the index file inside the project folder

        // [Authorize] // ----------------- Week 14 ---------------------
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            //_logger.LogInformation("Calling Project Index() action");

            try
            {
                var projects = await _db.Projects.ToListAsync();
                // _logger.LogInformation("Hello testing inside the try block");

                //var value = _sessionService.GetSessionData<int?>("Visited") ?? 0;

                //ViewBag.mysession = value + 1;
                return View(projects);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return View(null);
            }
        }

        /*
         * GET: Projects/Details/5
         * The ":int" constraint ensures id is an integer
         */
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }


        /*
         * GET: Projects/Create
         */
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }


        /*
         * POST: Projects/Create
         */
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Description, StartDate, EndDate, Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                await _db.AddAsync(project);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project); // Redirects to the index action in the same folder
        }


        /*
         * GET: Projects/Edit/5
         */
        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var project = _db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }


        /*
         * POST: Projects/Edit/5
         */
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId, Name, Description, StartDate, EndDate, Status")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update method doesn't need async, because it doesn't take long
                    _db.Update(project);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProjectExists(project.ProjectId))
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
            return View(project);
        }

        private async Task<bool> ProjectExists(int id)
        {
            return await _db.Projects.AnyAsync(e => e.ProjectId == id);
        }


        /*
         * GET: Projects/Delete/5
         */
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }


        /*
         * POST: Projects/DeleteConfirmed/5
         */
        [HttpPost("DeleteConfirmed/{id:int}")]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ProjectId)
        {
            var project = _db.Projects.Find(ProjectId);
            if (project != null)
            {
                // Remove doesn't need async
                _db.Projects.Remove(project);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Handle the case where the project is not found
            return NotFound();
        }


        // Custom route for search functionality
        // Accessible at /Projects/Search/{searchString?}
        [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var projectsQuery = from p in _db.Projects select p;
            bool searchPerformed = !string.IsNullOrEmpty(searchString);
            if (searchPerformed)
            {
                projectsQuery = projectsQuery.Where(p => p.Name.Contains(searchString) || p.Description.Contains(searchString));
            }

            var projects = await projectsQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", projects); // reuse the Index view to display results
        }
    }
}
