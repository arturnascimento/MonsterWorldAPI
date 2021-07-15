using MonsterWorldApi.API;
using MonsterWorldApi.Models;
using System.Collections.Generic;


namespace MonsterWorldApi.Services
{
    /// <summary>
    /// Interface que contém os métodos do CRUD dos Monstros.
    /// </summary>
    public interface IMonsterService
    {
        /// <summary>
        /// Método da interface para criação de um novo monstro.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        bool Create(Monster m); //bool de criação recebe objeto m do tipo Monster, caso true
        /// <summary>
        /// Método da interface para exclusão de um monstro, com o Id como parâmetro.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id); // bool delete para deletar com o parametro de id, caso true
        /// <summary>
        /// Método da interface que retorna a lista completa de monstros cadastrados.
        /// </summary>
        /// <returns></returns>
        List<Monster> Get(); // get geral, pegando toda a lista de objetos do tipo monstro
        /// <summary>
        /// Método da interface que retorna um monstro tendo seu Id como parâmetro.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Monster Get(int id); // get por id pegando objeto do tipo monstro com o id igual ao solicitado, caso exista
        /// <summary>
        /// Método da interface que retorna monstro com sua dificuldade como parâmetro.
        /// </summary>
        /// <param name="dificulty"></param>
        /// <returns></returns>
        Monster Get(Dificulties dificulty); //get solicitado no desafio, onde será exibido objeto do tipo Monster com a Dificuldade solicitada.
        /// <summary>
        /// Método da interface que realiza edição no monstro informado pelo usuário através do Id.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        bool Update(Monster m); // bool de update, funciona tipo o create e o delete, só executa o update em caso de true
        /// <summary>
        /// Método da interface que retorna uma lista de monstros que tem a role do usuário que o criou como parâmetro.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        List<Monster> Get(string role);

        /// <summary>
        /// Get especial para gerar um duelo entre 2 montros de mesma dificuldade.
        /// </summary>
        /// <param name="dificulty"></param>
        /// <returns></returns>
        List<Monster> GetFight(Dificulties dificulty);



    }
}
