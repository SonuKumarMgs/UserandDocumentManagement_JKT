using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using UserandDocumentManagement_JKT.Data;
using UserandDocumentManagement_JKT.DTOs;
using UserandDocumentManagement_JKT.Models;
using UserandDocumentManagement_JKT.Sevices.Interfaces;

namespace UserandDocumentManagement_JKT.Sevices.Implementations
{
    public class SignupServiceImpl : ISignupService
    {
        private readonly AppDbCotext _context;

        public SignupServiceImpl(AppDbCotext appDbCotext)
        {
            _context = appDbCotext;
        }

        public async Task<string> UserSingup(UserDto userdto)
        {
            if (_context.Users.Any(u => u.Username == userdto.Username))
                return "Username already exists";

            var user = new User
            {
                Username = userdto.Username,
                Password= userdto.Password,
                Email= userdto.Email                
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "Registered successfully";
        }
    }
}
