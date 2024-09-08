using MediatR;

namespace Movie.BuildingBlocks.CQRS;

    public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull
    {
    }
