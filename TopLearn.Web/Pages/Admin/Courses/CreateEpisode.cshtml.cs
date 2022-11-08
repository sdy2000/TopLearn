using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.Cource;

namespace TopLearn.Web.Pages.Admin.Courses
{
    public class CreateEpisodeModel : PageModel
    {
        private ICourseService _courseService;

        public CreateEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public CourseEpisode CourseEpisode { get; set; }

        public void OnGet(int id)
        {
            CourseEpisode = new CourseEpisode();
            CourseEpisode.CourseId = id;
        }

        public IActionResult OnPost(IFormFile fileEpisode)
        {
            if (string.IsNullOrEmpty(CourseEpisode.EpisodeTitle) || fileEpisode == null)
            {
                return Page();
            }
            if (_courseService.CheckExistFile(fileEpisode.FileName))
            {
                ModelState.AddModelError("CourseEpisode.EpisodFileName", "نام فایل تکراری میباشد");
                return Page();
            }

            _courseService.AddEpisode(CourseEpisode, fileEpisode);

            return RedirectToPage("Index");
        }
    }
}
