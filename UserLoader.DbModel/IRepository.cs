using LanguageExt;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using UserLoader.DbModel.Entities;

namespace UserLoader.DbModel
{
    public interface IRepository<T> where T : AbstractEntity
    {
        TryOption<T> Find(string id);
        Try<IEnumerable<T>> All { get; }
        Try<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate);
        TryAsync<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
        TryAsync<IEnumerable<T>> GetAsEnumerableAsync(Func<T, bool> predicate);
        Try<Unit> Insert(T entity);
        TryAsync<Unit> InsertAsync(T entity);
        Try<Unit> InsertRange(IEnumerable<T> entities);
        TryAsync<Unit> InsertRangeAsync(IEnumerable<T> entities);
    }
}