using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal class HabilidadeNegociadorInternacional : IHabilidadeLider
    {
        void IHabilidadeLider.AposAvanco(Jogador ativo, Partida partida, Projeto projeto)
        {
            if (partida.TrilhaGlobal is null)
            {
                throw new InvalidOperationException(
                    "A Trilha Global não está ativa nesta partida.");
            }

            partida.TrilhaGlobal.AvancarPresenca(ativo, partida, projeto.Escala);
        }
    }
}