using FlatFinder.Contracts.Services;
using FlatFinder.Web.ActionFilters;
using FlatFinder.Web.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlatFinder.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [CustomExceptionFilter]
    [ValidateModel]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(await _userService.GetAll());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.Remove(id);
            return Ok();
        }

        [HttpPost("change-password/{id}")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody]ChangePasswordRequest request)
        {
            await _userService.ChangePassword(id, request.Password);
            return Ok();
        }
    }
}