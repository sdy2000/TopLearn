using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs.Course;
using TopLearn.Core.Servises.Interfaces;

namespace TopLearn.Web.Pages.Admin.Courses
{
    public class IndexModel : PageModel
    {
        private ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public List<ShowCourseForAdminViewModel> ListCourse { get; set; }
        public void OnGet()
        {
            ListCourse = _courseService.GetCourseForAdmin();
        }
    }
}
