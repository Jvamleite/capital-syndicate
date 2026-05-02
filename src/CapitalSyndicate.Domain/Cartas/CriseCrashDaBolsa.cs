using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Cartas
{
    internal class CriseCrashDaBolsa : CartaCrise
    {
        public override string Descricao() => "Crise: Crash da Bolsa";

        public override void Resolver(Jogador jogadorQueRevelou, Partida partida)
        {
            partida.ObterTrimestreAtual().Encerrar(partida);
        }
    }
}