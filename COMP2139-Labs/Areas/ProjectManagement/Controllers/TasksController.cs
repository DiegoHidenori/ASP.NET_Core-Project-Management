using COMP2139_Labs.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using COMP2139_Labs.Areas.ProjectManagement.Models;
using Microsoft.CodeAnalysis;

namespace COMP2139_Labs.Areas.ProjectManagement.Controllers
{
    /*
     * Explain...
     */
    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class TasksController : Controller
    {
        private readonly AppDbContext _db;
        public TasksController(AppDbContext context)
        {
            _db = context;
        }


        // GET: Tasks
        [HttpGet("Index/{projectId:int}")]
        public async Task<IActionResult> Index(int? projectId)
        {

            // var taskName = "MyTask";


            /*
             * Grabs all the project tasks from the database, where the project ID is the same as the project ID 
             * that we're looking for
             */
            /*
            var result = _db.ProjectTasks.Where(current => current.ProjectId == projectId).ToList();

            result = _db.ProjectTasks.Where(current => current.Title.StartsWith(taskName)).ToList();

            result = _db.ProjectTasks.Where(current => current.Title.Contains(taskName)).ToList();

            result = _db.ProjectTasks.Where(current => current.Title.Contains(taskName) || current.Description.Contains(taskName)).ToList();


            var tasks = _db.ProjectTasks
                .Where(t => t.ProjectId == projectId)
                .ToList();
            ViewBag.ProjectId = projectId; // Store projectId in ViewBag
            return View(tasks);
            */

            var tasksQuery = _db.ProjectTasks.AsQueryable();

            if (projectId.HasValue)
            {
                tasksQuery = tasksQuery.Where(t => t.ProjectId == projectId.Value);
            }

            var tasks = await tasksQuery.ToListAsync();
            ViewBag.ProjectId = projectId; // Store projectId in ViewBag
            return View(tasks);

        }


        // GET: Tasks/Details/5
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var tasksQuery = _db.ProjectTasks.AsQueryable();
            var task = await _db.ProjectTasks
                .Include(t => t.Project) // include related project data
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }


        [HttpGet("Create/{projectId:int}")]
        public async Task<IActionResult> Create(int projectId)
        {
            var project = await _db.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound(); // Or handle appropriately if project doesn't exist
            }

            var task = new ProjectTask
            {
                ProjectId = projectId
            };

            return View(task);
        }

        [HttpPost("Create/{projectId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                await _db.ProjectTasks.AddAsync(task);
                await _db.SaveChangesAsync();

                // Redirect to the Index action with the projectId of the created task
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            // Repopulate the Projects SelectList if returning to the form
            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task); // Redirects to the index action in the same folder
        }


        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _db.ProjectTasks
                .Include(t => t.Project) // Include related project data
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (id != task.ProjectTaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(task);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _db.ProjectTasks
                .Include(t => t.Project) // Include related project data
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost("DeleteConfirmed/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ProjectTaskId)
        {
            var task = await _db.ProjectTasks.FindAsync(ProjectTaskId);
            if (task != null)
            {
                _db.ProjectTasks.Remove(task);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            return NotFound();
        }


        // Lab 5 - Search ProjectTasks
        // GET: Tasks/Search/{projectId}/{searchString?}
        // Tasks/Search?projectId=1&searchString=Task1
        [HttpGet("Search/{projectId:int}/{searchString?}")]
        public async Task<IActionResult> Search(int projectId, string searchString)
        {
            // var tasksQuery = _db.ProjectTasks.Where(t => t.ProjectId == projectId);
            var tasksQuery = _db.ProjectTasks.AsQueryable();
            // bool searchPerformed = !string.IsNullOrEmpty(searchString); // me

            if (!string.IsNullOrEmpty(searchString))
            {
                tasksQuery = tasksQuery.Where(t => t.Title.Contains(searchString) || t.Description.Contains(searchString));
            }

            var tasks = await tasksQuery.ToListAsync();
            ViewBag.ProjectId = projectId; // To keep track of the current project
            return View("Index", tasks); // Reuse the Index view to display results
        }
    }
}
