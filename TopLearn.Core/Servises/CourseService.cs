using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs.Course;
using TopLearn.Core.Generators;
using TopLearn.Core.Security;
using TopLearn.Core.Servises.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Cource;

namespace TopLearn.Core.Servises
{
    public class CourseService : ICourseService
    {
        private TopLearnContext _context;

        public CourseService(TopLearnContext context)
        {
            _context = context;
        }



        #region SAVE OR UPDATE    

        public string SaveOrUpdateCourseImg(IFormFile imgCourse, string CourseimgName = "no-photo.jpg",
            string SavePath = "wwwroot/Course/image", string thumbSavePath = "wwwroot/Course/thumb")
        {
            if (imgCourse != null && imgCourse.IsImage())
            {
                string imagePath = "";
                string thumbPath = "";
                if (CourseimgName != "no-photo.jpg")
                {
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), SavePath, CourseimgName);
                    if (File.Exists(imagePath))
                        File.Delete(imagePath);
                    thumbPath = Path.Combine(Directory.GetCurrentDirectory(), thumbSavePath, CourseimgName);
                    if (File.Exists(thumbPath))
                        File.Delete(thumbPath);
                }
                CourseimgName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgCourse.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), SavePath, CourseimgName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }

                //RESIZE IMAGE
                #region RESIZE IMAGE
                ImageConvertor imgResize = new ImageConvertor();
                thumbPath = Path.Combine(Directory.GetCurrentDirectory(), thumbSavePath, CourseimgName);

                imgResize.Image_resize(imagePath, thumbPath, 150);

                #endregion


                return CourseimgName;
            }
            else if (CourseimgName != "no-photo.jpg")
            {
                return CourseimgName;
            }
            else
            {
                return "no-photo.jpg";
            }
        }


        public string SaveDemo(IFormFile demoCourse, string CourseDemoName = null, string SavePath = "wwwroot/Course/demoes")
        {
            if (demoCourse != null)
            {
                string demoPath = "";
                if (CourseDemoName != null)
                {
                    demoPath = Path.Combine(Directory.GetCurrentDirectory(), SavePath, CourseDemoName);
                    if (File.Exists(demoPath))
                        File.Delete(demoPath);
                }
                CourseDemoName = NameGenerator.GenerateUniqCode() + Path.GetExtension(demoCourse.FileName);
                demoPath = Path.Combine(Directory.GetCurrentDirectory(), SavePath, CourseDemoName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    demoCourse.CopyTo(stream);
                }

                return CourseDemoName;
            }
            else
            {
                return null;
            }
        }

        #endregion


        #region GROUP

        public List<CourseGroup> GetAllGroup()
        {
            return _context.CourseGroup
                .Include(c=>c.CourseGroups)
                .ToList();
        }

        public List<SelectListItem> GetGroupForManageCourse()
        {
            return _context.CourseGroup
                .Where(g => g.ParentId == null)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                })
                .ToList();
        }

        public List<SelectListItem> GetSubGroupForManageCourse(int groupId)
        {
            return _context.CourseGroup
                .Where(g => g.ParentId == groupId)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                })
                .ToList();
        }

        public List<SelectListItem> GetTeachers()
        {
            return _context.UserRoles
                .Where(r => r.RoleId == 2)
                .Include(r => r.User)
                .Select(u => new SelectListItem()
                {
                    Value = u.UsreId.ToString(),
                    Text = u.User.UserName
                })
                .ToList();
        }

        public List<SelectListItem> GetLevels()
        {
            return _context.CourseLevels
               .Select(u => new SelectListItem()
               {
                   Value = u.LevelId.ToString(),
                   Text = u.LevelTitle
               })
               .ToList();
        }

        public List<SelectListItem> GetStatues()
        {
            return _context.CourseStatuses
               .Select(u => new SelectListItem()
               {
                   Value = u.StatusId.ToString(),
                   Text = u.StatusTitle
               })
               .ToList();
        }


        public CourseGroup GetGroupById(int groupId)
        {
            return _context.CourseGroup.Find(groupId);
        }



        public void AddGroup(CourseGroup group)
        {
            _context.CourseGroup.Add(group);
            _context.SaveChanges();
        }


        public void UpdateGroup(CourseGroup group)
        {
            _context.CourseGroup.Update(group);
            _context.SaveChanges();
        }

        #endregion



        #region COURSE

        public List<ShowCourseForAdminViewModel> GetCourseForAdmin()
        {
            return _context.Courses
                .Select(c => new ShowCourseForAdminViewModel()
                {
                    CourseId = c.CourseId,
                    ImageName = c.CourseImageName,
                    Title = c.CourseTitle,
                    EpisodeCount = c.CourseEpisodes.Count()
                })
                .ToList();
        }

        public int AddCourse(Course course, IFormFile imgCourse, IFormFile courseDemo)
        {
            course.CreateDate = DateTime.Now;
            course.CourseImageName = SaveOrUpdateCourseImg(imgCourse);
            course.DemoFileName = SaveDemo(courseDemo);

            if (course.SubGroup == 0)
            {
                course.SubGroup = 12;
            }
            if (course.CoursePrice == null)
            {
                course.CoursePrice = 0;
            }


            _context.Courses.Add(course);
            _context.SaveChanges();


            return course.CourseId;
        }

        public Course GetCourseById(int courseId)
        {
            return _context.Courses.Find(courseId);
        }

        public void UpdateCourse(Course course, IFormFile imgCourse, IFormFile courseDemo)
        {
            course.UpdateDate = DateTime.Now;
            course.CourseImageName = SaveOrUpdateCourseImg(imgCourse, course.CourseImageName);
            course.DemoFileName = SaveDemo(courseDemo, course.DemoFileName);

            if (course.SubGroup == 0)
            {
                course.SubGroup = 12;
            }
            if (course.CoursePrice == null)
            {
                course.CoursePrice = 0;
            }


            _context.Courses.Update(course);
            _context.SaveChanges();

        }

        public Tuple<List<ShowCourseListItemViewModel>, int> GetCourse(int pageId = 1, string filter = "", string getType = "all",
            string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> SelectedGroups = null, int take = 0)
        {
            if (take == 0)
                take = 8;

            IQueryable<Course> result = _context.Courses;
            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(c => c.CourseTitle.Contains(filter) || c.Tags.Contains(filter));
            }
            switch (getType)
            {
                case "all":
                    break;
                case "buy":
                    {
                        result = result.Where(c => c.CoursePrice != 0);
                        break;
                    }
                case "free":
                    {
                        result = result.Where(c => c.CoursePrice == 0);
                        break;
                    }
            }

            switch (orderByType)
            {
                case "date":
                    {
                        result = result.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                case "updatedate":
                    {
                        result = result.OrderByDescending(c => c.UpdateDate);
                        break;
                    }
            }

            if (startPrice > 0)
            {
                result = result.Where(c => c.CoursePrice > startPrice);
            }
            if (endPrice > 0)
            {
                result = result.Where(c => c.CoursePrice < endPrice);
            }

            if (SelectedGroups != null && SelectedGroups.Any())
            {
                foreach (int groupId in SelectedGroups)
                {
                    result = result.Where(c => c.GroupId == groupId || c.SubGroup == groupId);
                }
            }


            int skip = (pageId - 1) * take;

            int pageCount = result
                .Include(c => c.CourseEpisodes)
                .Select(c => new ShowCourseListItemViewModel()
                {
                    CourseId = c.CourseId,
                    ImageName = c.CourseImageName,
                    Price = c.CoursePrice,
                    Title = c.CourseTitle,
                   
                }).Count() / take;


            var query = result
                .Include(c => c.CourseEpisodes)
                .Select(c => new ShowCourseListItemViewModel()
                {
                    CourseId = c.CourseId,
                    ImageName = c.CourseImageName,
                    Price = c.CoursePrice,
                    Title = c.CourseTitle,
                    CourseEpisodes = c.CourseEpisodes
                })
                .Skip(skip).Take(take)
                .ToList();


            return Tuple.Create(query, pageCount);

        }

        public Course GetCourseForShow(int CourseId)
        {
            return _context.Courses
                .Include(c => c.CourseEpisodes)
                .Include(c => c.CourseStatus)
                .Include(c => c.CourseLevel)
                .Include(c => c.User)
                .Include(c => c.UserCourses)
                .FirstOrDefault(c => c.CourseId == CourseId);

        }

        public List<ShowCourseListItemViewModel> GetPopularCourse()
        {
            return _context.Courses
                .Include(c => c.OrderDetails)
                .Include(c=>c.CourseEpisodes)
                .Where(c=>c.OrderDetails.Any())
                .OrderByDescending(d => d.OrderDetails.Count)
                .Select(c=>new ShowCourseListItemViewModel()
                {
                    CourseId = c.CourseId,
                    ImageName = c.CourseImageName,
                    Price = c.CoursePrice,
                    Title = c.CourseTitle,
                    CourseEpisodes = c.CourseEpisodes   
                })
                .Take(8)
                .ToList();
        }


        public bool IsFree(int courseId)
        {
            return _context.Courses.Find(courseId).CoursePrice == 0;
        }



        #endregion



        #region Episode

        public List<CourseEpisode> GetListEpisodeCorse(int courseId)
        {
            return _context.CourseEpisodes
                .Where(e => e.CourseId == courseId)
                .ToList();
        }

        public bool CheckExistFile(string fileName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFiles", fileName);
            return File.Exists(path);
        }

        public int AddEpisode(CourseEpisode episode, IFormFile episodeFile)
        {
            episode.EpisodFileName = episodeFile.FileName;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFiles", episode.EpisodFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                episodeFile.CopyTo(stream);
            }

            _context.CourseEpisodes.Add(episode);
            _context.SaveChanges();

            return episode.EpisodeId;
        }

        public CourseEpisode GetEpisodeById(int episodeId)
        {
            return _context.CourseEpisodes.Find(episodeId);
        }

        public void EditEpisode(CourseEpisode episode, IFormFile episodeFile)
        {
            if (episodeFile != null)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFiles", episode.EpisodFileName);
                if (File.Exists(filePath)) File.Delete(filePath);

                episode.EpisodFileName = episodeFile.FileName;
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFiles", episode.EpisodFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    episodeFile.CopyTo(stream);
                }
            }
            _context.CourseEpisodes.Update(episode);
            _context.SaveChanges();
        }

        #endregion


        #region Comment

        public void AddComment(CourseComment comment)
        {
            _context.CourseComments.Add(comment);
            _context.SaveChanges();
        }

        public Tuple<List<CourseComment>, int> GetCourseComments(int courseId, int pageId = 1)
        {
            int take = 5;
            int skip = (pageId - 1) * take;
            int pageCount = _context.CourseComments.Where(c => !c.IsDelete && c.CourseId == courseId).Count() / take;

            if ((pageCount % 2) != 0)
            {
                pageCount += 1;
            }

            return Tuple.Create(
                _context.CourseComments
                .Include(c => c.User)
                .Where(c => !c.IsDelete && c.CourseId == courseId)
                .Skip(skip).Take(take)
                .OrderByDescending(c => c.CreateDate)
                .ToList(),
                pageCount);
        }

        #endregion

        

        public void AddVote(int userId, int courseId, bool vote)
        {
            var userVote = _context.CourseVotes.FirstOrDefault(c => c.UserId == userId && c.CourseId == courseId);

            if (userVote != null)
            {
                userVote.Vote = vote;
                _context.CourseVotes.Update(userVote);
            }
            else
            {
                userVote = new CourseVote()
                {
                    CourseId = courseId,
                    UserId = userId,
                    Vote = vote
                };
                _context.CourseVotes.Add(userVote);
            }
            _context.SaveChanges();
        }

        public Tuple<int, int> GetCourseVote(int courseId)
        {
            
            var votes = _context.CourseVotes
                .Where(c => c.CourseId == courseId)
                .Select(c=>c.Vote)
                .ToList();

            return Tuple.Create(votes.Count(c => c), votes.Count(c => !c));
        }
    }
}
