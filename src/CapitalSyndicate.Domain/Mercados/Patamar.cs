namespace CapitalSyndicate.Domain.Mercados
{
    public class Patamar
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