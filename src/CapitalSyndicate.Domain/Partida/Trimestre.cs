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

        private readonly Queue<Turno> _turnos;
        public Turno TurnoAtual => _turnos.Peek();

        public Trimestre(int numero, IEnumerable<Jogador> jogadores)
        {
            Numero = numero;
            _turnos = new Queue<Turno>(
                jogadores.Select(j => new Turno(j))
            );
        }

        public void AvancarTurno()
        {
            _turnos.Dequeue();

            if (_turnos.Count == 0)
            {
                Encerrado = true;
            }
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