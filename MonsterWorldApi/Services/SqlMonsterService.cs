using MonsterWorldApi.API;
using MonsterWorldApi.Data;
using MonsterWorldApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonsterWorldApi.Services
{
    public class SqlMonsterService : IMonsterService
    {
        //injecao de dependencia
        MonsterWorldApiContext _context; 
        public SqlMonsterService(MonsterWorldApiContext context)//construtor do serviço com contexto do banco de dados
        {
            _context = context;
        }

        public bool Create(Monster m)//cria objeto m tipo Monster e add na lista Monster de objetos do tipo Monster e salva no banco retornando true
        {
            try
            {
                _context.Monster.Add(m);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false; // se nao salvar ele retorna false
            }
        }

        public bool Delete(int id)//deleta objeto com id de parametro
        {
            try
            {
                _context.Monster.Remove(Get(id)); //vai na lista do context e usa o metodo remove apos buscar por id
                _context.SaveChanges(); // salva a alteracao no banco
                return true; // retorna trre com a operacao ok
            }
            catch
            {
                return false; // se nao deletar retorna false
            }
        }

        public List<Monster> Get() => _context.Monster.ToList(); // get padrao retorna a lista inteira de monstros

        public Monster Get(int id) => _context.Monster.FirstOrDefault(m => m.Id == id); // get por id, retorna o primeiro objeto com o id = ao id do parametro
        public Monster Get(Dificulties dificulty) => _context.Monster.OrderBy(a => Guid.NewGuid()).FirstOrDefault(m => m.Dificulty == dificulty);
        //get do desafio utilizando dificulty do tipo Enum Dificulties
        //utilizei o orderby na lista de monster onde a recebe a nova organização da lista atraves do metodo Guid.NewGuid() que a cada get muda a ordem dos itens
        //pegando o primeiro objeto com atributo Dificulty == ao dificulty do parametro apesar de pegar sempre o primeiro objeto, a cada get o primeiro objeto vai ser aleatorio.    
        public bool Update(Monster m)//update funciona tipo o dele e create e sabemos o monstro certo por causa da rota que eh por id.
        {
            try
            {
                _context.Monster.Update(m); //faz o update no objeto m do tipo Monster
                _context.SaveChanges(); //salava as alteraçoes no banco
                return true; // retorna true
            }
            catch
            {
                return false; // retorna false caso nao tenha feiro o update
            }
        }
    }
}

