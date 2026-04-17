using CapitalSyndicate.Domain.Enums;

namespace CapitalSyndicate.Domain.Cartas
{
    internal abstract class Carta
    {
        public Guid Id { get; set; }
        public Setor Setor { get; set; }

        public abstract string Descricao();

        public override bool Equals(object? obj)
        {
            if (obj is not Carta other)
            {
                return false;
            }

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}