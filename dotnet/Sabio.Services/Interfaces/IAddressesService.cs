using Sabio.Models.Domain;
using Sabio.Models.Requests.Addresses;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface IAddressesService
    {
        //these are method stubs 
        int Add(AddressAddRequest model, int currentUserId);
        Address Get(int id);
        List<Address> GetTop();
        void Update(AddressUpdateRequest model);
        void Delete(int id);
    }
}