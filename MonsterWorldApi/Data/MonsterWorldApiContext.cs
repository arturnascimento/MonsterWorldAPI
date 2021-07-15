using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonsterWorldApi.Models;


namespace MonsterWorldApi.Data
{
    /// <summary>
    /// Classe responsável por Gerenciar o que pode ser salvo no banco de dados.
    /// </summary>
    public class MonsterWorldApiContext: IdentityDbContext
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="options"></param>
        public MonsterWorldApiContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Dados que vão para o banco de dados.
        /// </summary>
        public DbSet<Monster> Monster { get; set; } //banco de dados recebe Monster do tipo Monster
    }
}
