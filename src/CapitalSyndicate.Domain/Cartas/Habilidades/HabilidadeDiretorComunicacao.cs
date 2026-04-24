using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal class HabilidadeDiretorComunicacoes : IHabilidadeLider
    {
        void IHabilidadeLider.AoFimDoTrimestre(Jogador ativo, Projeto projeto)
        {
            for (int i = 0; i < projeto.Profissionais.Count; i++)
            {
                ativo.PontosVitoria++;
            }
        }
    }
}