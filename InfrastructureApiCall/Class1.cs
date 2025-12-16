using Microsoft.Extensions.Configuration;

namespace InfrastructureApiCall;

public class Class1(IConfiguration conf)
{
    public void Call()
    {
        conf.GetConnectionString("connection-string-name");
    }
}