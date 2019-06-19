using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CoEco.BO.Config
{
    public class AppAuthConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("tokenPath", IsRequired = true)]
        public string TokenPath
        {
            get { return (string)this["tokenPath"]; }
            set { this["tokenPath"] = value; }
        }

        [ConfigurationProperty("expirationMinutes", IsRequired = true)]
        public int ExpirationMinutes
        {
            get { return (int)this["expirationMinutes"]; }
            set { this["expirationMinutes"] = value; }
        }

        [ConfigurationProperty("jwtKey")]
        public string JwtKey
        {
            get { return (string)this["jwtKey"]; }
            set { this["jwtKey"] = value; }
        }

        [ConfigurationProperty("jwtIssuer")]
        public string JwtIssuer
        {
            get { return (string)this["jwtIssuer"]; }
            set { this["jwtIssuer"] = value; }
        }

        [ConfigurationProperty("jwtAudience")]
        public string JwtAudience
        {
            get { return (string)this["jwtAudience"]; }
            set { this["jwtAudience"] = value; }
        }

        [ConfigurationProperty("userLockoutEnabledByDefault")]
        public bool UserLockoutEnabledByDefault
        {
            get { return Convert.ToBoolean(this["userLockoutEnabledByDefault"]); }
            set { this["userLockoutEnabledByDefault"] = value; }
        }

        [ConfigurationProperty("defaultAccountLockoutTimeSpan")]
        public int DefaultAccountLockoutTimeSpan
        {
            get { return Convert.ToInt32(this["defaultAccountLockoutTimeSpan"]); }
            set { this["defaultAccountLockoutTimeSpan"] = value; }
        }

        [ConfigurationProperty("maxFaileAccessAttemptsBeforeLockout")]
        public int MaxFailedAccessAttemptsBeforeLockout
        {
            get { return Convert.ToInt32(this["maxFaileAccessAttemptsBeforeLockout"]); }
            set { this["maxFaileAccessAttemptsBeforeLockout"] = value; }
        }
        [ConfigurationProperty("rolesClaimName")]
        public string RolesClaimName
        {
            get { return "ActiveRoles"; }
            //get { return (this["rolesClaimName"] ?? "ActiveRoles").ToString(); }
            set { this["rolesClaimName"] = value; }
        }

        public static AppAuthConfiguration Config
        {
            get { return ConfigurationManager.GetSection("appAuthConfiguration") as AppAuthConfiguration; }
        }
    }
}