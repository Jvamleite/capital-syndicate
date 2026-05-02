using CapitalSyndicate.Domain.Jogadores;

namespace CapitalSyndicate.Domain.Partidas
{
    public record Turno
    {
        public Jogador Jogador { get; }
        public bool Encerrado { get; private set; }

        public Turno(Jogador jogador)
        {
            Jogador = jogador;
        }

        public void Encerrar() => Encerrado = true;
    }
}