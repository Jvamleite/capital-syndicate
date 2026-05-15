using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal class HabilidadeInvestidor : IHabilidadeLider
    {
        void IHabilidadeLider.AposLayoff(Jogador ativo, Partida partida, Projeto projeto)
        {
            int escalaProjeto = projeto.Escala;
            List<Carta> cartasCompradas = [];
            for (int i = 0; i < escalaProjeto; i++)
            {
                cartasCompradas.Add(partida.Baralho.ComprarCartaDoMonte());
            }

            ativo.CartasNaMao.AddRange(cartasCompradas);
        }
    }
}