using BusinessObjects.Models;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class AccountService
    {
        private readonly NetCafeContext _context;

        public AccountService(NetCafeContext context)
        {
            _context = context;
        }

        public Account? ValidateLogin(string username, string password)
        {
            var account = _context.Accounts
                .Include(a => a.Employee) // Include để truy cập Role
                .FirstOrDefault(a =>
                    a.Username == username &&
                    a.PasswordHash == password &&
                    a.IsActive == true
                );

            // Debug nếu cần
            if (account == null)
            {
                System.Diagnostics.Debug.WriteLine($"== LOGIN FAILED ==\nUsername: {username}\nPassword: {password}");
            }

            return account;
        }
    }
}
