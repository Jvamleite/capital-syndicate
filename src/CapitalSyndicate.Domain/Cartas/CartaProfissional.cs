using CapitalSyndicate.Domain.Enums;

namespace CapitalSyndicate.Domain.Cartas
{
    internal class CartaProfissional : Carta
    {
        public Cargo Cargo { get; set; }

        public bool PodeSerLider()
        {
            return Cargo != Cargo.CONSULTOR_EXT;
        }

        public override string Descricao()
        {
            return $"Profissional: {Cargo} - {Setor}";
        }
    }
}