using System;
using System.Collections.Generic;

namespace Config
{
    public class BaseConfig<T>
    {
        public T Value { get; }

        protected BaseConfig(T value)
        {
            if (value == null)
                throw new Exception($"Value is null for {GetType().Name}");

            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;

            return ((BaseConfig<T>)obj).Value.Equals(Value);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }
    }
}