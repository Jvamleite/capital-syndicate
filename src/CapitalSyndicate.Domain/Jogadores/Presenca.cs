using CapitalSyndicate.Domain.Mercados;

namespace CapitalSyndicate.Domain.Jogadores
{
    public class Presenca(Mercado mercado, Guid id)
    {
        public Guid Id { get; set; } = id;
        public int IndicePatamar { get; set; } = 0;
        public Mercado Mercado { get; set; } = mercado;

        public int Avancar() => IndicePatamar++;

        public void Resetar() => IndicePatamar = 0;
    }
}