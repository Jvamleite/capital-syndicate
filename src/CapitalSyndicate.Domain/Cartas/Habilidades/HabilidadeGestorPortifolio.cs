using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Tokens;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal class HabilidadeGestorPortifolio : IHabilidadeLider
    {
        void IHabilidadeLider.AoFimDoJogo(Jogador ativo, Partida partida)
        {
            List<TokenAtivo> tokens = [.. ativo.Tokens.OfType<TokenAtivo>()];
            ativo.PontosVitoria += TokenAtivo.PontuarPortfolio(tokens);
        }
    }
}