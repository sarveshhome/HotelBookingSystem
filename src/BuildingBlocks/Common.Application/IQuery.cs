using MediatR;

namespace Common.Application;

public interface IQuery<out TResponse> : IRequest<TResponse> { }
