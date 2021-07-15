using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace MonsterWorldApi.API
{
    /// <summary>
    /// Classe responsável por retornar o Enum RoleTypes em forma de array de strings.
    /// </summary>
    public class RoleAuthorizationAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Método responsável por retornar as roles em um array de strings.
        /// </summary>
        /// <param name="r"></param>
        public RoleAuthorizationAttribute(params RoleTypes[] r) //pega os parametros e gera um array r
        {
            var roles = r.Select(x => Enum.GetName(typeof(RoleTypes), x));  // RoleType.Admin,RoleType.Common

            Roles = string.Join(",", roles); // Admin,Common
        }
    }
}
