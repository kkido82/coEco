using System;

namespace CoEco.Core.Interfaces
{
    public interface IHandlerScope<TRequest, TResponse> : IDisposable
            where TRequest : class, IRequest<TResponse>
            where TResponse : class
    {
        IHandler<TRequest, TResponse> Create();
    }
}
