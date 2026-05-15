using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Cartas
{
    public class CriseCrashDaBolsa : CartaCrise
    {
        public override string Descricao() => "Crise: Crash da Bolsa";

        public override async Task Resolver(Jogador jogadorQueRevelou, Partida partida)
        {
            partida.ObterTrimestreAtual().Encerrar(partida);
        }
    }
}