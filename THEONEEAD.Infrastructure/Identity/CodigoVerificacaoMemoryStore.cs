using System.Collections.Concurrent;
using THEONEEAD.Application.Common.Interfaces;

namespace THEONEEAD.Infrastructure.Identity;

public class CodigoVerificacaoMemoryStore : ICodigoVerificacaoStore
{
    private readonly ConcurrentDictionary<string, Entrada> _map = new();

    private sealed record Entrada(int UsuarioId, string CodigoHashBcrypt, DateTime ExpiraUtc, string Fluxo);

    public string Registrar(int usuarioId, string codigoHashBcrypt, DateTime expiraEmUtc, string fluxo)
    {
        var chave = Guid.NewGuid().ToString("N");
        _map[chave] = new Entrada(usuarioId, codigoHashBcrypt, expiraEmUtc, fluxo);
        return chave;
    }

    public bool TentarValidarEConsumir(string chave, string codigoPlaintext, out int usuarioId, out string fluxo)
    {
        usuarioId = default;
        fluxo = string.Empty;
        if (!_map.TryGetValue(chave, out var e))
            return false;

        if (DateTime.UtcNow > e.ExpiraUtc)
        {
            _map.TryRemove(chave, out _);
            return false;
        }

        if (!BCrypt.Net.BCrypt.Verify(codigoPlaintext, e.CodigoHashBcrypt))
            return false;

        _map.TryRemove(chave, out _);
        usuarioId = e.UsuarioId;
        fluxo = e.Fluxo;
        return true;
    }

    public bool ChaveExisteENaoExpirou(string chave)
    {
        if (string.IsNullOrWhiteSpace(chave) || !_map.TryGetValue(chave.Trim(), out var e))
            return false;

        if (DateTime.UtcNow > e.ExpiraUtc)
        {
            _map.TryRemove(chave.Trim(), out _);
            return false;
        }

        return true;
    }
}
