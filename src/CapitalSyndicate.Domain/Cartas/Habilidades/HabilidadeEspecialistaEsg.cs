using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Projetos;
using CapitalSyndicate.Domain.Tokens;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal class HabilidadeEspecialistaEsg : IHabilidadeLider
    {
        void IHabilidadeLider.AoFimDoTrimestre(Jogador ativo, Projeto projeto)
        {
            List<TokenEsg> tokens = [.. ativo.Tokens.OfType<TokenEsg>()];
            ativo.PontosVitoria += tokens.Sum(t => t.ObterPontuacao());
            tokens.ForEach(t => ativo.Tokens.Remove(t));
        }
    }
}