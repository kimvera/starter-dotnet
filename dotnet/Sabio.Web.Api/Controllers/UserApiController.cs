using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sabio.Models.Requests.Users;
using Sabio.Models;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : BaseApiController
    {
        private IUsersService _service = null;
        private IAuthenticationService<int> _authService = null;

        public UserApiController(IUsersService service, ILogger<UserApiController> logger, IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet]
        public ActionResult<ItemsResponse<User>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<User> list = _service.GetTop();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application Resource Not Found");
                }
                else
                {
                    response = new ItemsResponse<User> { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("{id:int}")]

        public ActionResult<ItemResponse<User>> Get(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                User user = _service.Get(id);
                if (user == null)
                {
                    code = 404;
                    response = new ErrorResponse("Not Found");
                }
                else
                {
                    response = new ItemResponse<User>() { Item = user };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
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
        public ActionResult<ItemResponse<int>> Create(UserAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(UserUpdateRequest model)
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

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<User>>> Pagination(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<User> user = _service.GetPage(pageIndex, pageSize);

                if (user == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<User>> { Item = user };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<User>>> Search(string query, int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<User> user = _service.Search(query, pageIndex, pageSize);

                if (user == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<User>> { Item = user };
                }
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
