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

        public async Task<IReadOnlyList<Carta>> FazerLayoff(Projeto projeto, Partida partida)
        {
            if (projeto.Gerente.Cargo == Cargo.GestorDeRh)
            {
                return await AplicarLayoffGestorDeRh(projeto, partida);
            }

            List<Carta> cartasDescartadas = [.. CartasNaMao];
            CartasNaMao.Clear();

            return cartasDescartadas;
        }

        private async Task<IReadOnlyList<Carta>> AplicarLayoffGestorDeRh(Projeto projeto, Partida partida)
        {
            int cartasAManter = await partida.Entrada.EscolherNumManterCartas(this, partida, projeto.Escala);
            int cartasADescartar = CartasNaMao.Count - cartasAManter;
            List<Carta> cartasDescartadas = [.. CartasNaMao.Take(cartasADescartar)];
            foreach (Carta carta in cartasDescartadas)
            {
                CartasNaMao.Remove(carta);
            }

            return cartasDescartadas;
        }
    }
}