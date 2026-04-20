namespace CapitalSyndicate.Domain.Mercados
{
    internal class Inovação : Mercado
    {
        public bool InjecaoDeRecursos { get; }

        public Inovação(bool avancado)
        {
            Nome = avancado ? "Pipeline de P&D" : "Laboratório de Inovação";
            Patamares = CriarPatamares(avancado);
            InjecaoDeRecursos = avancado;
        }

        protected override List<Patamar> CriarPatamares(bool avancado)
        {
            if (!avancado)
            {
                int[] pontos = [1, 2, 3, 4, 5, 6, 7, 8, 10];
                int[] requisitos = [1, 1, 1, 2, 2, 2, 3, 3, 3];
                foreach (int i in Enumerable.Range(0, pontos.Length))
                {
                    Patamares.Add(new Patamar(requisitos[i], pontos[i]));
                }
            }
            else
            {
                int[] pontos = [0, 1, 2, 3, 5, 7];
                for (int i = 1; i <= 6; i++)
                {
                    Patamares.Add(new Patamar(i, pontos[i - 1]));
                }
            }

            return Patamares;
        }
    }
}