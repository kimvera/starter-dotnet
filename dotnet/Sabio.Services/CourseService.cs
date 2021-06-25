using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Assessment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sabio.Services
{
    public class CourseService : ICourseService
    {
        IDataProvider _data = null;
        public CourseService(IDataProvider data)
        {
            _data = data;
        }

        #region Add Course
        public int Add(CourseAddRequest model)
        {
            int id = 0;

            string procName = "[dbo].[Assessment_Course_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection sqlCol)
            {
                sqlCol.AddWithValue("@Name", model.Name);
                sqlCol.AddWithValue("@CourseDesc", model.CourseDesc);
                sqlCol.AddWithValue("@SeasonTermId", model.SeasonTermId);
                sqlCol.AddWithValue("@TeacherId", model.TeacherId);

                SqlParameter courseIdOut = new SqlParameter("@Id", SqlDbType.Int);
                courseIdOut.Direction = ParameterDirection.Output;

                sqlCol.Add(courseIdOut);
            }
            , returnParameters: delegate (SqlParameterCollection returnIdCol)
            {
                object courseIdOut = returnIdCol["@Id"].Value;
                Int32.TryParse(courseIdOut.ToString(), out id);
            });
            return id;
        }
        #endregion

        #region Get By Id
        public Course Get(int id)
        {
            string procName = "[dbo].[Assessment_Course_SelectById]";

            Course course = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                course = CourseMapper(reader, ref index);
            });
            return course;
        }
        #endregion

        #region Update
        public void Update(CourseUpdateRequest model)
        {
            string procName = "[dbo].[Assessment_Course_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@Name", model.Name);
                col.AddWithValue("@CourseDesc", model.CourseDesc);
                col.AddWithValue("@SeasonTermId", model.SeasonTermId);
                col.AddWithValue("@TeacherId", model.TeacherId);

            }
            , returnParameters: null);
        }
        #endregion

        public void Delete(int id)
        {
            string procName = "[dbo].[Assessment_Student_Delete]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection delCol)
            {

                delCol.AddWithValue("@Id", id);

            }, returnParameters: null);
        }

        #region Pagination
        public Paged<Course> GetPage(int pageIndex, int pageSize)
        {
            Paged<Course> pagedList = null;
            List<Course> list = null;
            int totalCount = 0;

            string procName = "[dbo].[Assessment_Course_SelectPaginated]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Course course = CourseMapper(reader, ref index);
                totalCount = reader.GetSafeInt32(index);

                if (list == null)
                {
                    list = new List<Course>();
                }
                list.Add(course);
            });
            if (list != null)
            {
                pagedList = new Paged<Course>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        } 
        #endregion


        private static Course CourseMapper(IDataReader reader, ref int startingIdx)
        {
            Course course = new Course();

            course.Id = reader.GetSafeInt32(startingIdx++);
            course.Name = reader.GetSafeString(startingIdx++);
            course.CourseDesc = reader.GetSafeString(startingIdx++);
            course.SeasonTerm = reader.GetSafeString(startingIdx++);
            course.Teacher = reader.GetSafeString(startingIdx++);
            course.Students = reader.DeserializeObject<List<Student>>(startingIdx++);

            return course;
        }


    }
}
