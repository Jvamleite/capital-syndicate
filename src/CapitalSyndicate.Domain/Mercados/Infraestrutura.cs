namespace CapitalSyndicate.Domain.Mercados
{
    internal class Infraestrutura : Mercado
    {
        public bool Monopolio { get; }

        public Infraestrutura(bool avancado)
        {
            Nome = avancado ? "Telecomunicações" : "Concessões Públicas";
            Patamares = CriarPatamares(avancado);
            Monopolio = avancado;
        }

        protected override List<Patamar> CriarPatamares(bool avancado)
        {
            if (!avancado)
            {
                int[] pontos = [0, 0, 4, 7, 9, 12];
                for (int i = 1; i <= 6; i++)
                {
                    Patamares.Add(new Patamar(i, pontos[i]));
                }
            }
            else
            {
                int[] pontos = [2, 4, 6, 8, 10, 12];
                for (int i = 1; i <= 6; i++)
                {
                    Patamares.Add(new Patamar(i, pontos[i]));
                }
            }

            return Patamares;
        }
    }
}