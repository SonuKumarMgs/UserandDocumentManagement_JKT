using UserandDocumentManagement_JKT.DTOs;
using UserandDocumentManagement_JKT.Models;

namespace UserandDocumentManagement_JKT.Sevices.Interfaces
{
    public interface ISignupService
    {
        Task<string> UserSingup(UserDto userdto);
    }
}
