using Entities.Helpers;
using Entities.Models;
using Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Repository
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        private readonly ISortHelper<Book> _sortHelper;
        private readonly IDataShaper<Book> _dataShaper;
        public BookRepository(RepositoryContext repositoryContext, ISortHelper<Book> sortHelper, IDataShaper<Book> dataShaper) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public void AddBook(Book book)
        {
            Create(book);
        }

        public async Task AddBookAsync(Book book)
        {
            await CreateAsync(book);
        }

        public void DeleteBook(Book book)
        {
            Delete(book);
        }

        public Book GetBook(int id)
        {
            return FindByCondition(x => x.Id == id).FirstOrDefault();
        }

        public async Task<Entity> GetBookAsync(int id, string fields)
        {
            var book = await FindByConditionAsync(x => x.Id == id);

            return _dataShaper.ShapeData(book.FirstOrDefault(), fields);
        }

        public IEnumerable<Book> GetBooks(BookParameters parameters)
        {
            return FindAll()
                .OrderBy(x => x.Name)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(BookParameters parameters)
        {
            var books = await FindAllAsync();
            return books
                .OrderBy(on => on.Name)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();
        }

        public void UpdateBook(Book book)
        {
            Update(book);
        }

        /// <summary>
        /// Getting books with pagination
        /// </summary>
        public PagedList<Book> GetBooksWithPagination(BookParameters parameters)
        {
            return PagedList<Book>.ToPagedList(FindAll().OrderBy(on => on.Name),
                parameters.PageNumber,
                parameters.PageSize);
        }

        /// <summary>
        /// Getting books async with pagination
        /// </summary>
        public async Task<PagedList<Entity>> GetBooksWithPaginationAsync(BookParameters parameters)
        {
            var books = await FindAllAsync();

            Search(ref books, parameters.Name, parameters.Author);

            books = _sortHelper.ApplySort(books, parameters);

            var shapedBooks = _dataShaper.ShapeData(books, parameters.Fields);

            return PagedList<Entity>.ToPagedList(shapedBooks,
                parameters.PageNumber,
                parameters.PageSize);
        }

        /// <summary>
        /// searching
        /// </summary>
        private void Search(ref IEnumerable<Book> books, string name, string author)
        {
            if (!books.Any() || (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(author)))
                return;

            if (string.IsNullOrWhiteSpace(name))// has only author
            {
                books = books.Where(o => o.Author.ToLower().Contains(author.Trim().ToLower()));
                return;
            }

            if (string.IsNullOrWhiteSpace(author))//author is empty
            {
                books = books.Where(o => o.Name.ToLower().Contains(name.Trim().ToLower()));
                return;
            }
            //both fields are completed
            books = books.Where(o => o.Name.ToLower().Contains(name.Trim().ToLower()) && o.Author.ToLower().Contains(author.Trim().ToLower()));
        }
    }
}
