using CoEco.Core.Eventing;
using CoEco.Core.Interfaces;
using CoEco.Core.Ordering.Dto.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoEco.Core.Infrastructure
{
    public class Dispatcher
    {
        public Dispatcher(ILogger logger, IEventPublisher eventPublisher)
        {
            this.logger = logger;
            this.eventPublisher = eventPublisher;
        }
        readonly Dictionary<string, Func<object, Task<object>>> handlers = new Dictionary<string, Func<object, Task<object>>>();
        private readonly ILogger logger;
        private readonly IEventPublisher eventPublisher;

        public void AddHandler<TRequest, TResponse>(Func<IHandler<TRequest, TResponse>> handlerFactory)
            where TRequest : class, IRequest<TResponse>
            where TResponse : class
        {
            var key = typeof(TRequest).Name;
            handlers.Add(key, async (obj) =>
            {
                var req = obj as TRequest;
                var handler = handlerFactory();
                var x = await Run(handler, req);
                return x.Success ? x : x.Error;
            });
        }

        public void AddScopedHandler<TRequest, TResponse>(Func<IHandlerScope<TRequest, TResponse>> scopeFactory)
            where TRequest : class, IRequest<TResponse>
            where TResponse : class
        {
            var key = typeof(TRequest).Name;
            handlers.Add(key, async (obj) =>
            {
                var req = obj as TRequest;
                using (var scope = scopeFactory())
                {
                    var handler = scope.Create();
                    return await Run(handler, req);
                }

            });
        }

        async Task<Result<TResponse>> Run<TRequest, TResponse>(IHandler<TRequest, TResponse> handler, TRequest request)
            where TRequest : class, IRequest<TResponse>
            where TResponse : class
        {
            var x = await handler.Handle(request);
            if (x.Success)
            {
                Log(x.Value);
                Publish(x.Value);
                return x.Value;
            }
            return x.Error;
        }

        private void Publish<TResponse>(TResponse value) where TResponse : class
        {
            eventPublisher.PublishAsync(value);
        }

        private void Log<TResponse>(TResponse value) where TResponse : class
        {
        }

        public async Task<Result<TResponse>> Dispatch<TResponse>(IRequest<TResponse> request)
            where TResponse : class
        {
            var key = request.GetType().Name;
            if (handlers.ContainsKey(key))
            {
                var handler = handlers[key];
                var response = await handler(request) as Result<TResponse>;
                return response;
                //if (response.Success)
                //{
                //    return response as TResponse;
                //}
                //return response.Error;
            }

            throw new NotImplementedException();
        }

        public async Task<Result<object>> Dispatch(IRequest request)
        {
            var key = request.GetType().Name;
            if (handlers.ContainsKey(key))
            {
                var handler = handlers[key];
                var response = await handler(request);
                return response;
            }

            throw new NotImplementedException();
        }
    }
}
