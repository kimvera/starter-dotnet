using Sabio.Data.Providers;
using Sabio.Data;
using Sabio.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Sabio.Models.Requests.Addresses;

namespace Sabio.Services
{ // not different from address class object but will be referred to as a service because of its intention/purpose
    public class AddressesService : IAddressesService // this means that addressService IMPLEMENTS IAddressService 
    {
        IDataProvider _data = null;
        // constructor is a method that doesn't need a return type and is named after what is encapsulating it, used to create an instance of this class
        public AddressesService(IDataProvider data)
        {
            _data = data;
        }


        public void Delete(int id)
        {
            string procName = "[dbo].[Sabio_Addresses_DeleteById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection delCol)
                {
                    delCol.AddWithValue("@Id", id);
                },
                returnParameters: null);
        }

        public void Update(AddressUpdateRequest model)
        {
            string procName = "[dbo].[Sabio_Addresses_Update]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection requestCol)
                {
                    requestCol.AddWithValue("@LineOne", model.LineOne);
                    requestCol.AddWithValue("@SuiteNumber", model.SuiteNumber);
                    requestCol.AddWithValue("@City", model.City);
                    requestCol.AddWithValue("@State", model.State);
                    requestCol.AddWithValue("@PostalCode", model.PostalCode);
                    requestCol.AddWithValue("@IsActive", model.IsActive);
                    requestCol.AddWithValue("@Lat", model.Lat);
                    requestCol.AddWithValue("@Long", model.Long);
                    requestCol.AddWithValue("@Id", model.Id);

                },
                returnParameters: null);
        }

        // this method returns an int because we're returning an Id 
        public int Add(AddressAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Sabio_Addresses_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@LineOne", model.LineOne);
                    col.AddWithValue("@SuiteNumber", model.SuiteNumber);
                    col.AddWithValue("@City", model.City);
                    col.AddWithValue("@State", model.State);
                    col.AddWithValue("@PostalCode", model.PostalCode);
                    col.AddWithValue("@IsActive", model.IsActive);
                    col.AddWithValue("@Lat", model.Lat);
                    col.AddWithValue("@Long", model.Long);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);
                },
                returnParameters: delegate (SqlParameterCollection returnCol)
                {
                    object outputId = returnCol["@Id"].Value;
                    Int32.TryParse(outputId.ToString(), out id);
                });
            return id;
        }

        public Address Get(int id)
        {
            string procName = "[dbo].[Sabio_Addresses_SelectById]";

            Address address = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set) // single record mapper 
            {
                address = MapAddress(reader);
            }
           );

            return address;
        }


        // this is how to get many - to get list of address
        public List<Address> GetTop()
        {
            List<Address> listOfAddresses = null;

            string procName = "[dbo].[Sabio_Addresses_SelectRandom50]";

            Address address = null;

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                address = MapAddress(reader);

                //listOfAddresses = new List<Address>();
                if (listOfAddresses == null)
                {
                    listOfAddresses = new List<Address>();
                }

                listOfAddresses.Add(address);
            });
            return listOfAddresses;
        }

        private static Address MapAddress(IDataReader reader)
        {
            Address address = new Address();
            int startingIndex = 0;

            address.Id = reader.GetSafeInt32(startingIndex++);
            address.LineOne = reader.GetSafeString(startingIndex++);
            address.SuiteNumber = reader.GetSafeInt32(startingIndex++);
            address.City = reader.GetSafeString(startingIndex++);
            address.State = reader.GetSafeString(startingIndex++);
            address.PostalCode = reader.GetSafeString(startingIndex++);
            address.IsActive = reader.GetSafeBool(startingIndex++);
            address.Lat = reader.GetSafeDouble(startingIndex++);
            address.Long = reader.GetSafeDouble(startingIndex++);
            return address;
        }
    }
}
