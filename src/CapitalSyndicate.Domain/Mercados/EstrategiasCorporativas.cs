namespace CapitalSyndicate.Domain.Mercados
{
    internal class EstrategiasCorporativas : Mercado
    {
        public bool OperacaoesAfterHours { get; }

        public EstrategiasCorporativas(bool avancado)
        {
            Nome = avancado ? "Operações After-Hours" : "Fusões e Aquisições";
            Patamares = CriarPatamares(avancado);
            OperacaoesAfterHours = avancado;
        }

        protected override List<Patamar> CriarPatamares(bool avancado)
        {
            if (!avancado)
            {
                int[] pontos = [2, 4, 6, 8, 11, 14];
                int[] requisitos = [3, 3, 4, 4, 5, 5];
                foreach (int i in Enumerable.Range(0, pontos.Length))
                {
                    Patamares.Add(new Patamar(requisitos[i], pontos[i]));
                }
            }
            else
            {
                int[] pontos = [0, 1, 2, 2, 4, 6];
                for (int i = 1; i <= 6; i++)
                {
                    Patamares.Add(new Patamar(i, pontos[i - 1]));
                }
            }

            return Patamares;
        }
    }
}