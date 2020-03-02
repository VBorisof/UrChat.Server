using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UrChat.Data.Models;

namespace UrChat.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : ModelBase
    {
        private readonly ApplicationDbContext _context;


        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }


        public T GetOrDefault(long id)
        {
            return _context.Set<T>().Find(id);
        }

        public IQueryable<T> GetAsQueryable(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public IEnumerable<T> GetAsEnumerable()
        {
            IQueryable<T> query = _context.Set<T>();
            return query.ToList();
        }

        public Task<List<T>> GetAsEnumerableAsync()
        {
            IQueryable<T> query = _context.Set<T>();
            return query.ToListAsync();
        }

        public T Add(T item)
        {
            return _context.Set<T>().Add(item).Entity;
        }

        public async Task<T> AddAsync(T item)
        {
            return (await _context.Set<T>().AddAsync(item)).Entity;    
        }

        public void Remove(long id)
        {
            _context.Set<T>().Remove(GetOrDefault(id));
        }

        public void Update(T item)
        {
            _context.Set<T>().Update(item);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool Any(Func<T, bool> filter = null)
        {
            var query = _context.Set<T>();
            return filter != null ? query.Any(filter) : query.Any();
        }

        public Task<bool> AnyAsync(Func<T, bool> filter = null)
        {
            var query = _context.Set<T>();
            return filter != null ? query.AnyAsync(x => filter(x)) : query.AnyAsync();
        }
        
        async Task<T> SingleOrDefaultAsync(Func<T, bool> objectFilter)
        {
            return await GetAsQueryable().SingleOrDefaultAsync(o => objectFilter(o));
        }
    }
}