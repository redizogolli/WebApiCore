using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        IEnumerable<Book> GetBooks(BookParameters parameters);

        Book GetBook(int id);

        void AddBook(Book book);

        void UpdateBook(Book book);

        void DeleteBook(Book book);

        Task<IEnumerable<Book>> GetBooksAsync(BookParameters parameters);

        Task<Book> GetBookAsync(int id);

        Task AddBookAsync(Book book);

        Task<PagedList<Book>> GetBooksWithPaginationAsync(BookParameters parameters);

        PagedList<Book> GetBooksWithPagination(BookParameters parameters);
    }
}
