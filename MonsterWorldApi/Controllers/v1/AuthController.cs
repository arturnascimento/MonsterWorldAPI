using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonsterWorldApi.Services;
using System;
using System.Net.Mime;

namespace MonsterWorldApi.Controllers.v1
{/// <summary>
/// Controller para Autenticação
/// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    
    public class AuthController : CommunicationsController
    {
        //injecoes de dependencia
        AuthService _authService;
        ILogger _logger;
        /// <summary>
        /// Construtor da classe AuthController
        /// </summary>
        /// <param name="authService"></param>
        /// <param name="logger"></param>
        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        /// <summary>
        /// Endpoint responsável pelo cadastro de novos usuários da API, os dados mínimos que deverão ser informados são, Username, Email e PasswordHash.
        /// Por padrão todos os usuários criados pertencem a Role Common não possuindo nível de administrador.
        /// </summary>
        /// <param name="identityUser"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")] //api/Auth/Register
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult Register([FromBody] IdentityUser identityUser) //Registrando um usuario pegando identityUser do body
        {
            _logger.LogInformation($"Acesso a {nameof(Register)} as {DateTime.Now}");
            try
            {

                var result = _authService.Create(identityUser).Result; //chamando o metodo create la do meu serviço de autenticação
                if (result.Succeeded)
                {
                    identityUser.PasswordHash = default;
                    identityUser.SecurityStamp = default;
                    identityUser.ConcurrencyStamp = default;
                    _logger.LogInformation($"Criação de usuário concluída {nameof(Register)} as {DateTime.Now}");
                    return ApiOk(identityUser); //se resul = true o usuario retorna como criado, porem com os atributos acima nulls por segurança
                }
                _logger.LogError($"Erro ao criar usuário {nameof(Register)} as {DateTime.Now}");
                return ApiBadRequest(result.Errors); //se false retorna erro
            }
            catch (Exception e) //pega a exception e joga dentro da variavel e
            {
                _logger.LogError($"Erro ao criar usuário {nameof(Register)} as {DateTime.Now}");
                return ApiBadRequest(e.Message); //retorna a mensagem da exception
            }
        }

        
        /// <summary>
        /// Endpoint responsável por gerar um token de segurança para um usuário registrado na API.
        /// </summary>
        /// <param name="identityUser"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Token")] //api/Auth/Token
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]

        public IActionResult Token([FromBody] IdentityUser identityUser) // recebe email e senha via body
        {
            _logger.LogInformation($"Usuário tentando pegar o token de autenticação {nameof(Token)} as {DateTime.Now}");
            try
            {
                _logger.LogInformation($"Usuário pegou o token de autenticação {nameof(Token)} as {DateTime.Now}");
                return ApiOk(_authService.GenerateToken(identityUser)); //chama o GenerateToken do serviço de autenticação, retornando o token do usuario informado
            }
            catch (Exception e) //pega a exception e joga dentro da variavel "e"
            {
                _logger.LogError($"Usuário não pegou o token de autenticação {nameof(Token)} as {DateTime.Now}");
                return ApiBadRequest(e.Message); //retorna a mensagem da exception
            }
        }

    }
}
