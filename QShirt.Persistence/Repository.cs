using Microsoft.EntityFrameworkCore;
using QShirt.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace QShirt.Persistence
{

    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        #region Fields

        private readonly QShirtContext context;
        protected IQueryable<T> SourceQuery;
        public Guid Id { get; set; }


        #endregion Fields

        #region Constructor

        public Repository(QShirtContext context)
        {
            SourceQuery = context.Set<T>();

            this.context = context;
        }

        #endregion Constructor

        #region Methods

        #region Queries

        /// <summary>
        /// Returns query for object by Id
        /// </summary>
        public IQueryable<T> GetByIdQuery(Guid id)
        {
            return context.Set<T>().Where(o => o.Id.Equals(id));
        }

        /// <summary>
        /// Returns query for instances by their ids
        /// </summary>
        /// <param name="ids">instance ids</param>
        public IQueryable<T> GetByIdsQuery(IEnumerable<Guid> ids)
        {
            return SourceQuery.Where(o => ids.Contains(o.Id));
        }

        /// <summary>
        /// Returns objects by their ids
        /// </summary>
        public List<T> GetByIds(List<Guid> ids)
        {
            return context.Set<T>().Where(o => ids.Contains(o.Id)).ToList();
        }


        /// <summary>
        /// Returns object by its id
        /// </summary>
        public T? GetById(Guid id)
        {
            return context.Set<T>().SingleOrDefault(o => o.Id.Equals(id));
        }

        /// <summary>
        /// Returns query for instance by its id
        /// </summary>
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await context.FindAsync<T>(id);
        }

        #endregion Queris

        #region Commands

        /// <summary>
        /// Adds object
        /// </summary>
        public void Add(T objectToAdd)
        {
            context.Set<T>().Add(objectToAdd);
        }

        /// <summary>
        /// Adds collection of objects
        /// </summary>
        public void AddRange(IEnumerable<T> objectsToAdd)
        {
            context.Set<T>().AddRange(objectsToAdd);
        }

        #endregion Commands

        #region Remove

        /// <summary>
        /// Removes instance by its id
        /// </summary>
        /// <param name="instanceId">instance id</param>
        public void Remove(Guid instanceId)
        {
            T instance = GetByIdQuery(instanceId).FirstOrDefault();
            if (instance != null)
                context.Set<T>().Remove(instance);
        }

        /// <summary>
        /// Removes instance by its id (asynchronously)
        /// </summary>
        /// <param name="instanceId">instance id</param>
        public async Task RemoveAsync(Guid instanceId)
        {
            T instance = await GetByIdQuery(instanceId).FirstOrDefaultAsync();
            if (instance != null)
                context.Set<T>().Remove(instance);
        }

        /// <summary>
        /// Removes set of instances by their ids
        /// </summary>
        /// <param name="ids">instance ids</param>
        public void RemoveRange(IEnumerable<Guid> ids)
        {
            List<T> instances = GetByIdsQuery(ids).ToList();
            context.Set<T>().RemoveRange(instances);
        }

        /// <summary>
        /// Removes set of instances by their ids (asynchronously)
        /// </summary>
        /// <param name="ids">instance ids</param>
        public async Task RemoveRangeAsync(IEnumerable<Guid> ids)
        {
            List<T> instances = await GetByIdsQuery(ids).ToListAsync();
            context.Set<T>().RemoveRange(instances);
        }

        #endregion Remove

        #endregion Methods

        #region IQueryable

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator() => SourceQuery.GetEnumerator();

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable"></see> is executed.</summary>
        /// <returns>A <see cref="T:System.Type"></see> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.</returns>
        public Type ElementType => SourceQuery.ElementType;

        /// <summary>Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable"></see>.</summary>
        /// <returns>The <see cref="T:System.Linq.Expressions.Expression"></see> that is associated with this instance of <see cref="T:System.Linq.IQueryable"></see>.</returns>
        public Expression Expression => SourceQuery.Expression;

        /// <summary>Gets the query provider that is associated with this data source.</summary>
        /// <returns>The <see cref="T:System.Linq.IQueryProvider"></see> that is associated with this data source.</returns>
        public IQueryProvider Provider => SourceQuery.Provider;


        #endregion IQueryable

        #region IAsyncEnumerable

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return ((IAsyncEnumerable<T>)SourceQuery).GetAsyncEnumerator(cancellationToken);
        }

        #endregion IAsyncEnumerable
    }
}
