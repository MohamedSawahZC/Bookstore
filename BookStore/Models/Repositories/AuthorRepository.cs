namespace BookStore.Models.Repositories
{
    public class AuthorRepository : IBookStoreRepository<Author>
    {

        IList<Author> authors;
        public AuthorRepository()
        {
            authors = new List<Author>()
            {
                new Author
                {
                    Id = 1,
                    FullName="Mohamed Sawah"
                },
                new Author
                {
                    Id = 2,
                    FullName="Ahmed Sawah"
                },
                new Author
                {
                    Id = 3,
                    FullName="Ali Sawah"
                },
            };
        }
        public void Add(Author entity)
        {
            authors.Add(entity);
        }

        public void Delete(int id)
        {
            var author = Find(id);
            authors.Remove(author);
        }

        public Author Find(int id)
        {
            var author = authors.SingleOrDefault(b => b.Id == id);
            return author;
        }

        public IList<Author> List()
        {
           return authors;
        }

        public void Update(int id, Author entity)
        {
            var author = Find(id);
            author.FullName = entity.FullName;
        }
    }
}
