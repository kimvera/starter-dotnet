using Sabio.Data.Providers;
using Sabio.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Sabio.Data;
using Sabio.Models.Requests.Users;
using Sabio.Models;

namespace Sabio.Services
{
    public class UsersService : IUsersService
    {
        IDataProvider _data = null;
        public UsersService(IDataProvider data)
        {
            _data = data;
        }

        #region Get by Id
        public User Get(int id)
        {
            string procName = "[dbo].[Users_SelectById]";

            User user = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@Id", id);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                user = UserMapper(reader);

            });
            return user;
        }

        #endregion

        #region Get Top
        public List<User> GetTop()
        {
            List<User> listOfUsers = null;

            string procName = "[dbo].[Users_SelectAll]";

            User user = null;

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                user = UserMapper(reader);

                if (listOfUsers == null)
                {
                    listOfUsers = new List<User>();
                }
                listOfUsers.Add(user);
            });

            return listOfUsers;
        }
        #endregion

        public int Add(UserAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[Users_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@FirstName", model.FirstName);
                col.AddWithValue("@LastName", model.LastName);
                col.AddWithValue("@Email", model.Email);
                col.AddWithValue("@Password", model.Password);
                col.AddWithValue("@PasswordConfirm", model.PasswordConfirm);
                col.AddWithValue("@AvatarUrl", model.AvatarUrl);
                col.AddWithValue("@UserId", model.UserId);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCol)
            {
                object outputId = returnCol["@Id"].Value;
                Int32.TryParse(outputId.ToString(), out id);
            });
            return id;
        }

        public void Update(UserUpdateRequest model)
        {
            string procName = "[dbo].[Users_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection updatedCol)
            {
                updatedCol.AddWithValue("@FirstName", model.FirstName);
                updatedCol.AddWithValue("@LastName", model.LastName);
                updatedCol.AddWithValue("@Email", model.Email);
                updatedCol.AddWithValue("@Password", model.Password);
                updatedCol.AddWithValue("@PasswordConfirm", model.PasswordConfirm);
                updatedCol.AddWithValue("@AvatarUrl", model.AvatarUrl);
                updatedCol.AddWithValue("@UserId", model.UserId);
                updatedCol.AddWithValue("@Id", model.Id);
            }, returnParameters: null);

        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Users_Delete]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection deleteCol)
            {
                deleteCol.AddWithValue("@Id", id);
            }, returnParameters: null);

        }
        public Paged<User> GetPage(int pageIndex, int pageSize)
        {
            Paged<User> pagedList = null;
            List<User> list = null;
            int totalCount = 0;

            string procName = "[dbo].[Users_SelectedPaginated]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                User user = UserMapper(reader);
                totalCount = reader.GetSafeInt32(set);

                if (list == null)
                {
                    list = new List<User>();
                }
                list.Add(user);
            });
            if (list != null)
            {
                pagedList = new Paged<User>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<User> Search(string query, int pageIndex, int pageSize)
        {
            Paged<User> pagedList = null;
            List<User> list = null;
            int totalCount = 0;

            string procName = "[dbo].[Users_Search]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@Query", query);
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                User user = UserMapper(reader);
                totalCount = reader.GetSafeInt32(set);

                if (list == null)
                {
                    list = new List<User>();
                }
                list.Add(user);
            });
            if (list != null)
            {
                pagedList = new Paged<User>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        private static User UserMapper(IDataReader reader)
        {
            User user = new User();
            int startingIndex = 0;

            user.Id = reader.GetSafeInt32(startingIndex++);
            user.UserId = reader.GetSafeString(startingIndex++);
            user.FirstName = reader.GetSafeString(startingIndex++);
            user.LastName = reader.GetSafeString(startingIndex++);
            user.Email = reader.GetSafeString(startingIndex++);
            user.Password = reader.GetSafeString(startingIndex++);
            user.PasswordConfirm = reader.GetSafeString(startingIndex++);
            user.AvatarUrl = reader.GetSafeString(startingIndex++);

            return user;
        }
    }
}
