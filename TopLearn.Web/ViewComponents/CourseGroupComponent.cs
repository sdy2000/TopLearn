using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Servises.Interfaces;

namespace TopLearn.Web.ViewComponents
{
    public class CourseGroupComponent : ViewComponent
    {
        private ICourseService _courseService;
        public CourseGroupComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("CoureGroup",_courseService.GetAllGroup()));
        }
    }
}
