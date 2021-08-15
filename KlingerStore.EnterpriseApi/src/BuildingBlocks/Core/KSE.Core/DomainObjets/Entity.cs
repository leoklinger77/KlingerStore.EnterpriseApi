using System;

namespace KSE.Core.DomainObjets
{
    public abstract class Entity
    {
        public Guid Id { get; set; }               

        protected Entity()
        {
            Id = Guid.NewGuid();
            
        }        
        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }       

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 956) + Id.GetHashCode();
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
