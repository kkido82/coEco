using CoEco.Core.Eventing;
using CoEco.Core.Ordering.Dto.Responses;
using System;
using System.Threading.Tasks;

namespace CoEco.Core.Interfaces
{
    public interface IHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        Task<Result<TResponse>> Handle(TRequest request);
    }
}
