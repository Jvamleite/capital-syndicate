using CapitalSyndicate.Domain.Jogadores;

namespace CapitalSyndicate.Domain.Mercados
{
    internal class Tecnologia : Mercado
    {
        public bool ApenasLiderPontua { get; }

        public Tecnologia(bool avancado)
        {
            Nome = avancado ? "Plataforma Dominante" : "Corrida Algorítmica";
            Patamares = CriarPatamares(avancado);
            ApenasLiderPontua = avancado;
        }

        protected override List<Patamar> CriarPatamares(bool avancado)
        {
            if (!avancado)
            {
                int[] pontos = [2, 3, 5, 8, 12, 17];
                int[] requisitos = [2, 2, 4, 4, 6, 6];
                foreach (int i in Enumerable.Range(0, pontos.Length))
                {
                    Patamares.Add(new Patamar(requisitos[i], pontos[i]));
                }
            }
            else
            {
                int[] pontos = [1, 2, 3, 4];
                int[] requisitos = [1, 2, 4, 6];
                foreach (int i in Enumerable.Range(0, pontos.Length))
                {
                    Patamares.Add(new Patamar(requisitos[i], pontos[i]));
                }
            }

            return Patamares;
        }

        public static void ResetarAoFimDoTrimestre(List<Presenca> presencas>)
        {
            foreach (Presenca p in presencas)
            {
                p.Resetar();
            }
        }
    }
}