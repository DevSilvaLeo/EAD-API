namespace THEONEEAD.Application.Common.Interfaces;

public interface ICodigoAcessoGerador
{
    string GerarCodigoNumerico(int digitos = 6);
}
