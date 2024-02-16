using Information.Data;
using Information.Models;
using Information.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Information.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public StudentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(StudentViewModel viewModel)
        {
            var student = new Student
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Domain = viewModel.Domain,
                Subscribed = viewModel.Subscribed
            };
            await dbContext.students.AddAsync(student);
            await dbContext.SaveChangesAsync();


            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {

            var students = await dbContext.students.ToListAsync();
            return View(students);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await dbContext.students.FindAsync(id);
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student viewModel)
        {
            var student = await dbContext.students.FindAsync(viewModel.Id);

            if (student is not null)
            {
                student.Name = viewModel.Name;
                student.Email = viewModel.Email;
                student.Domain = viewModel.Domain;
                student.Subscribed = viewModel.Subscribed;

                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Student viewModel)
        {

            var student = await dbContext.students.AsNoTracking().SingleOrDefaultAsync(x => x.Id == viewModel.Id);

        if (student is not null) 
         {
                dbContext.students.Remove(viewModel);     
                await dbContext.SaveChangesAsync();
         }
            return RedirectToAction("List", "Students");

        }
    }
    
}
