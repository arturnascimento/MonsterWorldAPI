using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MonsterWorldApi.API;
using MonsterWorldApi.Models;
using MonsterWorldApi.Services;
using System;
using System.Net.Mime;


namespace MonsterWorldApi.Controllers.v1
{
    /// <summary>
    /// Controller responsável por gerenciar os endpoints dos monstros.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController, Authorize] //para acessar essa controller o usuario deverá estar logado.
    
    public class MonstersController : CommunicationsController
    {
       
        //chamando minha interface e o serviço
        IMonsterService _service;
        ILogger _logger;

        /// <summary>
        /// Construtor do MonstersController
        /// </summary>
        /// <param name="service">Serviço utilizado para gerenciar os monstros.</param>
        /// <param name="logger">Serviço utilizado para gerar os Logs da API.</param>
        public MonstersController(IMonsterService service, ILogger<MonstersController> logger) //criando construtor para a controller utilizando serviço da interface como entrada
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint responsável pela criação de um novo Monstro.
        /// </summary>
        /// <param name="monster"></param>
        /// <returns></returns>
        [HttpPost] //Create
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse<Monster>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(APIResponse<Monster>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult Create([FromBody] Monster monster)
        {
            _logger.LogInformation($"O Usuário está tentando criar um monstro. {nameof(Create)} as {DateTime.Now}");
            monster.CreatedBy = User.Identity.Name; //quando o usuario criar um monstro, vai aparecer o nome dele no campo CreatedBy
            

            if (_service.Create(monster))
            {
               _logger.LogInformation($"O Usuário criou um monstro com sucesso. {nameof(Create)} as {DateTime.Now}");
                return ApiOk("Monstro foi Criado", monster);
            }
            _logger.LogError($"O Usuário não criou um monstro com sucesso. {nameof(Create)} as {DateTime.Now}");
            return ApiNotFound<Monster>("Erro ao criar o monstro.");
        }
        //funcao create pega os dados do body da requisição de um objeto monster do tipo Monstro, caso o montro nao exista será retornado mensagem de sucesso com o monstro criado, senao será retornado erro
        /// <summary>
        /// Endpoint responsável por retornar uma lista contendo todos os monstros cadastrados até o momento.
        /// </summary>
        /// <returns></returns>
        [HttpGet] //Read GetPadrao
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public IActionResult List()
        {
            _logger.LogInformation($" O Usuário está fazendo uma busca por todos os monstros no banco de dados {nameof(List)} as {DateTime.Now}");
            return ApiOk(_service.Get());
            
        }//get padrao que pega a lista de monstros inteira

        /// <summary>
        /// Endpoint responsável por retornar um determinado monstro, portanto o usuário deverá informar o Id do monstro desejado.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]//Get por Id
        [Route("{id}")]//Adicionada a rota com o Id obrigatorio
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public IActionResult Get(int id)//get por id que chama o metodo get com o id obrigatório
        {
           _logger.LogInformation($"O Usuário está fazendo uma busca por Id de um monstro no banco de dados{nameof(Get)} as {DateTime.Now}");
            var exists = _service.Get(id); //variavel recebe objeto retornado caso ele exista, se nao existir var recebe null
            if(exists != null)
            {
                _logger.LogInformation($"O Usuário obteve o resultado de sua busca {nameof(Get)} as {DateTime.Now}");
                return ApiOk(exists);
            }
            else
            {
               _logger.LogError($"O Usuário buscou um monstro que não existe no banco de dados {nameof(Get)} as {DateTime.Now}");

                return ApiNotFound<Monster>("Monstro não existe");
            }
                                 
        }

        /// <summary>
        /// Endpoint responsável pelo retorno de um monstro aleatório, o usuário deverá informar a dificuldade desejada.
        /// </summary>
        /// <param name="dificulty"></param>
        /// <returns></returns>
        [HttpGet]//Get por dificult
        [Route("level/{dificulty}")]//Adicionada a rota /level/dificulty pq apesar de nao testar pensei que fosse ter conflito entre o get por id
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public IActionResult Get(Dificulties dificulty)//parecido com o get por id, porém temos que passar o numero relativo a dificuldade do monstro que vc deseja de forma aleatoria
        {
           _logger.LogInformation($"O Usuário está fazendo uma busca por um monstro aleatório com a dificuldade desejada{nameof(Get)} as {DateTime.Now}");

            var exists = _service.Get(dificulty); //var exists recebe valor do metodo get com dificuldade como parametro
            if (exists != null) 
            {
                _logger.LogInformation($"O Usuário  obteve o resultado de sua busca {nameof(Get)} as {DateTime.Now}");
                return ApiOk(exists);
            } // se exists for diferente de null retorna ok, se nao retorna o erro
            else
            {
               _logger.LogError($"O Usuário buscou um monstro que não existe no banco de dados {nameof(Get)} as {DateTime.Now}");

                return ApiNotFound<Monster>("Monstro não existe");
            }
                
        }

        /// <summary>
        /// Endpoint responsável por editar um monstro cadastrado.
        /// </summary>
        /// <param name="monster"></param>
        /// <returns></returns>
        [HttpPut]//update
        // pega as informações direto do body por causa da annotation, metodo recebe um objeto monster do tipo Monstro
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult Update([FromBody] Monster monster) 
        {
            _logger.LogInformation($"O Usuário está tentando editar um monstro. {nameof(Update)} as {DateTime.Now}");
            monster.UpdatedBy = User.Identity.Name; //quando o usuario fizer uptade em um monstro vai aparecer o nome dele no campo updatedBy
            if (_service.Update(monster))
            {
                _logger.LogInformation($"O Usuário editou o monstro com sucesso. {nameof(Update)} as {DateTime.Now}");
                return ApiOk("Monstro foi atualizado", _service.Get(monster.Id));//gambiarra pra retornar o monstro depois de atualizado
            }
            _logger.LogError($"O Usuário não editou o monstro com sucesso. {nameof(Update)} as {DateTime.Now}");
            return ApiNotFound<Monster>("Erro ao atualizar o monstro");
        }

        /// <summary>
        /// Endpoint responsável pela exclusão de um monstro cadastrado, para deletar um monstro o usuário deverá ser do tipo Admin e informar o Id do monstro a ser deletado.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete] //delete
        [RoleAuthorization(RoleTypes.Admin)] //somente usuarios pertencentes ao grupo Admin poderão deletar um monstro
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"O Usuário está tendando deletar um monstro. {nameof(Delete)} as {DateTime.Now}");
            if (_service.Delete(id))
            {
                _logger.LogInformation($"O Usuário deletou um monstro com sucesso. {nameof(Delete)} as {DateTime.Now}");
                return ApiOk("Monstro deletado");
            } else
            {
                _logger.LogError($" O usuário tentou deletar um monstro que não existe no banco de dados {nameof(Delete)} as {DateTime.Now}");
                return ApiNotFound<Monster>("Monstro não existe");
            }
        }

        /// <summary>
        /// Endpoint responsável por retornar uma lista de monstros criada por determinada Role, o usuário deverá informar a Role desejada.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("createdByRole/{role}")]//faz o get de uma lista de monstros criada por uma role em específica
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        public  IActionResult GetFromRole([FromRoute]string role)
        {
            var getRoles = _service.Get(role);
            if ( getRoles != null)
            {
                _logger.LogInformation($"O Usuário está fazendo uma busca por um lista de monstros criados por usuários pertencentes a role passada.{nameof(GetFromRole)} as {DateTime.Now}");
                return ApiOk(_service.Get(role));
            }
            
            else
            {
                _logger.LogError($"O Usuário não obteve o resultado de sua busca, pois não existem monstros criados pela role de sua escolha {nameof(GetFromRole)} as {DateTime.Now}");
                return ApiNotFound<Monster>("Monstro não existe");
            }
        }


       

    }
}
