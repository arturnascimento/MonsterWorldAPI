using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonsterWorldApi.Data;

namespace MonsterWorldApi
{
    /// <summary>
    /// Classe que principal do programa, que chama a iniciação.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Método que Builda a aplicação e chama o SeedData.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //aplicacao sendo montada para iniciar
            var app = CreateHostBuilder(args).Build();
            //enquanto eu estiver usando o escopo do app.services.createscope
            using (var scope = app.Services.CreateScope())
            {
                //buscar o provedor de serviços dentro do escopo
                var serviceProvider = scope.ServiceProvider;
                //seed inicando junto com o banco de dados
                SeedData.InitDatabase(serviceProvider);
            }
            //aplicacao vai comecar muhahaha
            app.Run();
        }

        /// <summary>
        /// Método que builda a aplicação e chama a StartUp.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
