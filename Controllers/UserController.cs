using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Constants.Enums;
using WebApplication1.Constants.Strings;
using WebApplication1.Dtos;
using WebApplication1.Entities;
using WebApplication1.Interfaces.Services;
using WebApplication1.Utils.HttpExceptionResponse;
using WebApplication1.Utils.HttpResponse;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]s")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public ActionResult<HttpResponse<UserEntity>> GetOne(Guid id)
        {
            var user = _userService.GetById(
                id,
                projection: (user) => new UserEntity() { Id = user.Id, Username = user.Username }
            );

            return user == null
                ? throw new HttpExceptionResponse(HttpExceptionTypes.NotFound, HttpExceptionMessages.NOT_FOUND)
                : SuccessResponse(user);
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public ActionResult<HttpResponse<List<UserEntity>>> GetMany([FromQuery] FilterByKeywordDto query)
        {
            var users = _userService.GetWithKeyword(
                query.Keyword,
                query.Page,
                query.PageSize,
                (user) => new UserEntity() { Id = user.Id, Username = user.Username }
            );

            return SuccessResponse(users.Count, users);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<HttpResponse<Guid>>> Update(Guid id, [FromBody] UserUpdateDto dataToUpdate)
        {
            var result = await _userService.Update(id, dataToUpdate)
                ?? throw new HttpExceptionResponse(HttpExceptionTypes.BadRequest, "Id not found");

            return SuccessResponse(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<HttpResponse<Guid>>> Remove(Guid id)
        {
            var result = await _userService.Remove(id)
                ?? throw new HttpExceptionResponse(HttpExceptionTypes.BadRequest, "Id not found");

            return SuccessResponse(result);
        }
    }
}

