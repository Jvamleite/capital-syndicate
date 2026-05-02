using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Enums;

namespace CapitalSyndicate.Domain.Projetos
{
    internal sealed class Projeto
    {
        public int Escala { get; }
        public Setor SetorFinal { get; }
        public List<CartaProfissional> Profissionais { get; }
        public CartaProfissional Gerente { get; }

        private Projeto(IEnumerable<CartaProfissional> profissionais, CartaProfissional gerente)
        {
            Profissionais = [.. profissionais];
            Gerente = gerente;
            Escala = CalcularEscala(profissionais);
            SetorFinal = gerente.Setor;
        }

        public static Projeto Criar(List<CartaProfissional> profissionais, CartaProfissional gerente)
        {
            if (profissionais is null || profissionais.Count == 0)
            {
                throw new Exception("Selecione pelo menos um profissional para criar o projeto.");
            }

            if (gerente == null)
            {
                throw new Exception("Escolha um gerente para o projeto.");
            }

            if (!profissionais.Contains(gerente))
            {
                throw new Exception("O gerente escolhido deve fazer parte do projeto.");
            }

            if (!gerente.PodeSerGerente())
            {
                throw new Exception("Esse profissional não pode atuar como líder.");
            }

            List<CartaProfissional> profissionaisComuns = [.. profissionais.Where(p => p.Cargo != Cargo.ConsultorExterno || p.Cargo != Cargo.Trainee)];
            bool mesmoSetor = profissionaisComuns.All(p => p.Setor == profissionais[0].Setor);
            bool mesmoCargo = profissionaisComuns.All(p => p.Cargo == profissionais[0].Cargo);

            if (!(mesmoSetor || mesmoCargo))
            {
                throw new Exception("Os profissionais precisam compartilhar o mesmo setor ou cargo.");
            }

            return new Projeto(profissionais, gerente);
        }

        private static int CalcularEscala(IEnumerable<CartaProfissional> profissionais)
        {
            return profissionais.Count();
        }
    }
}