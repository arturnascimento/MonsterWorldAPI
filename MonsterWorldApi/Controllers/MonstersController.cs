using Microsoft.AspNetCore.Mvc;
using MonsterWorldApi.API;
using MonsterWorldApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonsterWorldApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonstersController : ControllerBase
    {
        // função allmonsters, cria os monstros e retorna uma lista do tipo monster
        List<Monster> AllMonsters()
        {
            //rnd foi um random pra gerar os atributos dos monstros
            Random rnd = new Random();
            //instanciando uma lista de Monster com o nome de monsters
            List<Monster> monsters = new List<Monster>();

            // Array de strings com os nomes dos monstros
            string[] monsternames = new string[] { "Orc", "Troll", "Goblin", "Saci", "Curupira", "Cuca" };

            //for que usei para que cada nome do array entre no nesse for
            for (int i = 0; i < monsternames.Length; i++)
            {
               //fator de dificuldade do monstro
                int DificultyFactor = 1;
                //foreach q percorre o enum e criar os monstros
                foreach (Dificulties dificulty in Enum.GetValues(typeof(Dificulties)))
                {
                    monsters.Add(new Monster
                            {
                                Id = monsters.Count + 1,
                                Name = dificulty + " " + monsternames[i],
                                HP = rnd.Next(1 + DificultyFactor, 11 + DificultyFactor)*DificultyFactor,
                                Experience = rnd.Next(1 + DificultyFactor, 11 + DificultyFactor)*DificultyFactor,
                                Attack = rnd.Next(1 + DificultyFactor, 11 + DificultyFactor)*DificultyFactor,
                                Dificulty = dificulty
                            });
                    DificultyFactor++;
                        
                }
                 
            }

            return monsters;
        }



        [HttpPost] //Create
        public IActionResult Create([FromBody] Monster monster)//metodo de criaçao com a annotation pra pegar a informação direto do body
                                                                //pegando um objeto monster do tipo Monstro
        {
            // monsters recebe o retorno do metodo AllMonsters que é a lista de montros
            var monsters = AllMonsters();

            //exists recebe nome do monstro caso informado no objeto monster, caso já exista um Monstro de nome igual dentro da lista
            //caso nao exista, a variável receberá o valor de null
            var exists = monsters.FirstOrDefault(m => m.Name == monster.Name);
            //se exists for nulo
            if (exists == null)
            {
                //monsters chama o metodo add e adiciona o objeto monster a lista, retornando mensagem de sucesso
                monsters.Add(monster);
                //com a normalização o API response criado por meio da succeed true, message de sucesso e resultas = monster por isso APIResponse<Monster>
                return Ok(new APIResponse<Monster>() { Succeed = true, Message = "Monstro criado com sucesso.", Results = monster});
            }
            //se exists nao for nulo
            else
            {
                //o objeto ja existe e nao será criado e será retornado a msg de erro.
                return NotFound(new APIResponse<string>() { Succeed = false, Message = "O Monstro informado já existe." });
            }

        }

        [HttpGet] //Read GetPadrao
        // metodo List() recebe metodo OK contendo metodo AllMonsters
        public IActionResult List() => Ok(new APIResponse<List<Monster>>() { Succeed = true, Results = AllMonsters() });


        [HttpGet]//Get por Id
        [Route("{id}")]//Adicionada a rota com o Id obrigatorio

        public IActionResult Get(int? id)
        {
            // monsters recebe o retorno do metodo AllMonsters que é a lista de monstros
            var monsters = AllMonsters();

            //exists recebe objeto, caso já exista um monstro de id igual dentro da lista comparando os ids da lista com o id recebido no metodo
            //caso id nao exista, a variável receberá o valor de null
            var exists = monsters.FirstOrDefault(m => m.Id == id);
            // se exists receber null entao retorna o notfound senão retorna o Ok
            return exists == null ?
                NotFound(new APIResponse<string>() { Succeed = false, Message = "O Monstro não existe." }) :
                Ok(new APIResponse<Monster>() { Succeed = true, Results = exists });

        }

        [HttpPut] //update
        [Route("{id}")]
        public IActionResult Update([FromBody] Monster monster)// pega as informações direto do body por causa da annotation, metodo recebe um objeto monster do tipo Monstro
        {
            // monsters recebe o retorno do metodo AllMonsters que é a lista de monstros
            var monsters = AllMonsters();

            //exists recebe objeto, caso id informado no como entrada do metodo já exista dentro da lista
            //caso id nao exista, a variável receberá o valor de null
            var exists = monsters.FirstOrDefault(m => m.Id == monster.Id);
            // se exists receber null entao retorna o notfound senão retorna o Ok
            return exists == null ?
                NotFound(new APIResponse<string>() { Succeed = false, Message = "O Monstro não existe." }) :
                Ok(new APIResponse<Monster>() { Succeed = true, Message = "Monstro atualizado com sucesso.", Results = monster });

        }

        [HttpDelete] //delete
        [Route("{id}")]

        public IActionResult Delete(int? id)
        {
            // monsters recebe o retorno do metodo AllMonsters que é a lista de monstros
            var monsters = AllMonsters();

            //exists recebe monstro de id igual ao informado por parametro, caso já exista na lista
            //caso id nao exista, a variável receberá o valor de null
            var exists = monsters.FirstOrDefault(m => m.Id == id);
            // se exists receber null entao retorna o notfound senão retorna o Ok
            return exists == null ?
                NotFound(new APIResponse<string>() { Succeed = false, Message = "O Monstro não existe." }) :
                Ok(new APIResponse<string>() { Succeed = true, Message = "O Monstro foi deletado com sucesso." });

        }


    }
}
