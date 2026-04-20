using CapitalSyndicate.Domain.Jogadores;

namespace CapitalSyndicate.Domain.Mercados
{
    internal abstract class Mercado
    {
        public string Nome { get; set; }
        public List<Patamar> Patamares { get; set; }

        public void AvancarPresenca(Presenca presenca, int escalaProjeto)
        {
            if (presenca.IndicePatamar >= Patamares.Count - 1)
            {
                throw new Exception("Você já conquistou o maior patamar deste mercado");
            }

            Patamar novoPatamar = Patamares[presenca.IndicePatamar + 1];
            if (novoPatamar.Requisito <= escalaProjeto)
            {
                presenca.Avancar();
            }
        }

        public int ObterPontuacaoJogador(Presenca presenca) => Patamares[presenca.IndicePatamar].Pontos;

        protected abstract List<Patamar> CriarPatamares(bool avancado);
    }
}