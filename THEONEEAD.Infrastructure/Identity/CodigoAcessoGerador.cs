using THEONEEAD.Application.Common.Interfaces;

namespace THEONEEAD.Infrastructure.Identity;

public class CodigoAcessoGerador : ICodigoAcessoGerador
{
    public string GerarCodigoNumerico(int digitos = 6)
    {
        var max = (int)Math.Pow(10, digitos) - 1;
        return Random.Shared.Next(0, max + 1).ToString($"D{digitos}");
    }
}
