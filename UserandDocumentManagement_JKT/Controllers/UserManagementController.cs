using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserandDocumentManagement_JKT.Data;
using UserandDocumentManagement_JKT.DTOs;
using UserandDocumentManagement_JKT.Models;
using UserandDocumentManagement_JKT.Sevices.Interfaces;

namespace UserandDocumentManagement_JKT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AppDbCotext _context;

        public UserManagementController(IUserService userService, AppDbCotext context)
        {
            _userService = userService;
            _context = context;
        }
        
        [HttpGet("GetAllUsers")]        
        public async Task<ActionResult<UsersProfile>> GetAllUsersAsync()
        {          
            var data = await _userService.GetAllUsersAsync();
            return Ok(data);
        }
        
        [HttpGet("UpdateUserRoleById/{userId}/{newRole}")]
        public async Task<ActionResult<User>> UpdateUserRoleByIdAsync([FromRoute] Guid userId, [FromRoute] string newRole)
        {
            var data = await _userService.UpdateUserRoleByIdAsync(userId, newRole);
            return Ok(data);
        }

        [HttpGet("GetUsersByUserId/{userId}")]
        public async Task<ActionResult<UsersProfile>> GetUsersByUserIdAsync([FromRoute] Guid userId)
        {
            var data = await _userService.GetUsersByUserIdAsync(userId);
            return Ok(data);
        }
    }
}
