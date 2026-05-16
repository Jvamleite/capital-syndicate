using CapitalSyndicate.Domain.Cartas;
using CapitalSyndicate.Domain.Enums;
using CapitalSyndicate.Domain.Jogadores;
using CapitalSyndicate.Domain.Mercados;
using CapitalSyndicate.Domain.Partidas;
using CapitalSyndicate.Domain.Ports;
using CapitalSyndicate.Domain.Projetos;

namespace CapitalSyndicate.Web.Services;

public class EntradaJogadorBlazor : IEntradaJogador
{
    public Func<Jogador, int, Task<bool>>? OnConfirmarCashOut { get; set; }

    public Func<Jogador, Presenca, Presenca, Task<Presenca>>? OnEscolherMarcadorLogistica { get; set; }

    public Func<Jogador, Partida, Task<Projeto?>>? OnEscolherSegundoProjeto { get; set; }

    public Func<Jogador, Partida, Setor, Task<Projeto?>>? OnEscolherProjetoAfterHours { get; set; }

    public Func<Jogador, List<Presenca>, Task<Presenca>>? OnEscolherPresencaParaMemorando { get; set; }

    public Func<Jogador, List<Mercado>, Task<Mercado>>? OnEscolherMercadoExpansao { get; set; }

    public Func<Jogador, Partida, int, Task<int>>? OnEscolherNumManterCartas { get; set; }

    public Func<Jogador, Task<Carta>>? OnEscolherCartaParaMercadoDeTalentos { get; set; }

    public Func<Jogador, Task<TipoAtivo>>? OnEscolherTipoAtivoCorporativo { get; set; }

    public Func<CartaCrise, Task>? OnCriseRevelada { get; set; }

    public async Task<bool> ConfirmarCashOut(
        Jogador jogador,
        int pontosOferecidos)
    {
        return OnConfirmarCashOut is not null && await OnConfirmarCashOut.Invoke(
                jogador,
                pontosOferecidos);
    }

    public async Task<Presenca> EscolherMarcadorLogistica(
        Jogador jogador,
        Presenca primeira,
        Presenca segunda)
    {
        return OnEscolherMarcadorLogistica is not null
            ? await OnEscolherMarcadorLogistica.Invoke(
                jogador,
                primeira,
                segunda)
            : primeira;
    }

    public async Task<Projeto?> EscolherSegundoProjeto(
        Jogador jogador,
        Partida partida)
    {
        return OnEscolherSegundoProjeto is not null
            ? await OnEscolherSegundoProjeto.Invoke(
                jogador,
                partida)
            : null;
    }

    public async Task<Projeto?> EscolherProjetoAfterHours(
        Jogador jogador,
        Partida partida,
        Setor setorObrigatorio)
    {
        return OnEscolherProjetoAfterHours is not null
            ? await OnEscolherProjetoAfterHours.Invoke(
                jogador,
                partida,
                setorObrigatorio)
            : null;
    }

    public async Task<Presenca> EscolherPresencaParaMemorando(
        Jogador jogador,
        List<Presenca> presencasDisponiveis)
    {
        return OnEscolherPresencaParaMemorando is not null
            ? await OnEscolherPresencaParaMemorando.Invoke(
                jogador,
                presencasDisponiveis)
            : presencasDisponiveis[0];
    }

    public async Task<Mercado> EscolherMercadoExpansao(
        Jogador jogador,
        List<Mercado> mercadosValidos)
    {
        if (OnEscolherMercadoExpansao is null)
        {
            throw new InvalidOperationException(
                "Nenhum mercado foi escolhido para expansão.");
        }

        return await OnEscolherMercadoExpansao.Invoke(
            jogador,
            mercadosValidos);
    }

    public async Task<int> EscolherNumManterCartas(
        Jogador jogador,
        Partida partida,
        int escalaProjeto)
    {
        return OnEscolherNumManterCartas is not null
            ? await OnEscolherNumManterCartas.Invoke(
                jogador,
                partida,
                escalaProjeto)
            : 0;
    }

    public async Task<Carta> EscolherCartaParaMercadoDeTalentos(
        Jogador jogador)
    {
        if (OnEscolherCartaParaMercadoDeTalentos is null)
        {
            throw new InvalidOperationException(
                "Nenhuma carta foi escolhida para o mercado de talentos.");
        }

        return await OnEscolherCartaParaMercadoDeTalentos.Invoke(
            jogador);
    }

    public async Task<TipoAtivo>
        EscolherTipoAtivoCorporativo(
        Jogador jogador)
    {
        if (OnEscolherTipoAtivoCorporativo is null)
        {
            throw new InvalidOperationException(
                "Nenhum tipo de ativo foi escolhido.");
        }

        return await OnEscolherTipoAtivoCorporativo
            .Invoke(jogador);
    }

    public async Task NotificarCrise(CartaCrise crise)
    {
        if (OnCriseRevelada is not null)
        {
            await OnCriseRevelada(crise);
        }
    }
}