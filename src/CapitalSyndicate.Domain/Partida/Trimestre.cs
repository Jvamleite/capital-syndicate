using CapitalSyndicate.Domain.Enums;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Mercados;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Domain.Partidas
{
    public class Trimestre
    {
        public int Numero { get; set; }
        public int CrisesReveladas { get; set; }
        public bool Encerrado { get; private set; }

        private readonly Queue<Jogador> _jogadores;
        public Turno TurnoAtual { get; private set; }

        public Trimestre(int numero, IEnumerable<Jogador> jogadores)
        {
            Numero = numero;
            _jogadores = new Queue<Jogador>(jogadores);
            TurnoAtual = new Turno(_jogadores.Peek());
        }

        public void AvancarTurno()
        {
            TurnoAtual.Encerrar();

            Jogador jogador = _jogadores.Dequeue();
            _jogadores.Enqueue(jogador);

            TurnoAtual = new Turno(_jogadores.Peek());
        }

        public void Encerrar(Partida partida)
        {
            if (Encerrado)
            {
                return;
            }

            Encerrado = true;

            foreach (Jogador jogador in partida.Jogadores)
            {
                jogador.CartasNaMao.Clear();
            }

            foreach (Jogador jogador in partida.Jogadores)
            {
                foreach (Mercado mercado in partida.Mercados)
                {
                    mercado.AntesDaPontuacaoDoTrimestre(jogador, partida);
                }

                foreach (Projeto projeto in jogador.ProjetosNaMesa)
                {
                    projeto.Gerente.Cargo.ObterHabilidade().AoFimDoTrimestre(jogador, projeto);
                }
            }

            foreach (Jogador jogador in partida.Jogadores)
            {
                foreach (Mercado mercado in partida.Mercados)
                {
                    jogador.PontosVitoria += mercado.ObterPontuacaoJogador(jogador, partida);
                }
            }

            foreach (Jogador jogador in partida.Jogadores)
            {
                foreach (Projeto projeto in jogador.ProjetosNaMesa)
                {
                    jogador.PontosVitoria += projeto.Escala;
                }

                foreach (Mercado mercado in partida.Mercados)
                {
                    mercado.AposPontuacaoDoTrimestre(jogador, partida);
                }
            }

            if (partida.TrilhaGlobal != null)
            {
                foreach (Jogador jogador in partida.Jogadores)
                {
                    jogador.PontosVitoria += partida.TrilhaGlobal.PontuarFimDeTrimestre(jogador);
                }
            }
        }
    }
}