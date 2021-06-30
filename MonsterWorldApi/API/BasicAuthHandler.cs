using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonsterWorldApi.Services;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MonsterWorldApi.API
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        //injecao de dependencia 
        AuthService _authService;
        public BasicAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, //construtor da classe
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            AuthService authService
            ) : base (options, logger, encoder, clock)
        {
            _authService = authService;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                string rawAuthHeader = Request.Headers["Authorization"]; //pega a annotation do header
                AuthenticationHeaderValue authHeader = AuthenticationHeaderValue.Parse(rawAuthHeader);
                string rawParameter = authHeader.Parameter;
                byte[] bcrendentials = Convert.FromBase64String(rawParameter);//pega as credentials encodadas
                string scredentials = Encoding.UTF8.GetString(bcrendentials);//joga pra utf8
                string[] credentials = scredentials.Split(":");//divide a string em 2 strings separadas pelo :
                string email = credentials[0];//o email será a posição 0 do array de string
                string password = credentials[1]; // a senha será a posição 1 do array

                var isValidLogin = await _authService.isValidLogin(new IdentityUser { Email = email, PasswordHash = password });
                //recebe resultado do metodo isvalidlogin do serviço de autenticação, observando se o email e senha conferem

                if (!isValidLogin)
                {
                    return AuthenticateResult.Fail("Authentication Failed: Invalid Credentials.");// se isvalidlogin = false retorna erro
                }
                var claims = new[]//se true cria array de claims com as claims de email e nome, q sao iguais em valor nesse caso
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, email),
                };

                //config padrao para conseguir o ticket de acesso.
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Authentication failed.");
            }

            
        }
    }
}
