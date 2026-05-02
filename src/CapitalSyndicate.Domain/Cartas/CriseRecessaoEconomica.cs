using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Cartas
{
    internal class CriseRecessaoEconomica : CartaCrise
    {
        public override string Descricao() => "Crise: Recessão Econômica";

        public override void Resolver(Jogador jogadorQueRevelou, Partida partida)
        {
            foreach (var jogador in partida.Jogadores)
            {
                if (jogador.CartasNaMao.Count == 0)
                    continue;

                var carta = partida.Entrada.EscolherCartaParaMercadoDeTalentos(jogador);
                jogador.CartasNaMao.Remove(carta);
                partida.Baralho.AdicionarCartasNoMercadoDeTalentos([carta]);
            }
        }
    }
}