using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.Cource;

namespace TopLearn.Web.Pages.Admin.Courses
{
    public class IndexEpisodeModel : PageModel
    {
        private ICourseService _courseService;

        public IndexEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public List<CourseEpisode> CourseEpisode { get; set; }
        public void OnGet(int id)
        {
            ViewData["CourseId"] = id;

            CourseEpisode = _courseService.GetListEpisodeCorse(id);
        }
    }
}
