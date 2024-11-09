
using System.Linq.Expressions;

public class BookService(IUnitOfWork<Book> unitOfWork) : IBookService
{
    public async Task<bool> CreateBookAsync(BookCreateInfo book)
    {
        IGenericAddRepository<Book> repository = unitOfWork.AddRepository;
        IGenericFindRepository<Book> findRepository = unitOfWork.FindRepository;

        bool check = (await findRepository.FindAsync(x => x.Title.ToLower().Contains(book.BookInfo.Title.ToLower()) && x.Author.ToLower().Contains(book.BookInfo.Author.ToLower()))).Any();

        if (check) return false;

        await repository.AddAsync(book.ToBook());
        return await unitOfWork.Complete() != 0;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        IGenericDeleteRepository<Book> repository = unitOfWork.DeleteRepository;
        IGenericFindRepository<Book> findRepository = unitOfWork.FindRepository;

        Book? book = await findRepository.GetByIdAsync(id);
        if (book == null) return false;
        repository.Delete(book);
        return await unitOfWork.Complete() != 0;
    }

    public async Task<BookReadInfo?> GetBookByIdAsync(int id)
    {
        IGenericFindRepository<Book> repository = unitOfWork.FindRepository;
        Book? book = await repository.GetByIdAsync(id);
        return book?.ToReadInfo();
    }

    public async Task<PagedResponse<IEnumerable<BookReadInfo>>> GetBooksAsync(BookFilter filter)
    {
        IGenericFindRepository<Book> repository = unitOfWork.FindRepository;

        Expression<Func<Book, bool>> filterExpression = book =>
            (string.IsNullOrEmpty(filter.Name) || book.Title.Contains(filter.Name));

        IEnumerable<Book> query = (await repository
            .FindAsync(filterExpression)).ToList();

        int count = query.Count();

        IEnumerable<BookReadInfo> books = query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => x.ToReadInfo())
            .ToList();

        return PagedResponse<IEnumerable<BookReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, books);
    }

    public async Task<bool> UpdateBookAsync(BookUpdateInfo book)
    {
        IGenericUpdateRepository<Book> repository = unitOfWork.UpdateRepository;
        IGenericFindRepository<Book> findRepository = unitOfWork.FindRepository;

        Book? book1 = await findRepository.GetByIdAsync(book.Id);
        if (book.BookInfo.Author == null && book.BookInfo.Title == null) return false;
        repository.Update(book1!.UpdateBook(book));
        return await unitOfWork.Complete() != 0;
    }
}
