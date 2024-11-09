public interface IBookService
{
    Task<PagedResponse<IEnumerable<BookReadInfo>>> GetBooksAsync(BookFilter filter);
    Task<BookReadInfo?> GetBookByIdAsync(int id);
    Task<bool> CreateBookAsync(BookCreateInfo book);
    Task<bool> UpdateBookAsync(BookUpdateInfo book);
    Task<bool> DeleteBookAsync(int id);
}