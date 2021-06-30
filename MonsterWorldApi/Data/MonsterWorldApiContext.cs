using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonsterWorldApi.Models;


namespace MonsterWorldApi.Data
{
    public class MonsterWorldApiContext: IdentityDbContext
    {
        public MonsterWorldApiContext(DbContextOptions options) : base(options) { }

        public DbSet<Monster> Monster { get; set; } //banco de dados recebe Monster do tipo Monster
    }
}
