using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VContainerSample.Scripts.MessagePipe
{
    //Mediator
    public readonly struct Ping
    {
        public int Value { get; }

        public Ping(int value)
        {
            this.Value = value;
        }
    }

    public readonly struct Pong
    {
        public int Value { get; }

        public Pong(int value)
        {
            this.Value = value;
        }
    }
    public class PingPongRequestHandler : IRequestHandler<Ping, Pong>
    {
        public Pong Invoke(Ping request)
        {
            var pong = new Pong(UnityEngine.Random.Range(500, 1000));
            Debug.Log($"Receive Ping value {request.Value} in ${nameof(PingPongRequestHandler)} and response Pong value {pong.Value}" );
            return pong;
        }
    }
    
    public class PingPongAsyncRequestHandler : IAsyncRequestHandler<Ping, Pong>
    {
        public UniTask<Pong> InvokeAsync(Ping request, CancellationToken cancellationToken = new CancellationToken())
        {
            var pong = new Pong(UnityEngine.Random.Range(0, 100));
            Debug.Log($"Receive Ping value {request.Value} in ${nameof(PingPongAsyncRequestHandler)} and response Pong value {pong.Value}");
            return UniTask.FromResult(pong);
        }
    }

    public class FooController
    {
        readonly IRequestHandler<Ping, Pong> _handler;
        
        public FooController(IRequestHandler<Ping, Pong> handler)
        {
            _handler = handler;
        }
        
        public void Foo()
        {
            Random.InitState(34);
            var ping = new Ping(Random.Range(0, 100));
            Debug.Log($"Send Ping value {ping.Value} in ${nameof(FooController)}");
            _handler.Invoke(ping);
        }
    }
    
    public class FooAsyncController
    {
        readonly PingPongAsyncRequestHandler _handler;
        
        public FooAsyncController(PingPongAsyncRequestHandler handler)
        {
            _handler = handler;
        }
        
        public async UniTask Foo()
        {
            var ping = new Ping(Random.Range(0, 100));
            Debug.Log($"Send Ping value {ping.Value} in ${nameof(FooAsyncController)}");
            await _handler.InvokeAsync(ping);
        }
    }
    
    public class PingPongRequestHandler2 : IRequestHandler<Ping, Pong>
    {
        public Pong Invoke(Ping request)
        {
            var pong = new Pong(Random.Range(500, 1000));
            Debug.Log($"Receive Ping value {request.Value} in ${nameof(PingPongRequestHandler2)} and response Pong value {pong.Value}");
            return pong;
        }
    }
    
    public class PingPongRequestHandler3 : IRequestHandler<Ping, Pong>
    {
        public Pong Invoke(Ping request)
        {
            var pong = new Pong(Random.Range(500, 1000));
            Debug.Log($"Receive Ping value {request.Value} in ${nameof(PingPongRequestHandler3)} and response Pong value {pong.Value}");
            return pong;
        }
    }

    public class BarController
    {
        readonly IRequestAllHandler<Ping, Pong> _handler;
        
        public BarController(IRequestAllHandler<Ping, Pong> handler)
        {
            _handler = handler;
        }
        
        public void Bar()
        {
            Random.InitState(44);
            var ping = new Ping(Random.Range(0, 100));
            Debug.Log($"Send Ping value {ping.Value} in ${nameof(BarController)}");
            _handler.InvokeAll(ping);
        }
    }
    
    public class PingPongRequestHandlerFilter : RequestHandlerFilter<Ping, Pong>
    {
        public override Pong Invoke(Ping request, Func<Ping, Pong> next)
        {
            //ignore request has odd value
            if (request.Value % 2 == 1)
            {
                Debug.Log($"Ignore Ping value {request.Value} in ${nameof(PingPongRequestHandlerFilter)}");
                return default;
            }
            return next(request);
        }
    }
    
    public class PingPongMessageHandlerFilter : MessageHandlerFilter<Ping>
    {
        public override void Handle(Ping message, Action<Ping> next)
        {
            //ignore request has odd value
            if (message.Value % 2 == 1)
            {
                Debug.Log($"Ignore Ping value {message.Value} in ${nameof(PingPongMessageHandlerFilter)}");
                return;
            }
            next(message);
        }
    }
}