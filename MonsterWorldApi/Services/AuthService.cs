using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MonsterWorldApi.API;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MonsterWorldApi.Services
{
    /// <summary>
    /// Serviço que gerencia a parte de autenticação (Token Jwt) da API.
    /// </summary>
    public class AuthService
    {
        //injecoes de dependencia do usermanager e configuration
        UserManager<IdentityUser> _userManager; 
        IConfiguration _configuration;
        ILogger _logger;
        /// <summary>
        /// Construtor da classe AuthService
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Método para criação de um novo usuário.
        /// </summary>
        /// <param name="identityUser"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Create(IdentityUser identityUser) // metodo de criacao de usuario
        {
            _logger.LogInformation($"Tentando criar usuário em {nameof(Create)} as {DateTime.Now}");
            var created = await _userManager.CreateAsync(identityUser, identityUser.PasswordHash); // recebe identity user e passwordHash

            if (created.Succeeded) //ser created.Succeeded = true
            {
                var user = _userManager.FindByEmailAsync(identityUser.Email); //pega o usuario pelo email
                await _userManager.AddToRoleAsync(user.Result, Enum.GetName(default(RoleTypes))); 
                //define uma role default para o usuario que no nosso caso será a Common.
            }
            else
            {
                _logger.LogError($"Erro ao crair usuário {nameof(Create)} as {DateTime.Now}");
            }
            _logger.LogInformation($"Usuário Criado com sua role padrão. {nameof(Create)} as {DateTime.Now}");
            return created; //retorna o usuário criado com a role default


        }
        /// <summary>
        /// Método responsável por validar se o usuário está mesmo cadastrado e a qual role ele pertence.
        /// </summary>
        /// <param name="identityUser"></param>
        /// <returns></returns>
        public async Task<bool> isValidLogin(IdentityUser identityUser) //validando o usuario
        {
            _logger.LogInformation($"Usuário tentando fazer login {nameof(isValidLogin)} as {DateTime.Now}");
            var user = await _userManager.FindByEmailAsync(identityUser.Email);//pego o usuario atraves do email
            if (_userManager.GetRolesAsync(user).Result == null) 
            {
                _logger.LogError($"Usuário não possui uma role {nameof(isValidLogin)} as {DateTime.Now}");
                return false; // validando se o usuário possui uma role
            }
            else
            {
                _logger.LogInformation($"Usuário fazendo login {nameof(isValidLogin)} as {DateTime.Now}");
                return await _userManager.CheckPasswordAsync(user, identityUser.PasswordHash); //compara se a senha do usuario capturado existe.
            } 
            

        }

        /// <summary>
        /// Método responsável pela geração dos Tokens para os usuários que passarem pela validação, a role é determinante para algumas permissões.
        /// </summary>
        /// <param name="identityUser"></param>
        /// <returns></returns>
        public string GenerateToken(IdentityUser identityUser) //metódo para gerar o token recebendo um identityUser de parametro.
        {
            _logger.LogInformation($"Usuário tentando gerar um token válido {nameof(GenerateToken)} as {DateTime.Now}");
            if (!isValidLogin(identityUser).Result) 
            {
                _logger.LogError($"Usuário inválido {nameof(GenerateToken)} as {DateTime.Now}");
                throw new Exception("Invalid credentials");
            }

            _logger.LogInformation($"Usuário válido, gerando o Token {nameof(GenerateToken)} as {DateTime.Now}");
            var user = _userManager.FindByEmailAsync(identityUser.Email).Result; //pegando usuario pelo email

            var role = _userManager.GetRolesAsync(user).Result[0]; // pegando a sua primeira role

            var tokenHandler = new JwtSecurityTokenHandler(); //criando  a variável do tipo JwtSecurityTokenHandler do framework
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); //encodando a key do appsettings.json e colocando na variavel key

            var tokenDescriptor = new SecurityTokenDescriptor //descrição das minhas claims, assim como caracteristicas
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.UserName), //tipo de claim por nome, nome do usuario
                        new Claim(ClaimTypes.Role, role), // tipo de claim do grupo ao qual o usuario pertence
                        new Claim(ClaimTypes.NameIdentifier, user.Id) 
                    }),
                Expires = DateTime.UtcNow.AddSeconds(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);// token recebe tokenHandler
            //do tipo JwtSecurityTokenHandler classe que possui o método CreateToken, utilizando o token descriptor como base para a criação do token.


            _logger.LogInformation($" Token Gerado com sucesso {nameof(GenerateToken)} as {DateTime.Now}");
            return tokenHandler.WriteToken(token); //tokenHandler chamando o metodo de escrever token
            //finalizando o método GenerateToken

        }

    }
}
