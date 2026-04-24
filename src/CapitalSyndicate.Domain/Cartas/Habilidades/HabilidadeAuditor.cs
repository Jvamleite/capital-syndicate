using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Tokens;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal class HabilidadeAuditor : IHabilidadeLider
    {
        void IHabilidadeLider.AoFimDoJogo(Jogador ativo, Partida partida)
        {
            List<TokenAuditoria> tokens = [.. ativo.Tokens.OfType<TokenAuditoria>()];
            ativo.PontosVitoria += tokens.Sum(t => t.Valor);
        }
    }
}