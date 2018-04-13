using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;
using OneTravelApi.Models;

namespace OneTravelApi.Services
{
    public interface IUserService
    {
        Task<bool> UserLoggedIn(string email, UserResultListResource user);
        Task<bool> UpdateAsync(string email, SaveUserResource user);
        Task<User> Get(string email);
    }
    public class UserService : IUserService
    {
        public const string BaseAdminApiUrl = "http://localhost:49391/api/";
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> UserLoggedIn(string email, UserResultListResource user)
        {
            try
            {
                if (_userRepository.Query().Any(x => x.UserEmail == email))
                {
                    var userToUpdate = await _userRepository.FindAsync(x => x.UserEmail == email);
                    userToUpdate.LastLogin = DateTime.Now;
                    await _userRepository.UpdateAsync(userToUpdate);
                }
                else
                {
                    var userToAdd = new User
                    {
                        FullName = user.FullName,
                        Avatar = user.Avatar,
                        LastLogin = DateTime.Now,
                        UserEmail = user.Email,
                        UserIdentifer = user.UserIdentifier
                    };

                    await _userRepository.UpdateAsync(userToAdd);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(string email, SaveUserResource user)
        {
            try
            {
                var userToUpdate = await _userRepository.FindAsync(x => x.UserEmail == email);

                if (userToUpdate == null) return false;

                userToUpdate.Avatar = user.Avatar;
                userToUpdate.FullName = user.FullName;
                userToUpdate.LastLogin = user.LastLogin;
                await _userRepository.UpdateAsync(userToUpdate);

                using (var http = new HttpClient())
                {
                    var jsonString = JsonConvert.SerializeObject(user);
                    var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    await http.PutAsync(BaseAdminApiUrl + "user/update/" + email, httpContent);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User> Get(string email)
        {
            return await _userRepository.FindAsync(x => x.UserEmail == email);
        }
    }
}
