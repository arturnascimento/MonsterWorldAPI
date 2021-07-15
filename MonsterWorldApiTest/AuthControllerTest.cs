using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonsterWorldApi.Controllers.v1;
using MonsterWorldApi.Models;
using MonsterWorldApi.Services;
using System.Collections.Generic;
using Xunit;

namespace MonsterWorldApiTest
{
    /// <summary>
    /// Definitivamente nao deu certo... mas eu tentei.
    /// </summary>
    public class AuthControllerTest
    {
        List<IdentityUser> _fakeUsers;
        public AuthControllerTest()
        {
            

             _fakeUsers = new List<IdentityUser>
            {

                    new  IdentityUser {Id = "1", UserName =  "U1", Email = "teste1@teste.com", PasswordHash = "hsuahsuahsuahsuhasuhhs"},
                    new  IdentityUser {Id = "2", UserName =  "U2", Email = "teste2@teste.com", PasswordHash = "sasuhuasausaushasasas8h8sha8"}, 
                    new  IdentityUser {Id = "3", UserName =  "U3", Email = "teste3@teste.com", PasswordHash = "gysgaysgyagsasasa"},
                    new  IdentityUser {Id = "4", UserName =  "U4", Email = "teste4@teste.com", PasswordHash = "asashaushsuhsaushaushau"}
            };

        }

            [Theory]
            [InlineData("1", "U1", "teste1@teste.com", "haushaushaushaushauhsau")]
            [InlineData("10", "U10", "teste10@teste.com", "ahsuahsuahsau")]
            [InlineData("20", "U20", "teste20@teste.com", "kkkkkkkkkkkkkkajsiajssa0sa0sas0")]
            [InlineData("30", "U30", "teste30@teste.com", "asjaisjisasansiansijaisjaisj")]
            public void Registro(string id, string username, string email, string password)
            {
                IdentityUser identityUser = new IdentityUser { Id = id, UserName = username, Email = email, PasswordHash = password};

             
                AuthService service = A.Fake<AuthService>();
                ILogger<AuthController> logger = A.Fake<ILogger<AuthController>>();

                var PodeSerCriado = _fakeUsers.Find(u => u.Email == identityUser.Email) == null;

                 A.CallTo(() => service.Create(identityUser));

                var controller = new AuthController(service, logger);

                var result = controller.Created(email, password) as ObjectResult;

                var value = result.Value as APIResponse<string>;

                Assert.True(
                    (PodeSerCriado == true && value.Succeed == true ) ||
                    (PodeSerCriado == false && value.Succeed == false)
                );

            }




    }



    
}
