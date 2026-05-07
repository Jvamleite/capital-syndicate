using CapitalSyndicate.Domain.Cartas;

namespace CapitalSyndicate.Domain.Baralhos
{
    public class Baralho
    {
        private const int NumeroDeCrises = 3;
        private static readonly Random Random = new();

        public Stack<Carta> Monte { get; }
        public List<Carta> MercadoDeTalentos { get; }
        public IReadOnlyList<Carta> Descarte { get; private set; }

        public Baralho(List<Carta> cartasNormais, IEnumerable<Carta> cartasCrise, int numJogadores)
        {
            Monte = new Stack<Carta>(CriarMonte(cartasNormais, [.. cartasCrise]));
            MercadoDeTalentos = CriarMercadoDeTalentos(numJogadores);
            Descarte = [];
        }

        public Carta ComprarCartaDoMonte() => Monte.Pop();

        public Carta ComprarCartaDoMercadoDeTalentos(Guid idCarta)
        {
            Carta? carta = MercadoDeTalentos.FirstOrDefault(c => c.Id == idCarta)
                ?? throw new Exception("A carta selecionada é inválida.");

            MercadoDeTalentos.Remove(carta);

            return carta;
        }

        public void AdicionarCartasNoMercadoDeTalentos(List<Carta> cartas) =>
            MercadoDeTalentos.AddRange(cartas);

        public void DescartarCartasDoMercadoDeTalentos()
        {
            Descarte = [.. MercadoDeTalentos];
            MercadoDeTalentos.Clear();
        }

        private List<Carta> CriarMercadoDeTalentos(int numJogadores)
        {
            List<Carta> mercadoDeTalentos = [];
            for (int i = 0; i < numJogadores + 2; i++)
            {
                mercadoDeTalentos.Add(Monte.Pop());
            }

            return mercadoDeTalentos;
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

            for (int indiceAtual = resultado.Count - 1; indiceAtual > 0; indiceAtual--)
            {
                int indiceAleatorio = Random.Next(indiceAtual + 1);
                (resultado[indiceAtual], resultado[indiceAleatorio]) = (resultado[indiceAleatorio], resultado[indiceAtual]);
            }

            return resultado;
        }
    }
}