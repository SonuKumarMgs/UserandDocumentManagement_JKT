using UserandDocumentManagement_JKT.DTOs;
using UserandDocumentManagement_JKT.Models;

namespace UserandDocumentManagement_JKT.Sevices.Interfaces
{
    public interface IUserService
    {
        Task<List<UsersProfile>> GetAllUsersAsync();
        Task<UsersProfile> GetUsersByUserIdAsync(Guid userId);
        Task<string> UpdateUserRoleByIdAsync(Guid userId, string newRole);
    }
}
