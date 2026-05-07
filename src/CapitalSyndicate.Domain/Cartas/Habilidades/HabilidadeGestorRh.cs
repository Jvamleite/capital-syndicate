using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal class HabilidadeGestorRh : IHabilidadeLider
    {
        void IHabilidadeLider.AposAvanco(Jogador ativo, Partida partida, Projeto projeto)
        {
            int cartasAManter = partida.Entrada.EscolherNumManterCartas(ativo, partida, projeto.Escala);

            cartasAManter = Math.Min(cartasAManter, projeto.Escala);

            int cartasADescartar = ativo.CartasNaMao.Count - cartasAManter;
            if (cartasADescartar <= 0)
            {
                return;
            }

            List<Carta> descartadas = [.. ativo.CartasNaMao.Take(cartasADescartar)];
            ativo.CartasNaMao.RemoveRange(0, cartasADescartar);
            partida.Baralho.AdicionarCartasNoMercadoDeTalentos(descartadas);
        }
    }
}