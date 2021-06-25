using Microsoft.AspNetCore.Mvc;
using MonsterWorldApi.API;
using MonsterWorldApi.Models;
using MonsterWorldApi.Services;

namespace MonsterWorldApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonstersController : CommunicationsController
    {
        //chamando minha interface e o serviço
        IMonsterService _service;
        public MonstersController(IMonsterService service) //criando construtor para a controller utilizando serviço da interface como entrada
        {
            _service = service;
        }

        [HttpPost] //Create
        public IActionResult Create([FromBody] Monster monster) => _service.Create(monster) ? ApiOk("Monstro brabo criado com sucesso.", monster) : ApiNotFound("Erro na criação do monstro.");
        //funcao create pega os dados do body da requisição de um objeto monster do tipo Monstro, caso o montro nao exista será retornado mensagem de sucesso com o monstro criado, senao será retornado erro
        [HttpGet] //Read GetPadrao
        public IActionResult List() => ApiOk(_service.Get());//get padrao que pega a lista de monstros inteira

        [HttpGet]//Get por Id
        [Route("{id}")]//Adicionada a rota com o Id obrigatorio
        public IActionResult Get(int id)//get por id que chama o metodo get com o id obrigatório
        {
            var exists = _service.Get(id); //variavel recebe objeto retornado caso ele exista, se nao existir var recebe null
            return exists != null ?
                ApiOk(exists) :
                ApiNotFound("Monstro não existe");
        }

        [HttpGet]//Get por dificult
        [Route("level/{dificulty}")]//Adicionada a rota /level/dificulty pq apesar de nao testar pensei que fosse ter conflito entre o get por id
        public IActionResult Get(Dificulties dificulty)//parecido com o get por id, porém temos que passar o numero relativo a dificuldade do monstro que vc deseja de forma aleatoria
        {
            var exists = _service.Get(dificulty); //var exists recebe valor do metodo get com dificuldade como parametro
            return exists != null ? // se exists for diferente de null retorna ok, se nao retorna o erro
                ApiOk(exists) :
                ApiNotFound("Monstro não existe");
        }

        [HttpPut] //update
        // pega as informações direto do body por causa da annotation, metodo recebe um objeto monster do tipo Monstro
        public IActionResult Update([FromBody] Monster monster) => _service.Update(monster) ? ApiOk("Monstro foi atualizado", monster) : ApiNotFound("Erro ao atualizar o monstro");

        [HttpDelete] //delete
        [Route("{id}")]
        public IActionResult Delete(int id) => _service.Delete(id) ? ApiOk("Monstro deletado") : ApiNotFound("Monstro não existe");



    }
}
