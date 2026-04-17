using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Projetos;
using CapitalSyndicate.Domain.Tokens;

namespace CapitalSyndicate.Domain.Jogadores
{
    internal class Jogador(string nome)
    {
        public string Nome { get; } = nome;
        public int PontosVitoria { get; } = 0;
        public IEnumerable<Token> Tokens { get; } = [];
        public List<Projeto> ProjetosNaMesa { get; } = [];
        public List<Carta> CartasNaMao { get; } = [];

        public void ComprarCartas(IEnumerable<Carta> cartas)
        {
            if (CartasNaMao.Count == 10)
            {
                throw new Exception("Sua mão está cheia. Execute um projeto antes de comprar mais cartas.");
            }

            CartasNaMao.AddRange(cartas);
        }

        public void ExecutarProjeto(Projeto projeto)
        {
            if (!projeto.Profissionais.All(p => CartasNaMao.Contains(p)))
            {
                throw new Exception("Você não tem todas as cartas necessárias para este projeto.");
            }

            CartasNaMao.RemoveAll(x => projeto.Profissionais.Contains(x));

            ProjetosNaMesa.Add(projeto);
        }

        public IReadOnlyList<Carta> FazerLayoff()
        {
            List<Carta> cartas = CartasNaMao;
            CartasNaMao.Clear();
            return cartas;
        }
    }
}