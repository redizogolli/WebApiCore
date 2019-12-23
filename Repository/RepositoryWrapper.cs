using Entities.Helpers;
using Entities.Models;
using Interfaces;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private BookRepository _book;
        private ISortHelper<Book> _sortHelper;
        private readonly RepositoryContext _repoContext;

        public RepositoryWrapper(RepositoryContext repositoryContext, ISortHelper<Book> sortHelper)
        {
            _repoContext = repositoryContext;
            _sortHelper = sortHelper;
        }
        public IBookRepository Book
        {
            get
            {
                if (_book == null)
                    _book = new BookRepository(_repoContext, _sortHelper);
                return _book;
            }
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}
