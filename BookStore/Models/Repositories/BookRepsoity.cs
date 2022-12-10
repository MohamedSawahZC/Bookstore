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
                    Description="No Description",
                    Author = new Author
                    {
                        Id=1
                    }
                },
                new Book
                {
                    Id = 2,
                    Title="Python Programming",
                    Description="No Data",
                    Author = new Author
                    {
                        Id=2
                    }
                },
                new Book
                {
                    Id = 3,
                    Title="Java Programming",
                    Description="No Content",
                    Author = new Author
                    {
                        Id=3
                    }
                }
            };
        }
        public void Add(Book entity)
        {
            entity.Id = List().Count+1;
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
            book.ImageUrl = newBook.ImageUrl;
        }
    }
}
