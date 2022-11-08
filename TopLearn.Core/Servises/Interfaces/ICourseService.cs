using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs.Course;
using TopLearn.DataLayer.Entities.Cource;

namespace TopLearn.Core.Servises.Interfaces
{
    public interface ICourseService
    {
        #region Save Image

        string SaveOrUpdateCourseImg(IFormFile UserAvatar, string CourseimgName = "no-photo.jpg",
            string SavePath="wwwroot/Course/image", string thumbSavePath = "wwwroot/Course/thumb");
        string SaveDemo(IFormFile demoCourse, string CourseDemoName = null, string SavePath= "wwwroot/Course/demoes");


        #endregion


        #region Group

        List<CourseGroup> GetAllGroup();
        List<SelectListItem> GetGroupForManageCourse();
        List<SelectListItem> GetSubGroupForManageCourse(int groupId);
        List<SelectListItem> GetTeachers();
        List<SelectListItem> GetLevels();
        List<SelectListItem> GetStatues();
        CourseGroup GetGroupById(int groupId);
        void AddGroup(CourseGroup group);
        void UpdateGroup(CourseGroup group);


        #endregion


        #region Course

        List<ShowCourseForAdminViewModel> GetCourseForAdmin();
        int AddCourse(Course course, IFormFile imgCourse, IFormFile courseDemo);
        Course GetCourseById(int courseId);
        void UpdateCourse(Course course, IFormFile imgCourse, IFormFile courseDemo);
         Tuple<List<ShowCourseListItemViewModel>,int> GetCourse(int pageId=1,string filter="",string getType="all",
            string orderByType="date",int startPrice=0,int endPrice=0,List<int> SelectedGroups=null, int take=0);
        Course GetCourseForShow(int CourseId);
        List<ShowCourseListItemViewModel> GetPopularCourse();
        bool IsFree(int courseId);

        #endregion


        #region Episode

        List<CourseEpisode> GetListEpisodeCorse(int courseId);
        bool CheckExistFile(string fileName);
        int AddEpisode(CourseEpisode episode,IFormFile episodeFile);
        CourseEpisode GetEpisodeById(int episodeId);
        void EditEpisode(CourseEpisode episode, IFormFile episodeFile);

        #endregion


        #region Commetns

        void AddComment(CourseComment comment);
        Tuple<List<CourseComment>, int> GetCourseComments(int courseId, int pageId = 1);

        #endregion


        #region Corse Vote

        void AddVote(int userId, int courseId, bool vote);
        Tuple<int, int> GetCourseVote(int courseId);

        #endregion
    }
}
