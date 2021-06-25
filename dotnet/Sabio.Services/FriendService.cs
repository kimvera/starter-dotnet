using Sabio.Data.Providers;
using Sabio.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Sabio.Models.Requests.Friends;
using Sabio.Data;
using Sabio.Models;


namespace Sabio.Services
{
    public class FriendService : IFriendService
    {
        IDataProvider _data = null;
        public FriendService(IDataProvider data)
        {
            _data = data;
        }

        #region Get Friend 
        public Friend Get(int id)
        {
            string procName = "[dbo].[Friends_Join_SelectById]";

            Friend friend = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                friend = Friendmapper(reader, ref index);
            });
            return friend;
        }
        #endregion


        public List<Friend> GetTop()
        {
            List<Friend> friends = null;

            string procName = "[dbo].[Friends_SelectAll]";

            Friend friend = null;

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                friend = Friendmapper(reader, ref index );

                if (friends == null)
                {
                    friends = new List<Friend>();
                }
                friends.Add(friend);
            });
            return friends;


        }

        public int Add(FriendAddRequest model, int userId)
        { 
            int id = 0;
            
            string procName = "[dbo].[Friends_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection sqlCol)
            {
                sqlCol.AddWithValue("@Title", model.Title);
                sqlCol.AddWithValue("@Bio", model.Bio);
                sqlCol.AddWithValue("@Summary", model.Summary);
                sqlCol.AddWithValue("@Headline", model.Headline);
                sqlCol.AddWithValue("@Slug", model.Slug);
                sqlCol.AddWithValue("@ImageUrl", model.PrimaryImage);
                sqlCol.AddWithValue("@UserId", model.UserId);
                sqlCol.AddWithValue("@ImageTypeId", 1);

                SqlParameter friendIdOut = new SqlParameter("@Id", SqlDbType.Int);
                friendIdOut.Direction = ParameterDirection.Output;

                sqlCol.Add(friendIdOut);
            }
            , returnParameters: delegate (SqlParameterCollection returnIdCol)
            {
                object friendIdOut = returnIdCol["@Id"].Value;
                Int32.TryParse(friendIdOut.ToString(), out id);
            });
            return id;
        }

        public void Update(FriendUpdateRequest model)
        {
            string procName = "[dbo].[Friends_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Title", model.Title);
                col.AddWithValue("@Bio", model.Bio);
                col.AddWithValue("@Summary", model.Summary);
                col.AddWithValue("@Headline", model.Headline);
                col.AddWithValue("@Slug", model.Slug);
                col.AddWithValue("@PrimaryImageId", model.PrimaryImageId);
                col.AddWithValue("@ImageUrl", model.PrimaryImage);
                col.AddWithValue("@UserId", model.UserId);
                col.AddWithValue("@ImageTypeId", 1);
                col.AddWithValue("@Id", model.Id);
            }
            , returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Friends_Join_Delete]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection delCol)
            {

                delCol.AddWithValue("@Id", id);

            }, returnParameters: null);
        }

        public Paged<Friend> GetPage(int pageIndex, int pageSize)
        {
            Paged<Friend> pagedList = null;
            List<Friend> list = null;
            int totalCount = 0;

            string procName = "[dbo].[Friends_Join_SelectPaginated]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate(SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Friend friend = Friendmapper(reader, ref index);
                totalCount = reader.GetSafeInt32(index);

                if(list == null)
                {
                    list = new List<Friend>();
                }
                list.Add(friend);
            });
            if (list != null)
            {
                pagedList = new Paged<Friend>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }


        public Paged<Friend> Search(string query, int pageIndex, int pageSize)
        {
            Paged<Friend> pagedList = null;
            List<Friend> list = null;
            int totalCount = 0;

            string procName = "[dbo].[Friends_Join_Search]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@Query", query);
            }
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Friend friend = Friendmapper(reader, ref index);
                totalCount = reader.GetSafeInt32(index);

                if (list == null)
                {
                    list = new List<Friend>();
                }
                list.Add(friend);
            });

            if (list != null)
            {
                pagedList = new Paged<Friend>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        

        private static Friend Friendmapper(IDataReader reader, ref int startingIdx)
        {
            Friend friend = new Friend();
           
            friend.Id = reader.GetSafeInt32(startingIdx++);
            friend.Title = reader.GetSafeString(startingIdx++);
            friend.Bio = reader.GetSafeString(startingIdx++);
            friend.Summary = reader.GetSafeString(startingIdx++);
            friend.Headline = reader.GetSafeString(startingIdx++);
            friend.Slug = reader.GetSafeString(startingIdx++);
            friend.StatusId = reader.GetSafeString(startingIdx++);
            friend.PrimaryImage = new Image();
            friend.PrimaryImage.Id = reader.GetSafeInt32(startingIdx++);
            friend.PrimaryImage.ImageTypeId = reader.GetSafeInt32(startingIdx++);
            friend.PrimaryImage.ImageUrl = reader.GetSafeString(startingIdx++);
            friend.Skills = reader.DeserializeObject<List<Skill>>(startingIdx++);
            friend.UserId = reader.GetSafeString(startingIdx++);
            friend.DateAdded = reader.GetSafeUtcDateTime(startingIdx++);
            friend.DateModified = reader.GetSafeUtcDateTime(startingIdx++);

         
            return friend;
        }
    }
}
