using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Cartas
{
    internal class CriseRumoresDeMercado : CartaCrise
    {
        public override void Resolver(Jogador jogadorQueRevelou, Partida partida)
        {
            partida.Baralho.DescartarCartasDoMercadoDeTalentos();
            partida.Baralho.AdicionarCartasNoMercadoDeTalentos(
                [.. partida.Baralho.ComprarCartaDoMonte()]
            );
        }

        public override string Descricao() => "Crise: Rumores de Mercado";
    }
}