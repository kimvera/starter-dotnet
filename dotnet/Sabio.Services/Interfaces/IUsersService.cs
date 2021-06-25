using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Users;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface IUsersService
    {
        int Add(UserAddRequest model, int currentUserId);
        void Delete(int id);
        User Get(int id);
        List<User> GetTop();
        void Update(UserUpdateRequest model);

        public Paged<User> GetPage(int pageIndex, int pageSize);

        public Paged<User> Search(string query, int pageIndex, int pageSize);
    }
}