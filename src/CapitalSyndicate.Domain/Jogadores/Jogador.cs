using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Cartas.Habilidades;
using CapitalSyndicate.Domain.Enums;
using CapitalSyndicate.Domain.Mercados;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Projetos;
using CapitalSyndicate.Domain.Tokens;

namespace CapitalSyndicate.Domain.Jogadores
{
    internal class Jogador(string nome)
    {
        public string Nome { get; } = nome;
        public int PontosVitoria { get; set; } = 0;
        public List<Token> Tokens { get; } = [];
        public List<Projeto> ProjetosNaMesa { get; } = [];
        public List<Carta> CartasNaMao { get; } = [];
        public List<Presenca> PresencasDeMercado { get; } = [];

        public void ComprarCartas(IEnumerable<Carta> cartas)
        {
            if (CartasNaMao.Count == 10)
            {
                throw new Exception("Sua mão está cheia. Execute um projeto antes de comprar mais cartas.");
            }

            CartasNaMao.AddRange(cartas);
        }

        public IReadOnlyList<Carta> ExecutarProjeto(Projeto projeto, Partida partida, bool segundoProjeto = false)
        {
            if (!projeto.Profissionais.All(p => CartasNaMao.Contains(p)))
            {
                throw new Exception("Você não tem todas as cartas necessárias para este projeto.");
            }

            CartasNaMao.RemoveAll(x => projeto.Profissionais.Contains(x));

            ProjetosNaMesa.Add(projeto);

            Mercado mercado;

            if (projeto.Gerente.Cargo == Cargo.DiretorDeExpansao)
            {
                mercado = partida.Entrada.EscolherMercadoExpansao(this, partida);
            }
            else
            {
                mercado = partida.ObterMercado(projeto.SetorFinal);
            }

            bool podeAvancar = mercado.PodeAvancar(this, projeto);

            Cargo cargoGerenteProjeto = projeto.Gerente.Cargo;
            IHabilidadeLider habilidadeLider = cargoGerenteProjeto.ObterHabilidade();
            if (cargoGerenteProjeto == Cargo.NegociadorInternacional && partida.TrilhaGlobal != null)
            {
                partida.TrilhaGlobal.AvancarPresenca(this, partida, projeto.Escala);
            }
            else if (podeAvancar)
            {
                mercado.AvancarPresenca(this, partida);
                habilidadeLider.AposAvanco(this, partida, projeto);
            }

            if (!segundoProjeto && podeAvancar && cargoGerenteProjeto == Cargo.DiretorDeOperacoes)
            {
                partida.Entrada.ExecutarProjetoAdicional(this, mercado);
            }

            if (Cargo.Auditor == cargoGerenteProjeto || Cargo.GestorDePortifolio == cargoGerenteProjeto || Cargo.EspecialistaEsg == cargoGerenteProjeto)
            {
                Tokens.Add(Token.GerarToken(cargoGerenteProjeto));
            }

            return FazerLayoff(partida, projeto);
        }

        public IReadOnlyList<Carta> FazerLayoff(Partida partida, Projeto projeto)
        {
            if (projeto.Gerente.Cargo == Cargo.GestorDeRh)
            {
                int numCartas = partida.Entrada.EscolherNumManterCartas(this, partida, projeto.Escala);
                int numCartasDescartar = CartasNaMao.Count - numCartas;

                for (int i = 0; i < numCartasDescartar; i++)
                {
                    CartasNaMao.RemoveAt(0);
                }
            }

            List<Carta> cartas = CartasNaMao;
            CartasNaMao.Clear();
            return cartas;
        }
    }
}