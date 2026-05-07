using CapitalSyndicate.Domain.Enums;

namespace CapitalSyndicate.Domain.Cartas
{
    public class CartaProfissional : Carta
    {
        public Setor Setor { get; set; }
        public Cargo Cargo { get; set; }

        public bool PodeSerGerente()
        {
            return Cargo != Cargo.ConsultorExterno && Cargo != Cargo.Trainee;
        }

        public override string Descricao()
        {
            return $"Profissional: {Cargo} - {Setor}";
        }
    }
}