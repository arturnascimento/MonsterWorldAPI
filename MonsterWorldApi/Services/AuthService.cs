using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MonsterWorldApi.API;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MonsterWorldApi.Services
{
    public class AuthService
    {
        //injecoes de dependencia do usermanager e configuration
        UserManager<IdentityUser> _userManager; 
        IConfiguration _configuration;
        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<IdentityResult> Create(IdentityUser identityUser) // metodo de criacao de usuario
        {
            var created = await _userManager.CreateAsync(identityUser, identityUser.PasswordHash); // recebe identity user e passwordHash

            if (created.Succeeded) //ser created.Succeeded = true
            {
                var user = _userManager.FindByEmailAsync(identityUser.Email); //pega o usuario pelo email
                await _userManager.AddToRoleAsync(user.Result, Enum.GetName(default(RoleTypes))); 
                //define uma role default para o usuario que no nosso caso será a Common.
            }

            return created; //retorna o usuário criado com a role default


        }
        public async Task<bool> isValidLogin(IdentityUser identityUser) //validando o usuario
        {
            var user = await _userManager.FindByEmailAsync(identityUser.Email);//pego o usuario atraves do email
            if (_userManager.GetRolesAsync(user).Result == null) return false; // validando se o usuário possui uma role
            return await _userManager.CheckPasswordAsync(user, identityUser.PasswordHash); //compara se a senha do usuario capturado existe.

        }

        public string GenerateToken(IdentityUser identityUser) //metódo para gerar o token recebendo um identityUser de parametro.
        {

            if (!isValidLogin(identityUser).Result) throw new Exception("Invalid credentials");

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
                        new Claim(ClaimTypes.Role, role) // tipo de claim do grupo ao qual o usuario pertence
                    }),
                Expires = DateTime.UtcNow.AddSeconds(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);// token recebe tokenHandler
            //do tipo JwtSecurityTokenHandler classe que possui o método CreateToken, utilizando o token descriptor como base para a criação do token.



            return tokenHandler.WriteToken(token); //tokenHandler chamando o metodo de escrever token
            //finalizando o método GenerateToken

        }

    }
}
