using ApiCatalogoJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private static Dictionary<Guid, Jogo> jogos = new Dictionary<Guid, Jogo>()
        {
        {Guid.Parse("5e99c84a-108b-4dfa-ab7e-d8c55957a7ec"), new Jogo{Id = Guid.Parse("5e99c84a-108b-4dfa-ab7e-d8c55957a7ec"), Nome = "BattleField 2042", Produtora = "DICE", Preco = 349} },
        {Guid.Parse("123e4567-e89b-12d3-a456-426655440000"), new Jogo{Id = Guid.Parse("123e4567-e89b-12d3-a456-426655440000"), Nome = "No Man's Sky", Produtora = "Hello Games", Preco = 159} },
        {Guid.Parse("e8f1e5df-5517-459f-9ad6-8465de68749d"), new Jogo{Id = Guid.Parse("e8f1e5df-5517-459f-9ad6-8465de68749d"), Nome = "Call Of Duty - Warzone", Produtora = "Activision", Preco = 149} },
        {Guid.Parse("f6aee24e-7184-464b-8ec3-4c29386db0b7"), new Jogo{Id = Guid.Parse("f6aee24e-7184-464b-8ec3-4c29386db0b7"), Nome = "Conan Exiles - Isle of Siptah", Produtora = "Funcom", Preco = 99} },
        {Guid.Parse("9a842d57-ff4a-4d53-af03-d801693cf2d1"), new Jogo{Id = Guid.Parse("9a842d57-ff4a-4d53-af03-d801693cf2d1"), Nome = "Far Cry 6", Produtora = "Ubisoft", Preco = 399} },
        {Guid.Parse("c845e509-d635-4df3-95c2-64309343f0ee"), new Jogo{Id = Guid.Parse("c845e509-d635-4df3-95c2-64309343f0ee"), Nome = "Assassin's Creed - Valhalla", Produtora = "Ubisoft", Preco = 139} },
        };


        //Logica com link para fazer a paginação
        public Task<List<Jogo>> Obter(int pagina, int quantidade) 
        {
            return Task.FromResult(jogos.Values.Skip((pagina - 1) * quantidade).Take(quantidade).ToList());
        }

        public Task<Jogo> Obter(Guid id)
        {
            if (!jogos.ContainsKey(id))
                return null;

            return Task.FromResult(jogos[id]);
        }

        public Task<List<Jogo>> Obter(string nome, string produtora)
        {
            return Task.FromResult(jogos.Values.Where(jogo => jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora)).ToList());

        }
        //Obter = ObterSemLambda Mesmo resultado
        public Task<List<Jogo>> ObterSemLambda(string nome, string produtora)
        {
            var retorno = new List<Jogo>();

            foreach(var jogo in jogos.Values)
            {
                if (jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora))
                    retorno.Add(jogo);
            }
            return Task.FromResult(retorno);
        }

        public Task Inserir(Jogo jogo) {
            jogos.Add(jogo.Id, jogo);
            return Task.CompletedTask;
        }

        public Task Atualizar(Jogo jogo)
        {
            jogos[jogo.Id] = jogo;
            return Task.CompletedTask;
        }

        public Task Remover(Guid id)
        {
            jogos.Remove(id);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //Fecha a conexão com o BD
        }
    }
    
}
