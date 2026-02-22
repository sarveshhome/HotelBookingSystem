using MediatR;

namespace Common.Application;

public interface ICommand<out TResponse> : IRequest<TResponse> { }
