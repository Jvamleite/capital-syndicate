using CapitalSyndicate.Domain.Baralhos;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Ports;

namespace CapitalSyndicate.Domain.Partidas
{
    internal class Partida
    {
        public List<Jogador> Jogadores { get; }
        public Baralho Baralho { get; }
        public IEntradaJogador Entrada { get; }
    }
}