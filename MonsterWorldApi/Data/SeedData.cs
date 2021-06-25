using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MonsterWorldApi.API;
using MonsterWorldApi.Models;
using System;
using System.Linq;


namespace MonsterWorldApi.Data
{
    public class SeedData
    {
        public static void InitDatabase(IServiceProvider serviceProvider)
        {
            //usando esse context de forma temporária no seed para nao ocupar espaco em memoria apos o termino do using
            using (var context = new MonsterWorldApiContext(
                    serviceProvider.GetRequiredService<DbContextOptions<MonsterWorldApiContext>>()
                )
            )
            {
                context.Database.Migrate();

                if (!context.Monster.Any())
                {
                    //rnd foi um random pra gerar os atributos dos monstros
                    Random rnd = new Random();

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
                            context.Monster.Add(new Monster
                            {
                                
                                Name = dificulty + " " + monsternames[i],
                                HP = rnd.Next(1 + DificultyFactor, 11 + DificultyFactor) * DificultyFactor,
                                Experience = rnd.Next(1 + DificultyFactor, 11 + DificultyFactor) * DificultyFactor,
                                Attack = rnd.Next(1 + DificultyFactor, 11 + DificultyFactor) * DificultyFactor,
                                Dificulty = dificulty
                            });
                            DificultyFactor++;
                        }
                    }
                    context.SaveChanges();
                }
  
            }
        }
    }
}
