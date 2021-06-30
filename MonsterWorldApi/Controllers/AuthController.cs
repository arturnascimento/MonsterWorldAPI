using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MonsterWorldApi.Services;
using System;


namespace MonsterWorldApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : CommunicationsController
    {
        //injecoes de dependencia
        AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService; 
        }

        [HttpPost]
        [Route("Register")] //api/Auth/Register
        public IActionResult Register([FromBody] IdentityUser identityUser) //Registrando um usuario pegando identityUser do body
        {
            try
            {

                var result = _authService.Create(identityUser).Result; //chamando o metodo create la do meu serviço de autenticação
                if (result.Succeeded)
                {
                    identityUser.PasswordHash = default;
                    identityUser.SecurityStamp = default;
                    identityUser.ConcurrencyStamp = default;
                    return ApiOk(identityUser); //se resul = true o usuario retorna como criado, porem com os atributos acima nulls por segurança
                }
                return ApiBadRequest(result.Errors); //se false retorna erro
            }
            catch (Exception e) //pega a exception e joga dentro da variavel e
            {
                return ApiBadRequest(e.Message); //retorna a mensagem da exception
            }
        }

        [HttpPost]
        [Route("Token")] //api/Auth/Token

        public IActionResult Token([FromBody] IdentityUser identityUser) // recebe email e senha via body
        {
            try
            {
                return ApiOk(_authService.GenerateToken(identityUser)); //chama o GenerateToken do serviço de autenticação, retornando o token do usuario informado
            }
            catch (Exception e) //pega a exception e joga dentro da variavel e
            {
                return ApiBadRequest(e.Message); //retorna a mensagem da exception
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            return ApiOk(User.Identity.Name); //metodo para retornar o nome do usuario
        }
    }
}
