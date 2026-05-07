using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Application.Partidas
{
    public class PartidaService
    {
        public static void Iniciar(Partida partida)
        {
            if (partida.Trimestres.Count > 0)
            {
                throw new InvalidOperationException("A partida já foi iniciada.");
            }

            partida.Iniciar();
        }

        public static ResultadoAvancoTrimestre AvancarTrimestre(Partida partida, Jogador jogadorQueRevelouCrise)
        {
            Trimestre trimestreAtual = partida.ObterTrimestreAtual();

            if (!trimestreAtual.Encerrado)
            {
                throw new InvalidOperationException("O trimestre atual ainda não foi encerrado.");
            }

            partida.JogadorQueRevelouUltimaCrise = jogadorQueRevelouCrise;

            if (partida.TodosOsTrimestresEncerrados())
            {
                return EncerrarJogo(partida);
            }

            partida.IniciarNovoTrimestre();
            return new ResultadoAvancoTrimestre(JogoEncerrado: false, Vencedores: []);
        }

        private static ResultadoAvancoTrimestre EncerrarJogo(Partida partida)
        {
            partida.Encerrar();
            List<Jogador> vencedores = partida.DefinirVencedor();
            return new ResultadoAvancoTrimestre(JogoEncerrado: true, Vencedores: vencedores);
        }

        public EstadoPartida ObterEstado(Partida partida)
        {
            Trimestre trimestreAtual = partida.ObterTrimestreAtual();

            return new EstadoPartida(
                NumeroTrimestre: trimestreAtual.Numero,
                TotalTrimestres: partida.TotalTrimestres,
                CrisesReveladas: trimestreAtual.CrisesReveladas,
                JogadorDaVez: trimestreAtual.TurnoAtual.Jogador,
                TrimestreEncerrado: trimestreAtual.Encerrado,
                JogoEncerrado: partida.Encerrada
            );
        }
    }

    public record ResultadoAvancoTrimestre(
        bool JogoEncerrado,
        List<Jogador> Vencedores
    );

    public record EstadoPartida(
        int NumeroTrimestre,
        int TotalTrimestres,
        int CrisesReveladas,
        Jogador JogadorDaVez,
        bool TrimestreEncerrado,
        bool JogoEncerrado
    );
}