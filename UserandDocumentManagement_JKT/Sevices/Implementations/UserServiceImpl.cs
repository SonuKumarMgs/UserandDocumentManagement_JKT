using Microsoft.EntityFrameworkCore;
using UserandDocumentManagement_JKT.Data;
using UserandDocumentManagement_JKT.DTOs;
using UserandDocumentManagement_JKT.Models;
using UserandDocumentManagement_JKT.Sevices.Interfaces;

namespace UserandDocumentManagement_JKT.Sevices.Implementations
{
    public class UserServiceImpl : IUserService
    {
        private readonly AppDbCotext _context;

        public UserServiceImpl(AppDbCotext appDbCotext)
        {
            _context = appDbCotext;
        }
        public async Task<List<UsersProfile>> GetAllUsersAsync()
        {
            return await _context.Users
                            .Select(u => new UsersProfile
                            {
                                Id = u.Id,
                                Username = u.Username,
                                Email = u.Email,
                                Role = u.Role
                            }).Where(a=>a.Role!="Admin")
                            .ToListAsync();
        }

        public async Task<UsersProfile> GetUsersByUserIdAsync(Guid userId)
        {
              var user = await _context.Users.Where(a => a.Id == userId && a.Role != "Admin")
                      .Select(u => new UsersProfile
                      {
                          Id = u.Id,
                          Username = u.Username,
                          Role = u.Role,
                          Email=u.Email
                      }).FirstOrDefaultAsync();

            if (user == null)
                throw new KeyNotFoundException("User not found.");
            return user;
        }

        public async Task<string> UpdateUserRoleByIdAsync(Guid userId, string newRole)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            user.Role = newRole;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return "Updated data sucessfully..";
        }
    }
}
