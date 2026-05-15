using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal class HabilidadeNegociadorInternacional : IHabilidadeLider
    {
        async Task IHabilidadeLider.AposAvanco(Jogador ativo, Partida partida, Projeto projeto)
        {
            if (partida.TrilhaGlobal is null)
            {
                throw new InvalidOperationException(
                    "A Trilha Global não está ativa nesta partida.");
            }

            await partida.TrilhaGlobal.AvancarPresenca(ativo, partida, projeto.Escala);
        }
    }
}