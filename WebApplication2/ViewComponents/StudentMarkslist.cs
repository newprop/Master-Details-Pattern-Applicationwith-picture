using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.ViewComponents
{
    public class StudentMarkslist:ViewComponent
    {
        public IViewComponentResult Invoke(List<Models.StudentExamMarks> data)
        {


            ViewBag.Count = data.Count;
            ViewBag.ObtainedTotal = data.Sum(i => i.ObtainedNumber);

            return View(data);
        }
    }
}
