using KSE.Core.Messages;
using System;
using System.Collections.Generic;

namespace KSE.Core.DomainObjets
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public DateTime InsertDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
            
        }

        private List<Event> _notification;
        public IReadOnlyCollection<Event> Notification => _notification?.AsReadOnly();
        public void AddEvent(Event _event)
        {
            _notification = _notification ?? new List<Event>();

            _notification.Add(_event);
        }
        public void RemoveEvent(Event _event)
        {
            _notification?.Remove(_event);
        }
        public void DisposeEvent()
        {
            _notification?.Clear();
        }

        #region Comparações
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
        #endregion

        public virtual bool IsValid()
        {
            return true;
        }
    }
}
