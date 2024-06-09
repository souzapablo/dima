namespace Dima.Core.Models.Account;

public class RoleClaim
{
    public string Issuer { get; set; } = string.Empty;
    public string OrignalIssuer { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
}