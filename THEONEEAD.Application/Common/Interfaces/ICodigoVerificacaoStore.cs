namespace THEONEEAD.Application.Common.Interfaces;

/// <summary>
/// Armazena temporariamente o vínculo chave → usuário + hash do código (esqueci senha / primeiro acesso).
/// </summary>
public interface ICodigoVerificacaoStore
{
    /// <summary>Retorna a chave opaca enviada ao cliente após solicitar o código.</summary>
    string Registrar(int usuarioId, string codigoHashBcrypt, DateTime expiraEmUtc, string fluxo);

    /// <summary>Valida código em texto plano; remove a entrada se válida.</summary>
    bool TentarValidarEConsumir(string chave, string codigoPlaintext, out int usuarioId, out string fluxo);

    /// <summary>Indica se a chave ainda existe e não expirou (sem validar nem consumir o código).</summary>
    bool ChaveExisteENaoExpirou(string chave);
}
