using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Cartas.Habilidades
{
    internal interface IHabilidadeLider
    {
        // POS-AVANCO: GestorRH (layoff modificado)
        void AposAvanco(Jogador ativo, Partida partida, Projeto projeto) { }

        void AoFimDoTrimestre(Jogador ativo, Projeto projeto) { }

        void AoFimDoJogo(Jogador ativo, Partida partida) { }
    }
}