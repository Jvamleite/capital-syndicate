using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Domain.Cartas
{
    public abstract class CartaCrise : Carta
    {
        public abstract Task Resolver(Jogador jogadorQueRevelou, Partida partida);
    }
}