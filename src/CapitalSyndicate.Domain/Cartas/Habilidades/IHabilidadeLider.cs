using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    public interface IHabilidadeLider
    {
        int ModificarEscalaParaAvanco(int escala) => escala;

        Task AposAvanco(Jogador ativo, Partida partida, Projeto projeto) { return Task.CompletedTask; }

        void AoFimDoTrimestre(Jogador ativo, Projeto projeto) { }

        void AoFimDoJogo(Jogador ativo, Partida partida) { }

        void AposLayoff(Jogador ativo, Partida partida, Projeto projeto) { }
    }
}