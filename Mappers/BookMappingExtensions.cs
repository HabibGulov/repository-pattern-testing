public static class BookMappingExtensions
{
    public static BookReadInfo ToReadInfo(this Book book)
    {
        return new ()
        {
            Id=book.Id,
            BookInfo = new()
            {
                Title=book.Title,
                Author=book.Author
            }
            
        };
    }

    public static Book ToBook(this BookCreateInfo createInfo)
    {
        return new Book()
        {
            Title=createInfo.BookInfo.Title,
            Author=createInfo.BookInfo.Author
        };
    }

    public static Book UpdateBook(this Book book, BookUpdateInfo updateInfo)
    {
        book.Version+=1;
        book.UpdatedAt=DateTime.UtcNow;
        book.Author=updateInfo.BookInfo.Author;
        book.Title=updateInfo.BookInfo.Title;
        return book;
    }

    public static Book DeleteBook(this Book book)
    {
        book.Version+=1;
        book.IsDeleted=true;
        book.UpdatedAt=DateTime.UtcNow;
        book.DeletedAt=DateTime.UtcNow;
        return book;
    }
}