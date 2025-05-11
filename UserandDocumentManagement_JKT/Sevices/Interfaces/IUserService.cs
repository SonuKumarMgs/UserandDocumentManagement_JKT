using UserandDocumentManagement_JKT.DTOs;
using UserandDocumentManagement_JKT.Models;

namespace UserandDocumentManagement_JKT.Sevices.Interfaces
{
    public interface IUserService
    {
        Task<List<UsersProfile>> GetAllUsersAsync();
        Task<string> UpdateUserRoleByIdAsync(Guid userId, string newRole);
    }
}
