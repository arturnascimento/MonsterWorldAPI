using MonsterWorldApi.API;
using MonsterWorldApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonsterWorldApi.Services
{
    /// <summary>
    /// Serviço estático que foi utilizado no começo do desenvolvimento da API.
    /// </summary>
    public class StaticMonsterService: IMonsterService
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
                        HP = rnd.Next(1 + DificultyFactor, 11 + DificultyFactor) * DificultyFactor,
                        Experience = rnd.Next(1 + DificultyFactor, 11 + DificultyFactor) * DificultyFactor,
                        Attack = rnd.Next(1 + DificultyFactor, 11 + DificultyFactor) * DificultyFactor,
                        Dificulty = dificulty,
                        CreatedBy = "System",
                        UpdatedBy = default

                    });
                    DificultyFactor++;
                }
            }
            return monsters;
        }

        /// <summary>
        /// Método que busca a lista de monstros estática.
        /// </summary>
        /// <returns></returns>
        public List<Monster> Get() => AllMonsters();
        /// <summary>
        /// Método que busca um monstro por Id na lista estática.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Monster Get(int id) => AllMonsters().FirstOrDefault(m => m.Id == id);
        /// <summary>
        /// Método que busca um monstro pelo nivel de dificuldade retornando um monstro aleatório, monstro da lista estática.
        /// </summary>
        /// <param name="dificulty"></param>
        /// <returns></returns>
        public Monster Get(Dificulties dificulty) => AllMonsters().OrderBy(a => Guid.NewGuid()).FirstOrDefault(m => m.Dificulty == dificulty);
        /// <summary>
        /// Método de criação de monstro na lista estática.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool Create(Monster m) => AllMonsters().FirstOrDefault(monstro => monstro.Name == m.Name) == null;
        /// <summary>
        /// Método que realiza update em um monstro na lista estática.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool Update(Monster m) => AllMonsters().FirstOrDefault(monstro => monstro.Id == m.Id) != null;

        /// <summary>
        /// Método que exclui da lista estática um monstro escolhido pelo seu Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id) => AllMonsters().FirstOrDefault(m => m.Id == id) != null;

        /// <summary>
        /// Método pertence a interface porém não é utilizado para lista estática.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public List<Monster> Get(string role)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método implementado pela inteface, porém nao é utilizado nesta classe descontinuada.
        /// </summary>
        /// <param name="dificulty"></param>
        /// <returns></returns>
        public List<Monster> GetFight(Dificulties dificulty)
        {
            throw new NotImplementedException();
        }
    }
}
