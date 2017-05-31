using Microsoft.AspNet.Identity;

namespace LoginApp.Users
{
    public class User : IUser<string>
    {
        public User(string username, string passwordHash)
        {
            UserName = username;
            PasswordHash = passwordHash;
        }

        public string Id => UserName;
        public string UserName { get; set; }
        public string PasswordHash { get; }
    }
}