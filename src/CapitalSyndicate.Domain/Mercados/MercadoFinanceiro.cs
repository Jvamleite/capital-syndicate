namespace CapitalSyndicate.Domain.Mercados
{
    internal class MercadoFinanceiro : Mercado
    {
        public bool CashOutDisponivel { get; }

        public MercadoFinanceiro(bool avancado)
        {
            Nome = avancado ? "Bolsa de Valores" : "Especulação/Cash Out";
            Patamares = CriarPatamares(avancado);
            CashOutDisponivel = avancado;
        }

        protected override List<Patamar> CriarPatamares(bool avancado)
        {
            if (!avancado)
            {
                int[] pontos = [-2, 2, 0, 6, 3, 10, 5, 15];
                int[] requisitos = [1, 2, 1, 3, 1, 4, 1, 4];
                foreach (int i in Enumerable.Range(0, pontos.Length))
                {
                    Patamares.Add(new Patamar(requisitos[i], pontos[i]));
                }
            }
            else
            {
                int[] pontos = [1, 3, 6, 10, 15, 20];
                for (int i = 1; i <= 6; i++)
                {
                    Patamares.Add(new Patamar(i, pontos[i - 1]));
                }
            }

            return Patamares;
        }
    }
}