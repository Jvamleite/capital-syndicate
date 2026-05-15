using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Cartas
{
    public class CriseRecessaoEconomica : CartaCrise
    {
        public override string Descricao() => "Crise: Recessão Econômica";

        public override async Task Resolver(Jogador jogadorQueRevelou, Partida partida)
        {
            foreach (Jogador jogador in partida.Jogadores)
            {
                if (jogador.CartasNaMao.Count == 0)
                {
                    continue;
                }

                Carta carta = await partida.Entrada.EscolherCartaParaMercadoDeTalentos(jogador);
                jogador.CartasNaMao.Remove(carta);
                partida.Baralho.AdicionarCartasNoMercadoDeTalentos([carta]);
            }
        }
    }
}