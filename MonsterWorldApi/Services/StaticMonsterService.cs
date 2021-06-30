using MonsterWorldApi.API;
using MonsterWorldApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonsterWorldApi.Services
{
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

        public List<Monster> Get() => AllMonsters();

        public Monster Get(int id) => AllMonsters().FirstOrDefault(m => m.Id == id);

        public Monster Get(Dificulties dificulty) => AllMonsters().OrderBy(a => Guid.NewGuid()).FirstOrDefault(m => m.Dificulty == dificulty);
        public bool Create(Monster m) => AllMonsters().FirstOrDefault(monstro => monstro.Name == m.Name) == null;

        public bool Update(Monster m) => AllMonsters().FirstOrDefault(monstro => monstro.Id == m.Id) != null;

        public bool Delete(int id) => AllMonsters().FirstOrDefault(m => m.Id == id) != null;
    }
}
