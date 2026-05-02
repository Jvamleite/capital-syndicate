using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal class HabilidadeInvestidor : IHabilidadeLider
    {
        void IHabilidadeLider.AposAvanco(Jogador ativo, Partida partida, Projeto projeto)
        {
            int escalaProjeto = projeto.Escala;
            IEnumerable<Carta> cartasBaralho = partida.Baralho.ComprarCartaDoMonte(escalaProjeto);
            ativo.CartasNaMao.AddRange(cartasBaralho);
        }
    }
}