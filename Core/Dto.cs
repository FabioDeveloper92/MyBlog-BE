using System;

namespace Infrastructure.Core
{
    public abstract class Dto
    {
        public Guid Id { get; }

        protected Dto(Guid id)
        {
            Id = (id == Guid.Empty) ? Guid.NewGuid() : id;
        }
    }
}
