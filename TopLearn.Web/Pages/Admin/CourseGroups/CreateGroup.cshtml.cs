using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.Cource;

namespace TopLearn.Web.Pages.Admin.CourseGroups
{
    public class CreateGroupModel : PageModel
    {
        private ICourseService _courseService;

        public CreateGroupModel(ICourseService courseService)
        {
            _courseService = courseService;
        }


        [BindProperty]
        public CourseGroup CourseGroup { get; set; }
        public void OnGet(int? id)
        {
            CourseGroup = new CourseGroup()
            {
                ParentId = id
            };
        }


        public IActionResult OnPost()
        {
            if (CourseGroup.GroupTitle == null)
                return Page();


            _courseService.AddGroup(CourseGroup);

            return RedirectToPage("Index");
        }
    }
}
