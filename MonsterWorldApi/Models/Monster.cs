using MonsterWorldApi.API;

namespace MonsterWorldApi.Models
{
    public class Monster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HP { get; set; }
        public int Experience { get; set; }
        public int Attack { get; set; }
        public Dificulties Dificulty { get; set; }

    }
}
