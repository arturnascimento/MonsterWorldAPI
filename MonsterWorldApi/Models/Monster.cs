
using MonsterWorldApi.API;

namespace MonsterWorldApi.Models
{
    /// <summary>
    /// Classe Modelo dos atributos de um objeto do tipo Monster.
    /// </summary>
    public class Monster
    {
        /// <summary>
        /// Id do monstro
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nome do monstro
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Hp do monstro
        /// </summary>
        public int HP { get; set; }
        /// <summary>
        /// Experiencia do monstro
        /// </summary>
        public int Experience { get; set; }
        /// <summary>
        /// Attack do monstro
        /// </summary>
        public int Attack { get; set; }
        /// <summary>
        /// Dificuldado do monstro do tipo enum
        /// </summary>
        public Dificulties Dificulty { get; set; }
        /// <summary>
        /// Usuário que criou o monstro, e caso o seed tenha criado, vem como "System"
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Usuário que fez o update no monstro.
        /// </summary>
        public string UpdatedBy { get; set; }

    }
}
