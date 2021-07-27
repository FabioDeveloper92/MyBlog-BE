using MediatR;

namespace Application.Interfaces
{
    public interface IQuery<out T> : IRequest<T>
    {
    }
}
