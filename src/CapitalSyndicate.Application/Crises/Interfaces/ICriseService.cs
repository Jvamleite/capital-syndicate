using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Application.Crises.Interfaces
{
    public interface ICriseService
    {
        bool Resolver(CartaCrise crise, Jogador jogador, Partida partida);
    }
}