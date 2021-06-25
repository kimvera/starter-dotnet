using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Assessment;

namespace Sabio.Services
{
    public interface ICourseService
    {
        int Add(CourseAddRequest model);

        Course Get(int id);

        void Update(CourseUpdateRequest model);

        void Delete(int id);

        public Paged<Course> GetPage(int pageIndex, int pageSize);


    }
}