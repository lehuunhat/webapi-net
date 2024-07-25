using HienTangToc.Data;
using HienTangToc.Helpers;
using HienTangToc.Models;
using Microsoft.EntityFrameworkCore;

namespace HienTangToc.Services
{
    public class UserService
    {
        private readonly MyDbContext _context;

        public UserService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserModel>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<(UserModel user, string message)> LoginAsync(string emailorusername, string password)
        {
            if (string.IsNullOrWhiteSpace(emailorusername) || string.IsNullOrWhiteSpace(password))
            {
                return (null, "Email/Username và mật khẩu không được để trống.");
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Email == emailorusername || x.UserName == emailorusername);

            if (user == null)
            {
                return (null, "Thông tin đăng nhập không chính xác.");
            }

            bool isPasswordValid = PasswordHelper.VerifyPassword(password, user.Password);
            if (!isPasswordValid)
            {
                return (null, "Mật khẩu đăng nhập không chính xác.");
            }

            return (user, "Đăng nhập thành công.");
        }

        public async Task<(UserModel user, string message)> CreateUserAsync(UserModel user)
        {
            var existingUserByUsername = await _context.Users
           .FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (existingUserByUsername != null)
            {
                return (null, "Username đã tồn tại.");
            }

            var existingUserByEmail = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUserByEmail != null)
            {
                return (null, "Email đã tồn tại.");
            }
            user.Password = PasswordHelper.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (user, "Tạo user thành công.");
        }

        public async Task<(UserModel originalUser, UserModel updatedUser, string message)> UpdateUserAsync(int Id, UserModel userUpdate)
        {
            var existingUser = await _context.Users.FindAsync(Id);
            if (existingUser == null)
            {
                return (null, null, "User không tồn tại.");
            }

            var userWithSameUsername = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userUpdate.UserName && u.Id != Id);

            if (userWithSameUsername != null)
            {
                return (null, null, "Username đã tồn tại.");
            }

            var userWithSameEmail = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userUpdate.Email && u.Id != Id);

            if (userWithSameEmail != null)
            {
                return (null, null, "Email đã tồn tại.");
            }

            var originalUser = new UserModel
            {
                Id = existingUser.Id,
                UserName = existingUser.UserName,
                Email = existingUser.Email
            };

            existingUser.UserName = userUpdate.UserName;
            existingUser.Email = userUpdate.Email;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return (originalUser, existingUser, "Cập nhật user thành công.");
        }

        public async Task<DeleteUserResponse> DeleteUserAsync(int id)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return new DeleteUserResponse
                {
                    Success = false,
                    Message = "Không tìm thấy user"
                };
            }

            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();

            return new DeleteUserResponse
            {
                Success = true,
                Message = "Xóa thành công"
            };
        }

        public class DeleteUserResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }
    }
}
