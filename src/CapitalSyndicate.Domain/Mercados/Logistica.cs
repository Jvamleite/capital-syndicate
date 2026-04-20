namespace CapitalSyndicate.Domain.Mercados
{
    internal class Logistica : Mercado
    {
        public bool ExpansaoParalela { get; }

        public Logistica(bool avancado)
        {
            Nome = avancado ? "Operação Multihub" : "Malha de Distribuição";
            ExpansaoParalela = avancado;
        }

        protected override List<Patamar> CriarPatamares(bool avancado)
        {
            if (!avancado)
            {
                int[] pontos = [1, 2, 3, 5, 7, 9];
                for (int i = 1; i <= 6; i++)
                {
                    Patamares.Add(new Patamar(i, pontos[i - 1]));
                }
            }
            else
            {
                int[] pontos = [2, 5, 9, 14, 20];
                int[] requisitos = [1, 2, 3, 4, 5];
                foreach (int i in Enumerable.Range(0, pontos.Length))
                {
                    Patamares.Add(new Patamar(requisitos[i], pontos[i]));
                }
            }

            return Patamares;
        }
    }
}