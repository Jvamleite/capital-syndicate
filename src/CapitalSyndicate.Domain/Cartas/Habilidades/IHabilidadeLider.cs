using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    public interface IHabilidadeLider
    {
        int ModificarEscalaParaAvanco(int escala) => escala;

        void AposAvanco(Jogador ativo, Partida partida, Projeto projeto) { }

        void AoFimDoTrimestre(Jogador ativo, Projeto projeto) { }

        void AoFimDoJogo(Jogador ativo, Partida partida) { }
    }
}