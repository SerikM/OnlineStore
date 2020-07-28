using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ExternalAuthentication
{
   public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
       public ApplicationRoleManager (): base(new RoleStore<ApplicationRole>(new MyApplicationDbContext()))
       {
       
       }

    }
}
