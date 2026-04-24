using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    public interface IHabilidadeLider
    {
        void AposAvanco(Jogador ativo, Partida partida, Projeto projeto) { }

        void AoFimDoTrimestre(Jogador ativo, Projeto projeto) { }

        void AoFimDoJogo(Jogador ativo, Partida partida) { }
    }
}