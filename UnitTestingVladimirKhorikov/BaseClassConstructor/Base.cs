using Microsoft.AspNetCore.SignalR.Client;

namespace UnitTestingVladimirKhorikov.BaseClassConstructor;

public abstract class Base : IDisposable
{
    protected readonly HubConnection _connection;

    // this connect 1 time for all tests, is for integrations test with database or shared dependency
    protected Base()
    {
        _connection = new HubConnectionBuilder().WithUrl("http://localhost:5011/chatHub").Build();
        _connection.StartAsync().Wait();
    }

    public void Dispose()
    {
        _connection.DisposeAsync().GetAwaiter().GetResult();
    }
}