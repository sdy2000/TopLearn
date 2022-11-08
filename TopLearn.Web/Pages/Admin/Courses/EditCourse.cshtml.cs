using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.Cource;

namespace TopLearn.Web.Pages.Admin.Courses
{
    public class EditCourseModel : PageModel
    {
        private ICourseService _courseService;

        public EditCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public Course Course { get; set; }
        public void OnGet(int id)
        {
            Course = _courseService.GetCourseById(id);


            #region INPUTES VALUE


            List<SelectListItem> groups = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value="0"}
            }; groups.AddRange(_courseService.GetGroupForManageCourse());
            ViewData["Groups"] = new SelectList(groups, "Value", "Text",Course.GroupId);


            List<SelectListItem> subGroup = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value="0"}
            }; subGroup.AddRange(_courseService.GetSubGroupForManageCourse(Course.GroupId));
            ViewData["SubGroup"] = new SelectList(subGroup, "Value", "Text", Course.SubGroup??0);


            List<SelectListItem> teacher = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value="0"}
            }; teacher.AddRange(_courseService.GetTeachers());
            ViewData["Teachers"] = new SelectList(teacher, "Value", "Text", Course.TeacherId);


            List<SelectListItem> Levels = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value="0"}
            }; Levels.AddRange(_courseService.GetLevels());
            ViewData["Levels"] = new SelectList(Levels, "Value", "Text",Course.LevelId);


            List<SelectListItem> Statues = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value="0"}
            }; Statues.AddRange(_courseService.GetStatues());
            ViewData["Statues"] = new SelectList(Statues, "Value", "Text",Course.StatusId);

            #endregion


        }

        public IActionResult OnPost(IFormFile imgCourseUp, IFormFile demoUp)
        {
            #region VALIDATION INPUTES

            if (string.IsNullOrEmpty(Course.CourseTitle) || string.IsNullOrEmpty(Course.Description) ||
                string.IsNullOrEmpty(Course.Tags) || Course.GroupId == 0 ||
                Course.TeacherId == 0 || Course.LevelId == 0 || Course.StatusId == 0)
            {
                #region INPUTES VALUE


                List<SelectListItem> groups = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value="0"}
            }; groups.AddRange(_courseService.GetGroupForManageCourse());
                ViewData["Groups"] = new SelectList(groups, "Value", "Text", Course.GroupId);


                List<SelectListItem> subGroup = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value="0"}
            }; subGroup.AddRange(_courseService.GetSubGroupForManageCourse(int.Parse(groups.First().Value)));
                ViewData["SubGroup"] = new SelectList(subGroup, "Value", "Text", Course.SubGroup ?? 0);


                List<SelectListItem> teacher = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value="0"}
            }; teacher.AddRange(_courseService.GetTeachers());
                ViewData["Teachers"] = new SelectList(teacher, "Value", "Text", Course.TeacherId);


                List<SelectListItem> Levels = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value="0"}
            }; Levels.AddRange(_courseService.GetLevels());
                ViewData["Levels"] = new SelectList(Levels, "Value", "Text", Course.LevelId);


                List<SelectListItem> Statues = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value="0"}
            }; Statues.AddRange(_courseService.GetStatues());
                ViewData["Statues"] = new SelectList(Statues, "Value", "Text", Course.StatusId);

                #endregion
                ModelState.AddModelError("Course.CoursePrice", "لطفا لیست را کامل کنید .");
                return Page();

            }

            #endregion


            _courseService.UpdateCourse(Course, imgCourseUp, demoUp);


            return RedirectToPage("Index");
        }
    }
    
}
