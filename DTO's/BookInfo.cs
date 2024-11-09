public readonly record struct BookBaseInfo(
    string Title,
    string Author
);

public record struct BookCreateInfo(BookBaseInfo BookInfo);

public record struct BookUpdateInfo(int Id, BookBaseInfo BookInfo);

public readonly record struct BookReadInfo(int Id, BookBaseInfo BookInfo);