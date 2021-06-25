using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Friends;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/friends")]
    [ApiController]
    public class FriendApiController : BaseApiController
    {
        private IFriendService _service = null;
        private IAuthenticationService<int> _authService = null;
        public FriendApiController(IFriendService service, ILogger<IFriendService> logger, IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
       

        [HttpGet]
        public ActionResult<ItemsResponse<Friend>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Friend> list = _service.GetTop();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Source Not Found");
                }
                else
                {
                    response = new ItemsResponse<Friend> { Items = list };
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
        public ActionResult<ItemResponse<Friend>> Get(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {

                Friend friend = _service.Get(id);
                if (friend == null)
                {
                    code = 404;
                    response = new ErrorResponse("Source Not Found");
                }
                else
                {
                    response = new ItemResponse<Friend>() { Item = friend };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();

            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(FriendAddRequest model)
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
        public ActionResult<SuccessResponse> Update(FriendUpdateRequest model)
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
        public ActionResult<ItemResponse<Paged<Friend>>> Pagination(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Friend> friend = _service.GetPage(pageIndex, pageSize);

                if (friend == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Friend>> { Item = friend };
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
        public ActionResult<ItemResponse<Paged<Friend>>> Search(string query,int pageIndex, int pageSize)
        {
            ActionResult result = null; 

            try
            {
                Paged<Friend> friend = _service.Search(query, pageIndex, pageSize);

                if (friend == null)
                {
                    result = NotFound404(new ErrorResponse("App Resource not found."));
                }
                else
                {
                    ItemResponse<Paged<Friend>> response = new ItemResponse<Paged<Friend>>();
                    response.Item = friend;
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }
            return result;
        }

    

    }
}


