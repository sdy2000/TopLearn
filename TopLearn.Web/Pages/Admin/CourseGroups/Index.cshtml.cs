using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.Cource;

namespace TopLearn.Web.Pages.Admin.CourseGroups
{
    public class IndexModel : PageModel
    {
        private ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }


        public List<CourseGroup> CourseGroup { get; set; }
        public void OnGet()
        {
            CourseGroup = _courseService.GetAllGroup();
        }
    }
}
