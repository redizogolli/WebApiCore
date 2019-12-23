using Entities.Helpers;
using Entities.Models;
using Interfaces;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private BookRepository _book;
        private readonly ISortHelper<Book> _sortHelper;
        private readonly RepositoryContext _repoContext;
        private readonly IDataShaper<Book> _dataShaper;

        public RepositoryWrapper(RepositoryContext repositoryContext, ISortHelper<Book> sortHelper, IDataShaper<Book> dataShaper)
        {
            _repoContext = repositoryContext;
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }
        public IBookRepository Book
        {
            get
            {
                if (_book == null)
                    _book = new BookRepository(_repoContext, _sortHelper, _dataShaper);
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
