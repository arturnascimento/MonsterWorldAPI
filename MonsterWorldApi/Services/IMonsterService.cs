using MonsterWorldApi.API;
using MonsterWorldApi.Models;
using System.Collections.Generic;


namespace MonsterWorldApi.Services
{
    public interface IMonsterService
    {
        bool Create(Monster m); //bool de criação recebe objeto m do tipo Monster, caso true
        bool Delete(int id); // bool delete para deletar com o parametro de id, caso true
        List<Monster> Get(); // get geral, pegando toda a lista de objetos do tipo monstro
        Monster Get(int id); // get por id pegando objeto do tipo monstro com o id igual ao solicitado, caso exista
        Monster Get(Dificulties dificulty); //get solicitado no desafio, onde será exibido objeto do tipo Monster com a Dificuldade solicitada.
        bool Update(Monster m); // bool de update, funciona tipo o create e o delete, só executa o update em caso de true
        

    }
}
