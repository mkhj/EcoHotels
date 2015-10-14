using System;
using System.Collections.Generic;
using System.Linq;
using EcoHotels.Core.Domain;

namespace EcoHotels.Core.Infrastructure
{
    [Serializable]
    public abstract class EntityBase<T>
    {
        private readonly List<BusinessRule> brokenRules = new List<BusinessRule>();

        //public virtual Guid Id { get; protected internal set; }

        public virtual int Id { get; protected internal set; }

        protected void AddBrokenRule(BusinessRule businessRule)
        {
            brokenRules.Add(businessRule);
        }

        public virtual List<BusinessRule> GetBrokenRules()
        {
            brokenRules.Clear();
            Validate();
            return brokenRules;
        }

        public virtual bool IsValid()
        {
            return GetBrokenRules().Count() == 0;
        }

        protected abstract void Validate();

        #region - Equality -

        public override bool Equals(object other)
        {
            if (ReferenceEquals(this, other)) return true;

            if (!(other is EntityBase<T>)) return false;

            // fixes the issue with SET as a relation type when saving
            // if you add two new items to a collection they will be considered as equal
            // and only one of them will get saved.
            if (Id == 0) return false;

            var that = (EntityBase<T>)other;
            return Id == that.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region - Operators -

        public static bool operator ==(EntityBase<T> entity1, EntityBase<T> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
            {
                return true;
            }

            if ((object)entity1 == null || (object)entity2 == null)
            {
                return false;
            }

            if (entity1.Id.ToString() == entity2.Id.ToString())
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(EntityBase<T> entity1, EntityBase<T> entity2)
        {
            return (!(entity1 == entity2));
        }

        #endregion
    }

    [Serializable]
    public abstract class EntityIdentityBase<T>
    {
        private readonly List<BusinessRule> brokenRules = new List<BusinessRule>();

        public virtual int Id { get; protected internal set; }

        protected void AddBrokenRule(BusinessRule businessRule)
        {
            brokenRules.Add(businessRule);
        }

        public virtual List<BusinessRule> GetBrokenRules()
        {
            brokenRules.Clear();
            Validate();
            return brokenRules;
        }

        public virtual bool IsValid()
        {
            return GetBrokenRules().Count() == 0;
        }

        protected abstract void Validate();

        #region - Equality -

        public override bool Equals(object other)
        {
            if (ReferenceEquals(this, other)) return true;

            if (!(other is EntityIdentityBase<T>)) return false;

            // fixes the issue with SET as a relation type when saving
            // if you add two new items to a collection they will be considered as equal
            // and only one of them will get saved.
            if (Id == 0) return false;

            var that = (EntityIdentityBase<T>)other;
            return Id == that.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region - Operators -

        public static bool operator ==(EntityIdentityBase<T> entity1, EntityIdentityBase<T> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
            {
                return true;
            }

            if ((object)entity1 == null || (object)entity2 == null)
            {
                return false;
            }

            if (entity1.Id.ToString() == entity2.Id.ToString())
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(EntityIdentityBase<T> entity1, EntityIdentityBase<T> entity2)
        {
            return (!(entity1 == entity2));
        }

        #endregion
    }
}
