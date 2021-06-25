using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Assessment;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CourseApiController : BaseApiController
    {
        private ICourseService _service = null;
        public CourseApiController(ICourseService service, ILogger<ICourseService> logger) : base(logger)
        {
            _service = service;
           
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(CourseAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int id = _service.Add(model);

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

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Course>> Get(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {

                Course course = _service.Get(id);
                if (course == null)
                {
                    code = 404;
                    response = new ErrorResponse("Source Not Found");
                }
                else
                {
                    response = new ItemResponse<Course>() { Item = course };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(CourseUpdateRequest model)
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


        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Course>>> Pagination(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Course> course = _service.GetPage(pageIndex, pageSize);

                if (course == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Course>> { Item = course };
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
