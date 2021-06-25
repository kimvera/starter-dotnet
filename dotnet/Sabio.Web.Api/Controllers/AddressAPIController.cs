using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Addresses;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressAPIController : BaseApiController
    {
        private IAddressesService _service = null;
        private IAuthenticationService<int> _authService = null;

        // addressApiController depends on the AddressService so addressService needs to be injected into constructor of this class .. 
        public AddressAPIController(IAddressesService service
            , ILogger<AddressAPIController> logger
            , IAuthenticationService<int> authService):base(logger) // .net version of super(props) - base
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet] // same as  api/addresses/"" - this is our route 
        public ActionResult < ItemsResponse<Address> >GetAll()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Address> list = _service.GetTop();
                //ItemsResponse<Address> response = new ItemsResponse<Address>();
                //response.Items = list;

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application Resource Not Found");
                }
                else
                {
                    response = new ItemsResponse<Address> { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("{id:int}")] // api/addresses/{id:int} 
        public ActionResult<ItemResponse<Address>> Get(int id)
        {
            int iCode = 200; // setting default code
            BaseResponse response = null;

            try
            {
                // places information into type Address called address 
                Address address = _service.Get(id);

               // ItemResponse<Address> response = new ItemResponse<Address>();
               // response.Item = address;

                if (address == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Widget Not Found");
                   // return NotFound404(response);
                }
                else
                {
                    response = new ItemResponse<Address>()
                    {
                        Item = address
                    };
                  //  return Ok(response);
                }
            }
            catch (Exception ex)
            {
                iCode = 500;

                base.Logger.LogError(ex.ToString());

                response = new ErrorResponse($"Generic Error: {ex.Message}");

                // return base.StatusCode(500, new ErrorResponse($"Generic Error: {ex.Message}"));
            }
           
            return StatusCode(iCode, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);

                 response = new SuccessResponse();

                return Ok(response);
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
            
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(AddressAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                //id of new address 
                int id = _service.Add(model, userId);

                ItemResponse<int> response = new ItemResponse<int>();

                response.Item = id;

                return Ok(response);
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
            }

            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(AddressUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Update(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
    }
}
