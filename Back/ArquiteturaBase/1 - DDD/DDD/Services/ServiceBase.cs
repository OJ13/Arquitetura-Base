using DDD.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DDD.Services
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        TEntity GetById(long id);
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        TEntity Add(TEntity obj);
        TEntity Update(TEntity obj);
        void Remove(TEntity obj);
        void RemoveAll(Expression<Func<TEntity, bool>> predicate);
    }
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class
    {
        private readonly IRepositoryBase<TEntity> _repositoryBase;
        public ServiceBase(IRepositoryBase<TEntity> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public TEntity Add(TEntity obj)
        {
            return _repositoryBase.Add(obj);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _repositoryBase.GetAll();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _repositoryBase.GetAll(predicate);
        }

        public TEntity GetById(long id)
        {
            return _repositoryBase.GetById(id);
        }

        public TEntity GetById(int id)
        {
            return _repositoryBase.GetById(id);
        }

        public void Remove(TEntity obj)
        {
            _repositoryBase.Remove(obj);
        }

        public TEntity Update(TEntity obj)
        {
            return _repositoryBase.Update(obj);
        }

        public void RemoveAll(Expression<Func<TEntity, bool>> predicate)
        {
            _repositoryBase.RemoveAll(predicate);
        }
    }
}
