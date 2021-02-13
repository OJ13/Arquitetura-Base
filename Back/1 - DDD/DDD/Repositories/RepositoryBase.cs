using DDD.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DDD.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
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
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly ILogger _logger;
        private readonly EFContext _context;
        public RepositoryBase(EFContext context)
        {
            _context = context;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).ToList();
        }

        public TEntity GetById(long id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public TEntity GetById(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public TEntity Add(TEntity obj)
        {
            var Tentity = _context.Set<TEntity>().Add(obj);
            _context.SaveChanges();

            return Tentity.Entity;
        }
        public TEntity Update(TEntity obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
            _context.SaveChanges();

            return obj;
        }
        public void Remove(TEntity obj)
        {
            _context.Set<TEntity>().Remove(obj);
            _context.SaveChanges();
        }

        public void RemoveAll(Expression<Func<TEntity, bool>> predicate)
        {
            var items = _context.Set<TEntity>().Where(predicate).ToList();
            _context.Set<TEntity>().RemoveRange(items);
            _context.SaveChanges();
        }
    }
}
