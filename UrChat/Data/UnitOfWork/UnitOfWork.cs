using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UrChat.Data.Models;
using UrChat.Data.Repositories;

namespace UrChat.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        
        private Repository<User> _users;
        public IRepository<User> Users => _users ?? (_users = new Repository<User>(_context));

        private Repository<Message> _messages;
        public IRepository<Message> Messages => _messages ?? (_messages = new Repository<Message>(_context));

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        
        
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
        
        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void Save()
        {
            const int attemptCount = 3;

            int attemptsLeft = attemptCount;
            bool ok = false;

            while (--attemptsLeft > 0 && !ok)
            {
                try
                {
                    _context.SaveChanges();
                    ok = true;
                }
                catch
                {
                    Debug.WriteLine($"{nameof(UnitOfWork)}.{nameof(Save)}: Failed to save changes");
                }
            }

            if (!ok)
            {
                throw new Exception($"Could not save entity in {nameof(UnitOfWork)}.{nameof(Save)} after {attemptCount} retries.");
            }
        }
        
        public async Task SaveAsync()
        {
            const int attemptCount = 3;

            int attemptsLeft = attemptCount;
            bool ok = false;

            string lastError = "";
            while (--attemptsLeft > 0 && !ok)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    ok = true;
                }
                catch (Exception e)
                {
                    lastError = e.ToString();
                    Debug.WriteLine($"{nameof(UnitOfWork)}.{nameof(SaveAsync)}: Failed to save changes: {lastError}");
                }
            }

            if (!ok)
            {
                throw new Exception($"Could not save entity in {nameof(UnitOfWork)}.{nameof(SaveAsync)} after {attemptCount} retries: {lastError}");
            }
        }

        
        public void AddManyToManyAssociactions<TSubject, MTM, TObject>(
            TSubject subject, 
            IRepository<MTM> mtmRepo, 
            IRepository<TObject> objectRepo, 
            Func<MTM, bool> removeExistingFilter, 
            Func<TObject, bool> objectFilter,
            Func<TSubject, TObject, MTM, bool> mtmFilter,
            Func<TSubject, TObject, MTM> createMtm
        ) 
            where MTM : IModelBase
            where TObject : IModelBase
        {
            // First we remove existing associations according 
            // to a given filter.
            mtmRepo.GetAsEnumerable()
                .Where(removeExistingFilter)
                .Select(mtm => mtm.Id)
                .ToList()
                .ForEach(mtmRepo.Remove);
            
            // Get Object models that we want to set for Subject.
            var objects = objectRepo.GetAsEnumerable();
            var objectsToAdd = objects.Where(objectFilter);
            
            // Create an association for each object we want to add
            // to the subject.
            objectsToAdd.ToList().ForEach(@object =>
            {
                var mtmAsocciation = createMtm(subject, @object); 
                mtmRepo.Add(mtmAsocciation);                        
            });
        }
        
        public async Task<TObject> GetFirstOrCreateAsync<TObject>(
            IRepository<TObject> objects, 
            Func<TObject, bool> objectFilter,
            Func<TObject> createObject,
            Action<TObject> onCreate
        ) 
            where TObject : IModelBase
        {
            var objectOrDefault = await objects
                .GetAsQueryable(o => objectFilter(o))
                .FirstOrDefaultAsync();

            if (objectOrDefault != null)
            {
                return objectOrDefault;
            }
            
            var obj = createObject();
            onCreate(obj);
            return obj;
        }
    }
}
