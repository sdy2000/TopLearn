using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpCompress.Archives;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Entities.Cource;

namespace TopLearn.Web.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService _courseService;
        private IOrderService _orderService;
        private IUserService _userService;

        public CourseController(ICourseService courseService,IOrderService orderService, IUserService userService)
        {
            _courseService = courseService;
            _orderService = orderService;
            _userService = userService;
        }


        public IActionResult Index(int pageId = 1, string filter = "", string getType = "all",
            string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> SelectedGroups = null)
        {
            ViewBag.SelectedGroups = SelectedGroups;
            ViewBag.Groups = _courseService.GetAllGroup();
            ViewBag.pageId = pageId;
            return View(_courseService.GetCourse(pageId,filter,getType,orderByType,startPrice,endPrice,SelectedGroups,9));
        }

        

        [Route("ShowCourse/{id}")]
        public IActionResult ShowCourse(int id, int episodeId = 0)
        {
            var course = _courseService.GetCourseForShow(id);
            if (course == null)
            {
                return NotFound();
            }

            if (episodeId != 0 && User.Identity.IsAuthenticated)
            {
                if (course.CourseEpisodes.All(e => e.EpisodeId != episodeId))
                {
                    return NotFound();
                }

                if (!course.CourseEpisodes.First(e => e.EpisodeId == episodeId).IsFree)
                {
                    if (!_orderService.IsUserInCourse(User.Identity.Name, id))
                    {
                        return NotFound();
                    }
                }

                var ep = course.CourseEpisodes.First(e => e.EpisodeId == episodeId);
                ViewBag.Episode = ep;
                string filePath = "";
                string checkFilePath = "";
                if (ep.IsFree)
                {
                    filePath = "/courseOnline/" + ep.EpisodFileName.Replace(".rar", ".mp4");
                    checkFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/courseOnline/",
                        ep.EpisodFileName.Replace(".rar", ".mp4"));
                }
                else
                {
                    filePath = "/CoureseFilesOnline/" + ep.EpisodFileName.Replace(".rar", ".mp4");
                    checkFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CoureseFilesOnline/",
                        ep.EpisodFileName.Replace(".rar", ".mp4"));
                }


                if (!System.IO.File.Exists(checkFilePath))
                {
                    string targetPath = Directory.GetCurrentDirectory();
                    if (ep.IsFree)
                    {
                        targetPath = Path.Combine(targetPath, "wwwroot/courseOnline/");
                    }
                    else
                    {
                        targetPath = Path.Combine(targetPath, "wwwroot/CoureseFilesOnline/");
                    }

                    string rarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFiles/",
                        ep.EpisodFileName);
                    var archive = ArchiveFactory.Open(rarPath);

                    var Entries = archive.Entries.OrderBy(x => x.Key.Length);
                    foreach (var en in Entries)
                    {
                        if (Path.GetExtension(en.Key) == ".mp4")
                        {
                            en.WriteTo(System.IO.File.Create(Path.Combine(targetPath, ep.EpisodFileName.Replace(".rar", ".mp4"))));
                        }
                    }
                }

                ViewBag.filePath = filePath;
            }

            return View(course);
        }

        [Authorize]
        public IActionResult BuyCourse(int id)
        {
           int orderId =  _orderService.AddOrder(User.Identity.Name, id);
            return Redirect("/UserPanel/MyOrders/ShowOrder/"+ orderId);
        }

        [Route("DownloadFile/{episodeId}")]
        public IActionResult DownloadFile(int episodeId)
        {
            var episode = _courseService.GetEpisodeById(episodeId);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFiles", episode.EpisodFileName);
            string fileName = episode.EpisodFileName;

            if (episode.IsFree)
            {
                byte[] file = System.IO.File.ReadAllBytes(filePath);
                return File(file, "application/force-download", fileName);
            }
            if (User.Identity.IsAuthenticated)
            {
                if (_orderService.IsUserInCourse(User.Identity.Name, episode.CourseId))
                {
                    byte[] file = System.IO.File.ReadAllBytes(filePath);
                    return File(file, "application/force-download", fileName);
                }
            }


            return Forbid();
        }


        [HttpPost]
        public IActionResult CreateComment(CourseComment comment)
        {
            comment.IsDelete = false;
            comment.CreateDate = DateTime.Now;
            comment.UserId = _userService.GetUserIdByUserName(User.Identity.Name);
            _courseService.AddComment(comment);

            return View("ShowComment",_courseService.GetCourseComments(comment.CourseId));
        }

        public IActionResult ShowComment(int id,int pageId=1)
        {
            return View(_courseService.GetCourseComments(id, pageId));
        }


        public IActionResult CourseVote(int id)
        {
            if (!_courseService.IsFree(id))
            {
                if (!_orderService.IsUserInCourse(User.Identity.Name, id))
                {
                    ViewBag.NotAccess = true;
                }
            }
                return PartialView(_courseService.GetCourseVote(id));
        }

        [Authorize]
        public IActionResult AddVote(int id,bool vote)
        {
            int userId = _userService.GetUserIdByUserName(User.Identity.Name);
            _courseService.AddVote(userId, id, vote);

            return PartialView("CourseVote", _courseService.GetCourseVote(id));
        }

    }
}
