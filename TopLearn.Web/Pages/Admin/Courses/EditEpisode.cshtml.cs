using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.Cource;

namespace TopLearn.Web.Pages.Admin.Courses
{
    public class EditEpisodeModel : PageModel
    {
        private ICourseService _courseService;

        public EditEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public CourseEpisode CourseEpisode { get; set; }

        public void OnGet(int id)
        {
            CourseEpisode = _courseService.GetEpisodeById(id);

        }

        public IActionResult OnPost(IFormFile fileEpisode)
        {
            if (string.IsNullOrEmpty(CourseEpisode.EpisodeTitle))
            {
                return Page();
            }
            if (fileEpisode != null)
            {
                if (_courseService.CheckExistFile(fileEpisode.FileName))
                {
                    ModelState.AddModelError("CourseEpisode.EpisodFileName", "نام فایل تکراری میباشد");
                    return Page();
                }
            }

            _courseService.EditEpisode(CourseEpisode, fileEpisode);

            return RedirectToPage("Index");
        }
    }
}
