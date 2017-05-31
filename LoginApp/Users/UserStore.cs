using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using Microsoft.AspNet.Identity;

namespace LoginApp.Users
{
    public class UserStore : IUserPasswordStore<User>, IUserSecurityStampStore<User>, IUserStore<User>
    {
        private static readonly ConcurrentDictionary<string, string> SecurityStamps = new ConcurrentDictionary<string, string>();

        public void Dispose()
        {
        }

        public Task CreateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(User user) => await Task.Yield();

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(string userId) => FindByNameAsync(userId);

        public async Task<User> FindByNameAsync(string userName)
        {
            var users = await LoadAllUsersAsync();
            return users.FirstOrDefault(u => u.UserName == userName);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(User user) => Task.FromResult(user.PasswordHash);

        public Task<bool> HasPasswordAsync(User user) => Task.FromResult(true);
             
        public async Task<IEnumerable<User>> LoadAllUsersAsync()
        {
            var path = HostingEnvironment.MapPath(Consts.Filename);
            if (path == null) throw new Exception($"Can't map file {Consts.Filename}");

            string text;
            using (var reader = File.OpenText(path))
            {
                text = await reader.ReadToEndAsync();
            }

            return text
                .Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim())
                .Select(l => l.Split())
                .Select(l => new User(l[0], l[1]))
                .ToList();
        }

        public async Task SetSecurityStampAsync(User user, string stamp)
        {
            SecurityStamps.AddOrUpdate(user.UserName, stamp, (_, __) => stamp);
            await Task.Yield();
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            string value;
            return SecurityStamps.TryGetValue(user.UserName, out value) ? Task.FromResult(value) : Task.FromResult(string.Empty);
        }
    }
}