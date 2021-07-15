using System.Collections.Generic;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonsterWorldApi.API;
using MonsterWorldApi.Controllers.v1;
using MonsterWorldApi.Models;
using MonsterWorldApi.Services;
using Xunit;

namespace MonsterWorldApiTest
{
    /// <summary>
    /// Testes para validar os métodos da MonstersController
    /// Para que os testes funcionem corretamente deveremos comentar as linhas 34 e 118 de MonstersController
    /// </summary>
    public class MonstersControllerTest
    {
        List<Monster> _fakeMonsters;

        public MonstersControllerTest()
        {
            _fakeMonsters = new List<Monster>
            {

                    new Monster {Id = 1, Name = "M1", HP = 100, Attack = 100, Experience = 100, Dificulty = MonsterWorldApi.API.Dificulties.Nightmare, CreatedBy = "System", UpdatedBy = ""},
                    new Monster {Id = 2, Name = "M2", HP = 90, Attack = 900, Experience = 90, Dificulty = MonsterWorldApi.API.Dificulties.Extreme, CreatedBy = "System", UpdatedBy = ""},
                    new Monster {Id = 3, Name = "M3", HP = 80, Attack = 80, Experience = 80, Dificulty = MonsterWorldApi.API.Dificulties.Hard, CreatedBy = "Admin", UpdatedBy = ""},
                    new Monster {Id = 4, Name = "M4", HP = 70, Attack = 70, Experience = 70, Dificulty = MonsterWorldApi.API.Dificulties.Normal, CreatedBy = "Admin", UpdatedBy = ""},
                    new Monster {Id = 5, Name = "M5", HP = 60, Attack = 60, Experience = 60, Dificulty = MonsterWorldApi.API.Dificulties.Easy, CreatedBy = "Common", UpdatedBy = ""},
                    new Monster {Id = 6, Name = "M6", HP = 50, Attack = 50, Experience = 50, Dificulty = MonsterWorldApi.API.Dificulties.Sandbox, CreatedBy = "Common", UpdatedBy = ""}

            };

        }


        //GetAllMonsters
        /// <summary>
        /// Teste para validar o retorno da lista de todos os monstros.
        /// </summary>
        [Fact]
        public void GetAllTest()
        {
            IMonsterService service = A.Fake<IMonsterService>();
            ILogger<MonstersController> logger = A.Fake<ILogger<MonstersController>>();
            A.CallTo(() => service.Get()).Returns(_fakeMonsters);
            //A.CallTo(() => logger.LogInformation("teste"));

            var controller = new MonstersController(service, logger);
            var result = controller.List() as OkObjectResult;
            var values = result.Value as APIResponse<List<Monster>>;

            Assert.True
                (
                    values.Results == _fakeMonsters && values.Succeed == true
                );
        }

        /// <summary>
        /// Teste para validar o retorno de um monstro com o Id como parâmetro.
        /// </summary>
        /// <param name="id"></param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(0)]
        [InlineData(-150)]
        [InlineData(150)]
        public void GetById(int id)
        {
            IMonsterService service = A.Fake<IMonsterService>();
            ILogger<MonstersController> logger = A.Fake<ILogger<MonstersController>>();

            var valid_result = _fakeMonsters.Find(m => m.Id == id);
            bool valid_succeed = (valid_result != null);

            A.CallTo(() => service.Get(id)).Returns(valid_result);

            var controller = new MonstersController(service, logger);

            var result = controller.Get(id) as ObjectResult;
            var value = result.Value as APIResponse<Monster>;

            Assert.True(value.Results == valid_result && value.Succeed == valid_succeed);


        }

        /// <summary>
        /// Teste para validar o retorno de um monstro de acordo com sua dificuldade
        /// </summary>
        /// <param name="dificulty"></param>
        [Theory]
        [InlineData(MonsterWorldApi.API.Dificulties.Sandbox)]
        [InlineData(MonsterWorldApi.API.Dificulties.Easy)]
        [InlineData(MonsterWorldApi.API.Dificulties.Normal)]
        [InlineData(MonsterWorldApi.API.Dificulties.Hard)]
        [InlineData(MonsterWorldApi.API.Dificulties.Extreme)]
        [InlineData(MonsterWorldApi.API.Dificulties.Nightmare)]
        public void GetByDificulty(Dificulties dificulty)
        {
            IMonsterService service = A.Fake<IMonsterService>();
            ILogger<MonstersController> logger = A.Fake<ILogger<MonstersController>>();

            var valid_result = _fakeMonsters.Find(m => m.Dificulty == dificulty);
            bool valid_succeed = (valid_result != null);

            A.CallTo(() => service.Get(dificulty)).Returns(valid_result);

            var controller = new MonstersController(service, logger);

            var result = controller.Get(dificulty) as ObjectResult;
            var value = result.Value as APIResponse<Monster>;

            Assert.True(value.Results == valid_result && value.Succeed == valid_succeed);


        }

        /// <summary>
        /// Teste para validar o retorno de uma lista de monstros baseada no tipo de role.
        /// </summary>
        /// <param name="role"></param>
        [Theory]
        [InlineData("Admin")]
        [InlineData("Common")]
        [InlineData("Vagabond")]

        public void GetByRole(string role)
        {
            IMonsterService service = A.Fake<IMonsterService>();
            ILogger<MonstersController> logger = A.Fake<ILogger<MonstersController>>();

            var valid_result = _fakeMonsters.FindAll(m => m.CreatedBy == role);
            bool valid_succeed = (valid_result != null);

            A.CallTo(() => service.Get(role)).Returns(valid_result);

            var controller = new MonstersController(service, logger);

            var result = controller.GetFromRole(role) as ObjectResult;
            var value = result.Value as APIResponse<List<Monster>>;

            Assert.True(value.Results == valid_result && value.Succeed == valid_succeed);


        }


        /// <summary>
        /// Teste para validar a criação de um monstro, deve ser comentada a linha 34 da MonstersController para o teste funcionar
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="hp"></param>
        /// <param name="attack"></param>
        /// <param name="exp"></param>
        /// <param name="cBy"></param>
        /// <param name="uBy"></param>
        [Theory]
        [InlineData("M1", 100, 100, 100, "System", "Teste")]
        [InlineData("M10", 100, 100, 100, "System", "Teste")]
        [InlineData("M11", 100, 100, 100, "System", "Teste")]
        [InlineData("M2", 100, 100, 100, "System", "Teste")]
        public void Create(string nome, int hp, int attack, int exp, string cBy, string uBy)
        {
            Monster monster = new Monster { Name = nome, HP = hp, Attack = attack, Experience = exp, CreatedBy = cBy, UpdatedBy = uBy };

            IMonsterService service = A.Fake<IMonsterService>();
            ILogger<MonstersController> logger = A.Fake<ILogger<MonstersController>>();

            var PodeSerCriado = _fakeMonsters.Find(m => m.Name == monster.Name) == null;

            A.CallTo(() => service.Create(monster)).Returns(PodeSerCriado);

            var controller = new MonstersController(service, logger);

            var result = controller.Create(monster) as ObjectResult;

            var value = result.Value as APIResponse<Monster>;

            Assert.True(
                (PodeSerCriado && value.Results == monster && value.Succeed == true) ||
                (!PodeSerCriado && value.Results == null && value.Succeed == false)
            );

        }


        /// <summary>
        /// Teste para validar a edição de monstro, deve ser comentada a linha 118 da MonstersController para o teste funcionar.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nome"></param>
        /// <param name="hp"></param>
        /// <param name="attack"></param>
        /// <param name="exp"></param>
        /// <param name="cBy"></param>
        /// <param name="uBy"></param>
        [Theory]
        [InlineData(1,"M1", 100, 100, 100, "System", "Teste")]
        [InlineData(2,"M10", 100, 100, 100, "System", "Teste")]
        [InlineData(3,"M11", 100, 100, 100, "System", "Teste")]
        [InlineData(50,"M2", 100, 100, 100, "System", "Teste")]
        public void UpdateByID(int id, string nome, int hp, int attack, int exp, string cBy, string uBy)
        {
            Monster monster = new Monster {Id = id, Name = nome, HP = hp, Attack = attack, Experience = exp, CreatedBy = cBy, UpdatedBy = uBy };

            IMonsterService service = A.Fake<IMonsterService>();
            ILogger<MonstersController> logger = A.Fake<ILogger<MonstersController>>();

            var PodeSerEditado = _fakeMonsters.Find(m => m.Id == monster.Id) != null;

            A.CallTo(() => service.Update(monster)).Returns(PodeSerEditado);

            var controller = new MonstersController(service, logger);

            var result = controller.Update(monster) as ObjectResult;

            var value = result.Value as APIResponse<Monster>;

            Assert.True(
                (PodeSerEditado == true && value.Succeed == true ) || (PodeSerEditado == false && value.Succeed == false));

        }

        /// <summary>
        /// Teste para validar método de exlusão de monstrons utilizando o Id como parâmetro.
        /// </summary>
        /// <param name="id"></param>
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(0)]
        [InlineData(-150)]
        [InlineData(150)]
        public void DeleteById(int id)
        {
            IMonsterService service = A.Fake<IMonsterService>();
            ILogger<MonstersController> logger = A.Fake<ILogger<MonstersController>>();

            var valid_result = _fakeMonsters.Find(m => m.Id == id);
            bool valid_succeed = (valid_result != null);
            A.CallTo(() => service.Delete(id));
            
            //var valid_resultDelete = _fakeMonsters.Find(m => m.Id == id);
            //bool Valid_succeedDelete = (valid_resultDelete == null);

            var controller = new MonstersController(service, logger);

            var result = controller.Delete(id) as ObjectResult;
            var value = result.Value as APIResponse<Monster>;

            Assert.True(value.Results == valid_result && value.Succeed == valid_succeed || valid_result != null && value.Results != valid_result );


        }
    }
}
