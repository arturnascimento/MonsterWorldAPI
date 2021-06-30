using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonsterWorldApi.API
{
    public class RoleAuthorizationAttribute : AuthorizeAttribute
    {
        public RoleAuthorizationAttribute(params RoleTypes[] r) //pega os parametros e gera um array r
        {
            var roles = r.Select(x => Enum.GetName(typeof(RoleTypes), x));  // RoleType.Admin,RoleType.Common

            Roles = string.Join(",", roles); // Admin,Common
        }
    }
}
