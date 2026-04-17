using CapitalSyndicate.Domain.Enums;

namespace CapitalSyndicate.Domain.Cartas
{
    internal abstract class Carta
    {
        public Guid Id { get; set; }
        public Setor Setor { get; set; }

        public abstract string Descricao();
    }
}