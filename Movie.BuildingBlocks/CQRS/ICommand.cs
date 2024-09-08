using MediatR;

namespace Movie.BuildingBlocks.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse>
    where TResponse : notnull
{
}

public interface ICommand : ICommand<Unit>
{
}
