using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using LanguageExt;
using LanguageExt.Common;

using MongoDB.Driver;

using UserLoader.DbModel.Entities;

namespace UserLoader.DbModel
{
    public class Repository<T> : IRepository<T> where T : AbstractEntity
    {
        private readonly IMongoCollection<T> _entities;
        private readonly UnitOfWork _unitOfWork;

        public Repository(IMongoCollection<T> entities, UnitOfWork unitOfWork)
        {
            _entities = entities;
            _unitOfWork = unitOfWork;
        }

        public TryOption<T> Find(string id) => () => _entities.Find(entity => entity.Id.ToString() == id).FirstOrDefault();

        public Try<IEnumerable<T>> All => () => new Result<IEnumerable<T>>(_entities.AsQueryable().AsEnumerable());

        public Try<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate) => () => _entities.Find(predicate).ToList();

        public TryAsync<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate) =>
            () =>
            {
                try
                {
                    return new Result<IEnumerable<T>>(_entities.Find(predicate).ToList()).AsTask();
                }
                catch (Exception ex)
                {
                    return new Result<IEnumerable<T>>(ex).AsTask();
                }
            };

        public TryAsync<IEnumerable<T>> GetAsEnumerableAsync(Func<T, bool> predicate) =>
            () =>
            {
                try
                {
                    return new Result<IEnumerable<T>>(_entities.AsQueryable().AsEnumerable().Where(predicate)).AsTask();
                }
                catch (Exception ex)
                {
                    return new Result<IEnumerable<T>>(ex).AsTask();
                }
            };

        public Try<Unit> Insert(T entity) => () =>
        {
            if (entity == null)
            {
                return Unit.Default;
            }

            _entities.InsertOne(_unitOfWork.Session, entity);

            return Unit.Default;
        };

        public TryAsync<Unit> InsertAsync(T entity) => async () =>
        {
            if (entity == null)
            {
                return Unit.Default;
            }

            await _entities.InsertOneAsync(_unitOfWork.Session, entity);

            return Unit.Default;
        };

        public Try<Unit> InsertRange(IEnumerable<T> entities) => () =>
        {
            _entities.InsertMany(_unitOfWork.Session, entities);

            return Unit.Default;
        };

        public TryAsync<Unit> InsertRangeAsync(IEnumerable<T> entities) => async () =>
        {
            await _entities.InsertManyAsync(_unitOfWork.Session, entities);
            return Unit.Default;
        };

        private Expression<Func<T, bool>> IdPredicate(string id) => entity => entity.Id == id;
    }
}