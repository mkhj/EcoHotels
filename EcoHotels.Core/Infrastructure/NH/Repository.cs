using System;
using System.Collections.Generic;
using EcoHotels.Core.Domain;
using NHibernate;
using NHibernate.Criterion;

namespace EcoHotels.Core.Infrastructure.NH
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// http://nhforge.org/blogs/nhibernate/archive/2008/08/31/data-access-with-nhibernate.aspx
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> //where T : IAggregateRoot
    {
        public ISession Session
        {
            get
            {
                return SessionFactory.GetCurrentSession();
                //return SessionScopeWebModule.GetCurrentSession();
            }
        }

        /// <summary>
        /// Retrieves the entity with the given id
        /// </summary>
        /// <param name="key"></param>
        /// <returns>the entity or null if it doesn't exist</returns>
        public T FindBy(int key)
        {
            return Session.Get<T>(key);
        }

        /// <summary>
        /// Retrieves the entity with the given id
        /// </summary>
        /// <param name="key"></param>
        /// <returns>the entity or null if it doesn't exist</returns>
        public T FindBy(IEnumerable<int> key)
        {
            return Session.Get<T>(key);
        }

        /// <summary>
        /// Returns the one entity that matches the given criteria. Throws an exception if
        /// more than one entity matches the criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public T FindOne(DetachedCriteria criteria)
        {
            return criteria.GetExecutableCriteria(Session).UniqueResult<T>();
        }

        /// <summary>
        /// Returns the first entity to match the given criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public T FindFirst(DetachedCriteria criteria)
        {
            var results = criteria.SetFirstResult(0)
                .SetMaxResults(1)
                .GetExecutableCriteria(Session).List<T>();

            if (results.Count > 0)
            {
                return results[0];
            }

            return default(T);
        }

        /// <summary>
        /// Returns the first entity to match the given criteria, ordered by the given order
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public T FindFirst(DetachedCriteria criteria, Order order)
        {
            return FindFirst(criteria.AddOrder(order));
        }

        public IEnumerable<T> FindAll()
        {
            var criteria = DetachedCriteria.For(typeof(T)).SetResultTransformer(CriteriaSpecification.DistinctRootEntity);
            return FindAll(criteria);
        }

        /// <summary>
        /// Returns each entity that matches the given criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IEnumerable<T> FindAll(DetachedCriteria criteria)
        {
            return criteria.GetExecutableCriteria(Session).List<T>();
        }

        /// <summary>
        /// Returns each entity that maches the given criteria, and orders the results
        /// according to the given Orders
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public IEnumerable<T> FindAll(DetachedCriteria criteria, params Order[] orders)
        {
            if (orders != null)
            {
                foreach (var order in orders)
                {
                    criteria.AddOrder(order);
                }
            }

            return FindAll(criteria);
        }

        /// <summary>
        /// Returns each entity that matches the given criteria, with support for paging,
        /// and orders the results according to the given Orders
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="firstResult"></param>
        /// <param name="numberOfResults"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public IEnumerable<T> FindAll(DetachedCriteria criteria, int firstResult, int numberOfResults, params Order[] orders)
        {
            criteria.SetFirstResult(firstResult).SetMaxResults(numberOfResults);
            return FindAll(criteria, orders);
        }

        /// <summary>
        /// Returns the total number of entities that match the given criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public long Count(DetachedCriteria criteria)
        {
            return Convert.ToInt64(criteria.GetExecutableCriteria(Session)
                .SetProjection(Projections.RowCountInt64()).UniqueResult());
        }

        /// <summary>
        /// Returns true if at least one entity exists that matches the given criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public bool Exists(DetachedCriteria criteria)
        {
            return Count(criteria) > 0;
        }

        public void Save(T item)
        {
            Session.SaveOrUpdate(item);
            Session.Flush();
        }

        public void Remove(T item)
        {
            Session.Delete(item);
        }
    }
}

