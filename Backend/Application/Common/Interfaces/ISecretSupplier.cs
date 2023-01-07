namespace Application.Common.Interfaces;

public interface ISecretSupplier
{
    string GetSecret(string key);
}