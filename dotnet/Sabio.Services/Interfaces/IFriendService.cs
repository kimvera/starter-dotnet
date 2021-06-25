using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Friends;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface IFriendService
    {
        int Add(FriendAddRequest model, int userId);
        void Delete(int id);
        Friend Get(int id);
        List<Friend> GetTop();
        void Update(FriendUpdateRequest model);

        public Paged<Friend> GetPage(int pageIndex, int pageSize);

        public Paged<Friend> Search(string query, int pageIndex, int pageSize);

    }
}