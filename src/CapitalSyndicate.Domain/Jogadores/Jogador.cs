using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Enums;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;
using CapitalSyndicate.Domain.Tokens;

namespace CapitalSyndicate.Domain.Jogadores
{
    public class Jogador(string nome)
    {
        private const int CapacidadeMaximaDaMao = 10;

        public string Nome { get; } = nome;
        public int PontosVitoria { get; set; } = 0;
        public List<Token> Tokens { get; } = [];
        public List<Projeto> ProjetosNaMesa { get; } = [];
        public List<Carta> CartasNaMao { get; } = [];
        public List<Presenca> PresencasDeMercado { get; } = [];

        public void ComprarCartas(IEnumerable<Carta> cartas)
        {
            if (CartasNaMao.Count == CapacidadeMaximaDaMao)
            {
                throw new InvalidOperationException("Sua mão está cheia. Execute um projeto antes de comprar mais cartas.");
            }

            CartasNaMao.AddRange(cartas);
        }

        public void ValidarCartas(Projeto projeto)
        {
            if (!projeto.Profissionais.All(CartasNaMao.Contains))
            {
                throw new InvalidOperationException("Você não tem todas as cartas necessárias para este projeto.");
            }
        }

        public void AlocarProfissionais(Projeto projeto)
        {
            CartasNaMao.RemoveAll(projeto.Profissionais.Contains);
            ProjetosNaMesa.Add(projeto);
        }

        public IReadOnlyList<Carta> FazerLayoff(Projeto projeto, Partida partida)
        {
            if (projeto.Gerente.Cargo == Cargo.GestorDeRh)
            {
                AplicarLayoffGestorDeRh(projeto, partida);
            }

            List<Carta> cartasDescartadas = [.. CartasNaMao];
            CartasNaMao.Clear();
            return cartasDescartadas;
        }

        private void AplicarLayoffGestorDeRh(Projeto projeto, Partida partida)
        {
            int cartasAManter = partida.Entrada.EscolherNumManterCartas(this, partida, projeto.Escala);
            int cartasADescartar = CartasNaMao.Count - cartasAManter;
            CartasNaMao.RemoveRange(0, cartasADescartar);
        }
    }
}