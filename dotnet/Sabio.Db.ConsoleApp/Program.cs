using Sabio.Data;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Addresses;
using Sabio.Models.Requests.Users;
using Sabio.Models.Requests.Friends;
using Sabio.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace Sabio.Db.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Here are two example connection strings. Please check with the wiki and video courses to help you pick an option

            //string connString = @"Data Source=ServerName_Or_IpAddress;Initial Catalog=DB_Name;User ID=SabioUser;Password=Sabiopass1!";
            string connString = @"Data Source=104.42.194.102;Initial Catalog=C104_verakim94_gmail;User ID=C104_verakim94_gmail_User;Password=C104_verakim94_gmail_User7DB99721";

            TestConnection(connString);

            SqlDataProvider provider = new SqlDataProvider(connString);

            #region - Address Service (OK)

            AddressesService addressService = new AddressesService(provider);

            Address address1 = addressService.Get(9);

            List<Address> listOfAddresses = addressService.GetTop();
            #endregion

            #region Address - Add/Update (OK)

            //insert model

            AddressAddRequest request = new AddressAddRequest();

            request.LineOne = "100 Culver Blvd";
            request.SuiteNumber = 300;
            request.City = "Culver City";
            request.State = "CA";
            request.PostalCode = "90010";
            request.IsActive = true;
            request.Lat = 28374;
            request.Long = 12884772;

            int newId = addressService.Add(request, );


            // update model 

            AddressUpdateRequest updateRequest = new AddressUpdateRequest();

            updateRequest.LineOne = "200 Culver Blvd";
            updateRequest.SuiteNumber = 0;
            updateRequest.City = "Culver City";
            updateRequest.State = "CA";
            updateRequest.PostalCode = "90010";
            updateRequest.IsActive = true;
            updateRequest.Lat = 28374;
            updateRequest.Long = 12884772;
            updateRequest.Id = newId;

            addressService.Update(updateRequest); 

            Address updatedAdd = addressService.Get(newId);
            #endregion

            #region Address - Delete (OK)
      
            addressService.Delete(2); 
            #endregion

            #region User Service (OK)
            UsersService userService = new UsersService(provider);
            User newUser = userService.Get(3);

            List<User> listOfUsers = userService.GetTop();
            #endregion

            #region User Add Request (OK)

            UserAddRequest addRequest = new UserAddRequest();

            addRequest.FirstName = "Algo";
            addRequest.LastName = "Grokking";
            addRequest.Email = "algogrokking@fake.com";
            addRequest.Password = "Password!1";
            addRequest.PasswordConfirm = "Password!1";
            addRequest.AvatarUrl = "https://i.pravatar.cc/150?img=58";
            addRequest.UserId = "algo123";

            int newUserId = userService.Add(addRequest);
            #endregion

            #region User Update Request (OK)
            UserUpdateRequest updateUser = new UserUpdateRequest();

            updateUser.FirstName = "Algori";
            updateUser.LastName = "Grokking";
            updateUser.Email = "algorigrokking@fake.com";
            updateUser.Password = "Password!1";
            updateUser.PasswordConfirm = "Password!1";
            updateUser.AvatarUrl = "https://i.pravatar.cc/150?img=100";
            updateUser.UserId = "algori123";
            updateUser.Id = newUserId;

            userService.Update(updateUser);

            User updatedUser = userService.Get(newUserId);
            #endregion


            #region User Delete (OK)
            userService.Delete(20); 
            #endregion


            #region Friend Services (OK)
            FriendService friendService = new FriendService(provider);

            Friend aFriend = friendService.Get(4);

            List<Friend> friends = friendService.GetTop();
            #endregion

            #region Friend Add (OK)

            FriendAddRequest addFriend = new FriendAddRequest();

            addFriend.Title = "Brian Yang";
            addFriend.Bio = "Brian is a software engineer at a tech co";
            addFriend.Summary = "Software Engineer";
            addFriend.Headline = "100 Irvine Spectrum";
            addFriend.Slug = "brianyang@fake.com";
            addFriend.PrimaryImageId = 11;
            addFriend.UserId = "byang";

            int newFriendId = friendService.Add(addFriend);
            #endregion

            #region Friend Update (OK)
            FriendUpdateRequest updateFriend = new FriendUpdateRequest();

            updateFriend.Title = "Kang Min Yang";
            updateFriend.Bio = "Kang Min Yang is a software engineer at a tech co";
            updateFriend.Summary = "Software Engineer";
            updateFriend.Headline = "100 Irvine Spectrum";
            updateFriend.Slug = "bky@fake.com";
            updateFriend.PrimaryImageId = 11;
            updateFriend.UserId = "bkyang";
            updateFriend.Id = newFriendId;

            friendService.Update(updateFriend);
            ;
            Friend updatedFriend = friendService.Get(newFriendId);

            #endregion

            #region Friend Delete (OK)
            friendService.Delete(30); 
            #endregion



            Console.ReadLine();//This waits for you to hit the enter key before closing window
        }

        private static void TestConnection(string connString)
        {
            bool isConnected = IsServerConnected(connString);
            Console.WriteLine("DB isConnected = {0}", isConnected);
        }

        private static bool IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
    }
}
