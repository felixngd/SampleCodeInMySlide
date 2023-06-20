using MessagePipe;
using VContainer;
using VContainer.Unity;

namespace VContainerSample.Scripts.MessagePipe
{
    public class MessagePipeLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            var messagePipeOption = builder.RegisterMessagePipe();
            messagePipeOption.AddGlobalRequestHandlerFilter<PingPongRequestHandlerFilter>();
            builder.RegisterRequestHandlerFilter<PingPongRequestHandlerFilter>();

            builder.RegisterRequestHandler<Ping, Pong, PingPongRequestHandler>(messagePipeOption);
            builder.RegisterRequestHandler<Ping, Pong, PingPongRequestHandler2>(messagePipeOption);
            builder.RegisterRequestHandler<Ping, Pong, PingPongRequestHandler3>(messagePipeOption);

            builder.Register<FooController>(Lifetime.Singleton);
            builder.Register<BarController>(Lifetime.Singleton);

            builder.RegisterEntryPoint<TestPingPong>(Lifetime.Singleton).AsSelf();
        }
    }
}