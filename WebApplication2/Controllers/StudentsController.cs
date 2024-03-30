using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.ViewComponents;

namespace WebApplication2.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentContext _context;
        private readonly IWebHostEnvironment _enc;
        public StudentsController(StudentContext context, IWebHostEnvironment enc)
        {
            _context = context;
            _enc = enc;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var data = await _context.Students.Include(i => i.StudentMarks).ThenInclude(s => s.Subject).ToListAsync();

            ViewBag.Count = data.Count;

            ViewBag.ObtainedTotal = data.Sum(i => i.StudentMarks.Sum(l => l.ObtainedNumber));

            //ViewBag.Average = data.Average(i=> i.Items.Sum(l=> l.ItemTotal)) ;

            ViewBag.Average = data.Count > 0 ? data.Average(i => i.StudentMarks.Sum(l => l.ObtainedNumber)) : 0;

            return View(data);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.Include(i => i.StudentMarks).ThenInclude(s => s.Subject)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View(new Students());
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Students student, string command = "")
        {


            if (student.ImageUpload != null)
            {



                student.StudentImage = "\\Image\\" + student.ImageUpload.FileName;


                string serverPath = _enc.WebRootPath + student.StudentImage;


                using FileStream stream = new FileStream(serverPath, FileMode.Create);


                await student.ImageUpload.CopyToAsync(stream);

                TempData["image"] = student.StudentImage;

			}
            else
            {
                student.StudentImage = TempData["image"]?.ToString();
            }
            if (command == "Add")
            {
                student.StudentMarks.Add(new());
                return View(student);
            }
            else if (command.Contains("delete"))// delete-3-sdsd-5   ["delete", "3"]
            {
                int idx = int.Parse(command.Split('-')[1]);

                student.StudentMarks.RemoveAt(idx);
                ModelState.Clear();
                return View(student);
            }

            if (ModelState.IsValid)
            {


                //_context.Add(student);
                //await _context.SaveChangesAsync();
                var rows = await _context.Database.ExecuteSqlRawAsync("exec SpInsertStudent @p0, @p1, @p2, @p3 ,@p4,@p5", student.Name, student.Address, student.ContactNo, student.StudentImage, student.Class, student.Regular);


                if (rows > 0)
                {
                    student.ID = _context.Students.Max(x => x.ID);

                    foreach (var Marks in student.StudentMarks)
                    {
                        await _context.Database.ExecuteSqlRawAsync("exec SpInsertStudentMarks @p0, @p1, @p2,@p3",  Marks.SubjectId, Marks.ObtainedNumber, Marks.StartDate, student.ID);
                    }

                    return RedirectToAction(nameof(Index));
                }
                return View(student);
            }
            return View(student);
                
        }
            // GET: Students/Edit/5ude
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var student = await _context.Students.Include(i => i.StudentMarks).ThenInclude(s => s.Subject)
         .FirstOrDefaultAsync(m => m.ID == id);
                if (student == null)
                {
                    return NotFound();
                }
                return View(student);
            }

            // POST: Students/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to.
            // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, Students student,string command="")
        {
            if (student.ImageUpload != null)
            {



                student.StudentImage = "\\Image\\" + student.ImageUpload.FileName;


                string serverPath = _enc.WebRootPath + student.StudentImage;


                using FileStream stream = new FileStream(serverPath, FileMode.Create);


                await student.ImageUpload.CopyToAsync(stream);
                TempData["image"] = student.StudentImage;

            }
            else
            {
                student.StudentImage = TempData["image"]?.ToString();
            }


            if (command == "Add")
            {
                student.StudentMarks.Add(new());
                return View(student);
            }
            else if (command.Contains("delete"))// delete-3   ["delete", "3"]
            {
                int idx = int.Parse(command.Split('-')[1]);

                student.StudentMarks.RemoveAt(idx);
                ModelState.Clear();
                return View(student);
            }
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    //_context.Update(invoice);
                    ////await _context.SaveChangesAsync();

                    //var itemsIdList = invoice.Items.Select(i => i.ItemId).ToList();

                    //var delItems = await _context.InvoiceItems.Where(i => i.InvoiceId == id).Where(i => !itemsIdList.Contains(i.ItemId)).ToListAsync();


                    //_context.InvoiceItems.RemoveRange(delItems);


                    //await _context.SaveChangesAsync();



                    var row = await _context.Database.ExecuteSqlRawAsync("exec SpUpdateStudent @p0, @p1, @p2, @p3, @p4,@p5,@p6", student.Name, student.Address, student.ContactNo, student.StudentImage, student.Class, student.Regular, student.ID);

                    foreach (var marks in student.StudentMarks)
                    {
                        await _context.Database.ExecuteSqlRawAsync("exec SpInsertStudentMarks @p0, @p1, @p2, @p3",  marks.SubjectId, marks.ObtainedNumber, marks.StartDate ,student.ID);
                    }

                    return RedirectToAction(nameof(Index));


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentsExists(student.ID))
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
            return View(student);

        }
        

            // GET: Students/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var student = await _context.Students.Include(i => i.StudentMarks).ThenInclude(s => s.Subject)
         .FirstOrDefaultAsync(m => m.ID == id);
                if (student == null)
                {
                    return NotFound();
                }

                return View(student);
            }

            // POST: Students/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
            //var students = await _context.Students.FindAsync(id);
            //if (students != null)
            //{
            //    _context.Students.Remove(students);
            //}

            await _context.Database.ExecuteSqlAsync($"exec SpDeleteStudent {id}");
            await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        [HttpDelete]
        [Route("~/deletestudent/{id}")]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            //var invoice = await _context.Invoices.FindAsync(id);
            //if (invoice != null)
            //{             

            //    //_context.Invoices.Remove(invoice);
            //}
            await _context.Database.ExecuteSqlAsync($"exec SpDeleteStudent {id}");

            //await _context.SaveChangesAsync();

            return Ok();
        }

        private bool StudentsExists(int id)
            {
                return _context.Students.Any(e => e.ID == id);
            }
        }
    }
