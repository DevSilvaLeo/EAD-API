namespace THEONEEAD.Application.Auth.Dtos;

public record LoginRequestDto(string Email, string Senha);

/// <summary>Contrato do frontend: token e dataHoraAcesso null em caso de falha.</summary>
public record LoginResponseDto(string? Token, string? DataHoraAcesso, string Mensagem);

public record EsqueciSenhaRequestDto(string? Email, string? Cpf);

/// <summary>Inclui chaveVerificacao quando o código foi gerado (necessária em validar-codigo).</summary>
public record EsqueciSenhaResponseDto(string Mensagem, string? ChaveVerificacao);

public record PrimeiroAcessoRequestDto(string Cpf);

public record PrimeiroAcessoResponseDto(string Mensagem, string? ChaveVerificacao);

public record ValidarCodigoRequestDto(string Codigo, string? ChaveVerificacao);

/// <summary>token vazio quando inválido (contrato Angular).</summary>
public record ValidarCodigoResponseDto(string Token, string Mensagem);

public record TrocarSenhaRequestDto(string Senha, string SenhaConfirmacao, string Token);

public record TrocarSenhaResponseDto(string Mensagem);

/// <summary>Recuperação em um passo: chave + código + nova senha (sem JWT intermediário).</summary>
public record RecuperarSenhaUnificadoRequestDto(string? RecuperaSenhaToken, string? Code, string? NovaSenha);

/// <summary>Consulta se a chave de primeiro acesso ainda está ativa (sem consumir o código).</summary>
public class ResetaPrimeiroAcessoRequestDto
{
    public string? ChaveVerificacao { get; set; }
    /// <summary>Alias legado (mesmo significado que <see cref="ChaveVerificacao"/>).</summary>
    public string? TokenAcesso { get; set; }
}

public record ResetaPrimeiroAcessoResponseDto(int StatusCodeResponse, string Message, string? Token = null);

/// <summary>Respostas genéricas administrativas / legado.</summary>
public record MensagemSimplesResponseDto(string Mensagem);
