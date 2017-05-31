using Microsoft.AspNet.Identity;

namespace LoginApp.Users
{
    public static class UserManagerFactory
    {
        public static UserManager<User> Create() => new UserManager<User>(new UserStore());
    }
}