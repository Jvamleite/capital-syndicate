using CapitalSyndicate.Domain.Partidas;

namespace CapitalSyndicate.Web.Services;

public class PartidaEstado
{
    public Partida? Partida { get; private set; }

    public bool EmAndamento => Partida is not null && !Partida.Encerrada;

    public void Iniciar(Partida partida)
    {
        Partida = partida;
    }

    public void Encerrar()
    {
        Partida = null;
    }
}