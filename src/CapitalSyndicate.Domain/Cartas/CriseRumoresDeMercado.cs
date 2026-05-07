using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Cartas
{
    internal class CriseRumoresDeMercado : CartaCrise
    {
        public override void Resolver(Jogador jogadorQueRevelou, Partida partida)
        {
            partida.Baralho.DescartarCartasDoMercadoDeTalentos();
            List<Carta> cartasParaComprar = [];
            for (int i = 0; i < partida.Jogadores.Count + 2; i++)
            {
                cartasParaComprar.Add(partida.Baralho.ComprarCartaDoMonte());
            }

            partida.Baralho.AdicionarCartasNoMercadoDeTalentos(cartasParaComprar);
        }

        public override string Descricao() => "Crise: Rumores de Mercado";
    }
}