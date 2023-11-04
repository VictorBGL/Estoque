using Microsoft.Graph.Models;

namespace Estoque.Core.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }


        protected EntityBase()
        {
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }
    }

    public class EntityComparer<T> : IEqualityComparer<T> where T : EntityBase
    {
        public bool Equals(T x, T y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(T obj)
        {
            int hCode = obj.Id.GetHashCode();
            return hCode;
        }
    }

    public class EntityBaseEqualityComparer : IEqualityComparer<EntityBase>
    {
        public bool Equals(EntityBase x, EntityBase y)
        {
            return x.GetType() == y.GetType() && x.Id == y.Id;
        }

        public int GetHashCode(EntityBase obj)
        {
            int hCode = obj.GetType().GUID.GetHashCode();
            return hCode.GetHashCode();
        }
    }
}
