namespace CapitalSyndicate.Domain.Cartas
{
    public abstract class Carta
    {
        public Guid Id { get; set; }

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