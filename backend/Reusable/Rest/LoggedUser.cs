using Reusable.CRUD.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Reusable.Rest
{
    public interface ILoggedUser
    {
        string Role { get; set; }
        long? UserID { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
    }

    public class LoggedUser : ILoggedUser
    {
        ClaimsIdentity identity;

        public LoggedUser()
        {
            Role = null;
            UserID = null;
            UserName = null;
            Email = null;

            FillValue();
        }

        public LoggedUser(long userKey, string sUserName, string sRole)
        {
            Role = sRole;
            UserID = userKey;
            UserName = sUserName;

            FillValue();
        }

        public LoggedUser(long userKey, string sUserName, string sRole, string sEmail)
        {
            Role = sRole;
            UserID = userKey;
            UserName = sUserName;
            Email = sEmail;

            FillValue();
        }

        public LoggedUser(long userKey, string sUserName, string sRole, string sEmail, string sValue)
        {
            Role = sRole;
            UserID = userKey;
            UserName = sUserName;
            Email = sEmail;
            Value = sValue;

            FillValue();
        }

        public LoggedUser(ClaimsIdentity identity)
        {
            Role = null;
            UserID = null;
            UserName = null;
            Email = null;
            Value = null;

            if (identity != null)
            {
                this.identity = identity;
                IEnumerable<Claim> claims = identity.Claims;

                if (claims.Count() > 0)
                {
                    Claim claimRole = claims.FirstOrDefault(c => c.Type == "role");
                    if (claimRole != null)
                    {
                        Role = claimRole.Value;
                    }
                    Claim claimUserID = claims.FirstOrDefault(c => c.Type == "userID" || c.Type == "sub");
                    if (claimUserID != null)
                    {
                        UserID = long.Parse(claimUserID.Value);
                    }
                    Claim claimUserName = claims.FirstOrDefault(c => c.Type == "userName" || c.Type == "preferred_username");
                    if (claimUserName != null)
                    {
                        UserName = claimUserName.Value;
                    }
                    Claim claimEmail = claims.FirstOrDefault(c => c.Type == "email");
                    if (claimEmail != null)
                    {
                        Email = claimEmail.Value;
                    }
                    Claim claimValue = claims.FirstOrDefault(c => c.Type == "nickname");
                    if (claimValue != null)
                    {
                        Value = claimValue.Value;
                    }
                    Claim claimIdentityProvider = claims.FirstOrDefault(c => c.Type == "idp");
                    if (claimIdentityProvider != null)
                    {
                        IdentityProvider = claimIdentityProvider.Value?.ToLower().Trim();
                    }
                }
            }
            FillValue();
        }

        public string Role { get; set; }
        public long? UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Value { get; set; }
        public string IdentityProvider { get; set; }

        public User LocalUser { get; set; }

        private void FillValue()
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                Value = IdentityProvider?.ToLower().Trim() == "google" ? Email
                    : !string.IsNullOrWhiteSpace(UserName) ? UserName
                    : !string.IsNullOrWhiteSpace(Email) ? Email
                    : null;
            }
        }
    }
}
