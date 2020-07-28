using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ExternalAuthentication
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private int minimumPasswordLength = 8;

        public ApplicationUserManager() : base(new UserStore<ApplicationUser>(new MyApplicationDbContext()))
        {
            PasswordValidator = new MinimumLengthValidator(minimumPasswordLength);
        }

    }
}





















