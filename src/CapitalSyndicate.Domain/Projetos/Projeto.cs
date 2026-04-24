using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Enums;

namespace CapitalSyndicate.Domain.Projetos
{
    public sealed class Projeto
    {
        private static readonly IReadOnlySet<Cargo> CargosCoringa = new HashSet<Cargo> { Cargo.ConsultorExterno, Cargo.Trainee };

        public int Escala { get; }
        public Setor SetorFinal { get; }
        public List<CartaProfissional> Profissionais { get; }
        public CartaProfissional Gerente { get; }

        private Projeto(IEnumerable<CartaProfissional> profissionais, CartaProfissional gerente)
        {
            Profissionais = [.. profissionais];
            Gerente = gerente;
            Escala = profissionais.Count();
            SetorFinal = gerente.Setor;
        }

        public static Projeto Criar(List<CartaProfissional> profissionais, CartaProfissional gerente)
        {
            ValidarProfissionais(profissionais);
            ValidarGerente(gerente, profissionais);
            ValidarCoesaoDaEquipe(profissionais);
            return new Projeto(profissionais, gerente);
        }

        private static void ValidarProfissionais(List<CartaProfissional> profissionais)
        {
            if (profissionais is null || profissionais.Count == 0)
            {
                throw new InvalidOperationException("Selecione pelo menos um profissional para criar o projeto.");
            }
        }

        private static void ValidarGerente(CartaProfissional gerente, List<CartaProfissional> profissionais)
        {
            if (gerente is null)
            {
                throw new InvalidOperationException("Escolha um gerente para o projeto.");
            }

            if (!profissionais.Contains(gerente))
            {
                throw new InvalidOperationException("O gerente escolhido deve fazer parte do projeto.");
            }

            if (!gerente.PodeSerGerente())
            {
                throw new InvalidOperationException("Esse profissional não pode atuar como líder.");
            }
        }

        private static void ValidarCoesaoDaEquipe(List<CartaProfissional> profissionais)
        {
            List<CartaProfissional> profissionaisElegiveisParaValidacao = [.. profissionais
                .Where(p => !CargosCoringa.Contains(p.Cargo))];

            if (profissionaisElegiveisParaValidacao.Count == 0)
            {
                return;
            }

            CartaProfissional referencia = profissionaisElegiveisParaValidacao[0];

            bool mesmoSetor = profissionaisElegiveisParaValidacao.All(p => p.Setor == referencia.Setor);
            bool mesmoCargo = profissionaisElegiveisParaValidacao.All(p => p.Cargo == referencia.Cargo);

            if (!mesmoSetor && !mesmoCargo)
            {
                throw new InvalidOperationException("Os profissionais precisam compartilhar o mesmo setor ou cargo.");
            }
        }
    }
}