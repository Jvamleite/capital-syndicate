using CapitalSyndicate.Domain.Cartas;

namespace CapitalSyndicate.Domain.Baralho
{
    internal class Baralho
    {
        private const int NumeroDeCrises = 3;
        private static readonly Random Rng = new();

        public Stack<Carta> Monte { get; }

        public List<Carta> MercadoDeTalentos { get; set; }
        public IEnumerable<Carta> Descarte { get; set; }

        public Baralho(List<Carta> cartasNormais, List<Carta> cartasCrise)
        {
            Monte = new Stack<Carta>(CriarMonte(cartasNormais, cartasCrise));
            MercadoDeTalentos = [];
            Descarte = [];
        }

        public List<Carta> ComprarDoMonte(int n)
        {
            List<Carta> cartasCompradas = [];
            for (int i = 0; i < n; i++)
            {
                cartasCompradas.Add(Monte.Pop());
            }

            return cartasCompradas;
        }

        public Carta ComprarCartasDoMercadoDeTalentos(int indiceCarta)
        {
            Carta carta = MercadoDeTalentos[indiceCarta];
            MercadoDeTalentos.RemoveAt(indiceCarta);
            return carta;
        }

        public void AdicionarCartasNoMercadoDeTalentos(List<Carta> cartas) =>
            MercadoDeTalentos.AddRange(cartas);

        public void DescartarCartasDoMercadoDeTalentos()
        {
            Descarte = MercadoDeTalentos;
            MercadoDeTalentos = [];
        }

        private static List<Carta> CriarMonte(List<Carta> normais, List<Carta> crises)
        {
            List<Carta> embaralhadas = Embaralhar(normais);

            int metade = embaralhadas.Count / 2;
            List<Carta> parteSuperior = [.. embaralhadas.Take(metade)];
            List<Carta> parteInferior = [.. embaralhadas.Skip(metade)];

            List<Carta> parteInferiorComCrises = DistribuirCrisesNasPilhas(parteInferior, crises);

            return [.. parteSuperior, .. parteInferiorComCrises];
        }

        private static List<Carta> DistribuirCrisesNasPilhas(List<Carta> cartas, List<Carta> crises)
        {
            List<List<Carta>> pilhas = DividirEmPilhas(cartas, NumeroDeCrises);

            for (int i = 0; i < pilhas.Count; i++)
            {
                pilhas[i].Add(crises[i]);
                pilhas[i] = Embaralhar(pilhas[i]);
            }

            return [.. pilhas.SelectMany(p => p)];
        }

        private static List<List<Carta>> DividirEmPilhas(List<Carta> cartas, int numeroDePilhas)
        {
            int tamanhoBase = cartas.Count / numeroDePilhas;
            int resto = cartas.Count % numeroDePilhas;
            List<List<Carta>> pilhas = [];
            int index = 0;

            for (int i = 0; i < numeroDePilhas; i++)
            {
                int tamanho = tamanhoBase + (i < resto ? 1 : 0);
                pilhas.Add([.. cartas.Skip(index).Take(tamanho)]);
                index += tamanho;
            }

            return pilhas;
        }

        private static List<Carta> Embaralhar(List<Carta> cartas)
        {
            List<Carta> resultado = [.. cartas];

            for (int i = resultado.Count - 1; i > 0; i--)
            {
                int j = Rng.Next(i + 1);
                (resultado[i], resultado[j]) = (resultado[j], resultado[i]);
            }

            return resultado;
        }
    }
}