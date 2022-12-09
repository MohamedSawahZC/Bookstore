namespace BookStore.Models.Repositories
{
    public class BookRepsoity : IBookStoreRepository<Book>
    {
        List<Book> books;

        public BookRepsoity()
        {
            books = new List<Book>()
            {
                new Book
                {
                    Id = 1,
                    Title="C# Programming",
                    Description="No Description"
                },
                new Book
                {
                    Id = 2,
                    Title="Python Programming",
                    Description="No Data"
                },
                new Book
                {
                    Id = 3,
                    Title="Java Programming",
                    Description="No Content"
                }
            };
        }
        public void Add(Book entity)
        {
            books.Add(entity);
        }

        public void Delete(int id)
        {
            var book = Find(id);
            books.Remove(book);
        }

        public Book Find(int id)
        {
            var book = books.SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return books;
        }

        public void Update(int id,Book newBook)
        {
            var book = Find(id);
            book.Title = newBook.Title;
            book.Description = newBook.Description;
            book.Author= newBook.Author;
        }
    }
}
