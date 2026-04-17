namespace CapitalSyndicate.Domain.Cartas
{
    internal class CartaCrise : Carta
    {
        public string? DescricaoCrise { get; set; }

        public override string Descricao()
        {
            return $"Crise revelada: {DescricaoCrise}";
        }
    }
}