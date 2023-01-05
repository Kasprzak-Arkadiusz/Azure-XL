namespace Infrastructure.Common.Exceptions;

public class ExternalProviderException : Exception
{
    public ExternalProviderException(string message) : base(message) { }
}