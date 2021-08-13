using System.Collections.Generic;

namespace KSE.WebAppMvc.Models
{
    public class UserResponseLogin
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserToken UserToken { get; set; }
    }

    public class UserToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserClaims> Claims { get; set; }
    }
    public class UserClaims 
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
