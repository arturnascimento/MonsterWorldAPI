using Microsoft.AspNetCore.Identity;
using MonsterWorldApi.API;
using MonsterWorldApi.Data;
using MonsterWorldApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MonsterWorldApi.Services
{
    /// <summary>
    /// Serviço responsável por gerenciar os monstros e salvar as informações no banco de dados.
    /// </summary>
    public class SqlMonsterService : IMonsterService
    {
        //injecao de dependencia
        MonsterWorldApiContext _context;
        ILogger _logger;

        /// <summary>
        /// lista utilizada na batalha de monstros.
        /// </summary>
        List<Monster> monstrinhos = new();
        


        /// <summary>
        /// Construtor da classe SqlMonsterService
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public SqlMonsterService(MonsterWorldApiContext context, ILogger<SqlMonsterService> logger)//construtor do serviço com contexto do banco de dados
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Método responsável por criar um monstro e salvar no banco de dados
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool Create(Monster m)//cria objeto m tipo Monster e add na lista Monster de objetos do tipo Monster e salva no banco retornando true
        {
            _logger.LogInformation($"Tentando criar um monstro {nameof(Create)} as {DateTime.Now}");
            try
            {
                _context.Monster.Add(m);
                _context.SaveChanges();
                _logger.LogInformation($"Monstro criado {nameof(Create)} as {DateTime.Now}");
                return true;
            }
            catch
            {
                _logger.LogError($"Monstro não foi criado {nameof(Create)} as {DateTime.Now}");
                return false; // se nao salvar ele retorna false
            }
        }

        /// <summary>
        /// Método responsável por deletar um monstro do banco de dados, caso ele exista.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)//deleta objeto com id de parametro
        {
            _logger.LogInformation($"Tentando deletar um monstro {nameof(Delete)} as {DateTime.Now}");
            try
            {
                _context.Monster.Remove(Get(id)); //vai na lista do context e usa o metodo remove apos buscar por id
                _context.SaveChanges(); // salva a alteracao no banco
                _logger.LogInformation($"Monstro {nameof(Delete)} as {DateTime.Now}");
                return true; // retorna trre com a operacao ok
            }
            catch
            {
                _logger.LogError($"Monstro não foi deletado {nameof(Delete)} as {DateTime.Now}");
                return false; // se nao deletar retorna false
            }
        }

        /// <summary>
        /// Método responsável por consultar no banco de dados uma lista de monstros por role de criação, onde foi implementada esta Query para obtenção dos resultados esperados.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public List<Monster> Get(string role) //get recebe string da role dos usuarios e consegue fazer a lista atraves da query abaixo
        {
            _logger.LogInformation($"Pegando uma lista de monstros pela role do usuário que o criou {nameof(Get)} as {DateTime.Now}");
            var query = from m in _context.Set<Monster>() //m como tabela Monster
                        from u in _context.Set<IdentityUser>() // u como tabela IdentityUser
                        from urole in _context.Set<IdentityUserRole<string>>()//urole como tabela IdentityUserRole
                        from r in _context.Set<IdentityRole>()// r como tabela IdentityRole
                        where m.CreatedBy == u.UserName && u.Id == urole.UserId && urole.RoleId == r.Id && r.Name == role
                        //vai selecionar os monstros da tabela m onde o nome do usuario q o criou for igual ao nome de usuario da tabela u &&
                        //id na tabela user for igual a userId da tabela IdentityUserRole &&
                        //RoleId da tabela IdentityUserRole for igual a Id na tabela IdentityRole &&
                        //Name na tabela IdentityRole for igual string informada no get
                        select new Monster() //criando um novo monstro para exibição
                        {
                            Id = m.Id,
                            Name = m.Name,
                            HP = m.HP,
                            Experience = m.Experience,
                            Attack = m.Attack,
                            Dificulty = m.Dificulty,
                            CreatedBy = m.CreatedBy,
                            UpdatedBy = m.UpdatedBy


                        };
            _logger.LogInformation($"Obtendo a lista de monstros por role de usuário que o criou {nameof(Get)} as {DateTime.Now}");
            return query.ToList(); // retornando os monstros criados em uma lista
           
        }

        /// <summary>
        /// Método responsável por retornar a lista de monstros existente no banco de dados
        /// </summary>
        /// <returns></returns>
        public List<Monster> Get()
        {
            _logger.LogInformation($"Pegando todos os monstros do banco de dados {nameof(Get)} as {DateTime.Now}");
            return _context.Monster.ToList(); 
        } // get padrao retorna a lista inteira de monstros


        /// <summary>
        /// Método responsável por retornar um determinado monstro existente no banco de dados.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Monster Get(int id)
        {
            _logger.LogInformation($"Pegando um monstro no banco de dados passando o id como referência{nameof(Get)} as {DateTime.Now}");
            return _context.Monster.FirstOrDefault(m => m.Id == id);
        }// get por id, retorna o primeiro objeto com o id = ao id do parametro
        /// <summary>
        /// Método que retorna um monstro aletório do banco de dados de uma determinada dificuldade.
        /// </summary>
        /// <param name="dificulty"></param>
        /// <returns></returns>
        public Monster Get(Dificulties dificulty)
        {

            _logger.LogInformation($"Pegando um monstro aleatório com a dificuldade informada pelo usuário {nameof(Get)} as {DateTime.Now}");
            var monsterRandom = _context.Monster.OrderBy(a => Guid.NewGuid()).FirstOrDefault(m => m.Dificulty == dificulty);
            return monsterRandom;


            //get do desafio utilizando dificulty do tipo Enum Dificulties
            //utilizei o orderby na lista de monster onde a recebe a nova organização da lista atraves do metodo Guid.NewGuid() que a cada get muda a ordem dos itens
        }//pegando o primeiro objeto com atributo Dificulty == ao dificulty do parametro apesar de pegar sempre o primeiro objeto, a cada get o primeiro objeto vai ser aleatorio.
         
        /// <summary>
        /// Método responsável pelo update de um monstro do banco de dados e salvar as informações alteradas no banco de dados.
        /// </summary>
        /// <param name="monster"></param>
        /// <returns></returns>
        public bool Update(Monster monster)//update funciona tipo o dele e create e sabemos o monstro certo por causa da rota que eh por id.
        {
            _logger.LogInformation($"Tentando realizar um update em um monstro com a id informada pelo usuário {nameof(Update)} as {DateTime.Now}");
            try
            {
                _logger.LogInformation($"Pegando o monstro informado no banco de dados {nameof(Get)} as {DateTime.Now}");
                var exists = Get(monster.Id); //faz o get do monstro a ser atualizado
                if (exists == null)
                {
                    _logger.LogError($"Monstro informado não existe no banco de dados{nameof(Get)} as {DateTime.Now}");
                    return false; 
                } // se exists for null da false se for true ele pega os atributos do monstro e atualiza ele, sem perder a origem da criação
                exists.Name = monster.Name; //name recebido pelo body passa pro objeto exists
                exists.Attack = monster.Attack; //attack recebido pelo body passa pro objeto exists
                exists.Dificulty = monster.Dificulty; // dificuldade recebida pelo body passa pro objeto exists
                exists.UpdatedBy = monster.UpdatedBy; //updated.by do monster é inputado o nome do usuário pego na controller e compondo o objeto monster q vai passar a informação pra exists
                exists.HP = monster.HP; //hp recebido pelo body e passa pro objeto exists
                _context.Monster.Update(exists); //faz o update no objeto exists do tipo Monster justamente pra nao perder o CreatedBy da Origem
                _context.SaveChanges(); //salva as alteraçoes no banco
                _logger.LogInformation($"Monstro editado pelo usuário {nameof(Update)} as {DateTime.Now}");
                return true; // retorna true
            }
            catch
            {
                _logger.LogError($"Erro ao editar o monstro {nameof(Update)} as {DateTime.Now}");
                return false; // retorna false caso nao tenha feiro o update
            }
        }

        /// <summary>
        /// Método que retorna 2 monstros aleatórios de mesma dificuldade e repete o retorno do vencedor do duelo em uma lista de monstros.
        /// </summary>
        /// <param name="dificulty"></param>
        /// <returns></returns>
        public List<Monster> GetFight(Dificulties dificulty)
        {
            
            _logger.LogInformation($"Pegando um monstro aleatório com a dificuldade informada pelo usuário {nameof(Get)} as {DateTime.Now}");
            var monsterRandom = _context.Monster.OrderBy(a => Guid.NewGuid()).FirstOrDefault(m => m.Dificulty == dificulty);
            var monsterRandom2 = _context.Monster.OrderBy(a => Guid.NewGuid()).LastOrDefault(m => m.Dificulty == dificulty);
            if(monsterRandom == monsterRandom2)
            {
                monsterRandom = _context.Monster.OrderBy(a => Guid.NewGuid()).FirstOrDefault(m => m.Dificulty == dificulty);
                monsterRandom2 = _context.Monster.OrderBy(a => Guid.NewGuid()).LastOrDefault(m => m.Dificulty == dificulty);
            }

            monstrinhos.Add(monsterRandom2);
            monstrinhos.Add(monsterRandom);
            Monster monsterWinner = new Monster();

            if(monsterRandom.Attack + monsterRandom.HP > monsterRandom2.Attack + monsterRandom2.HP)
            {
                monsterWinner = monsterRandom;
                monstrinhos.Add(monsterWinner);
            }
            else
            {
                monsterWinner = monsterRandom2;
                monstrinhos.Add(monsterWinner);
            }
          
            return monstrinhos;

        }
          
    }
}

