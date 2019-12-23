using System.Threading.Tasks;

namespace Interfaces
{
    public interface IRepositoryWrapper
    {
        IBookRepository Book { get; }

        void Save();

        Task SaveAsync();
    }
}
