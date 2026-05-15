using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Application.Turnos.Interfaces
{
    public interface ITurnoService
    {
        IEnumerable<Carta> ComprarCartas(int quantidade, Partida partida);

        Task<IEnumerable<Carta>> ExecutarProjeto(Projeto projeto, Partida partida);
    }
}