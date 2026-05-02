namespace CapitalSyndicate.Domain.Mercados
{
    internal class Patamar
    {
        public int Requisito { get; set; }
        public int Pontos { get; set; }

        public Patamar(int requisito, int pontos)
        {
            Requisito = requisito;
            Pontos = pontos;
        }
    }
}