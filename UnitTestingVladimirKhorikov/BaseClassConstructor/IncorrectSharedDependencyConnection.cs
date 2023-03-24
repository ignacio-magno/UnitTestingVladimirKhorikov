using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR.Client;

namespace UnitTestingVladimirKhorikov.BaseClassConstructor;

// this is incorrect because make each connection for each test
public class IncorrectSharedDependencyConnection
{
    private HubConnection _connection;

    [SetUp]
    public void Setup()
    {
        _connection = new HubConnectionBuilder().WithUrl("http://localhost:5011/chatHub").Build();
        _connection.StartAsync().Wait();
    }
    
    [TearDown]
    public void TearDown()
    {
        _connection.DisposeAsync().GetAwaiter().GetResult();
    }

    [Test, Repeat(10)]
    public async Task CorrectConnection()
    {
        Channel<string> channel = Channel.CreateUnbounded<string>();

        _connection.On("ReceiveMessage",
            (string message) => { channel.Writer.TryWrite(message); });

        await _connection.InvokeCoreAsync("SendMessage", typeof(string), new[] { "ignacio", "hellou" });

        var result = await channel.Reader.ReadAsync();

        Assert.That(result, Is.EqualTo("hi"));
    }

    [Test, Repeat(10)]
    public async Task CorrectConnectionParallel()
    {
        Channel<string> channel = Channel.CreateUnbounded<string>();

        _connection.On("ReceiveMessage",
            (string message) => { channel.Writer.TryWrite(message); });

        await _connection.InvokeCoreAsync("SendMessage", typeof(string), new[] { "ignacio", "hellou" });

        var result = await channel.Reader.ReadAsync();

        Assert.That(result, Is.EqualTo("hi"));
    }
}