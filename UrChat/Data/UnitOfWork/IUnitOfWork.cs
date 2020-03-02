using System.Threading.Tasks;
using UrChat.Data.Models;
using UrChat.Data.Repositories;

namespace UrChat.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Save();
        Task SaveAsync();

        IRepository<User> Users { get; }
        IRepository<Message> Messages { get; }
    }
}