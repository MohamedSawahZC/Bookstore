using BookStore.Models;
using BookStore.Models.Repositories;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookStoreRepository<Book> bookRepository;
        private readonly IBookStoreRepository<Author> authorRepoosity;
        private readonly IHostingEnvironment hosting;


        public BookController(IBookStoreRepository<Book> bookRepository,IBookStoreRepository<Author> authorRepoosity,IHostingEnvironment hosting)
        {
            this.bookRepository = bookRepository;
            this.authorRepoosity = authorRepoosity;
            this.hosting = hosting;
        }
        // GET: BookController
        public ActionResult Index()
        {
            var books = bookRepository.List();
            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList(),
            };
            return View(model);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            try
            {
                string fileName = string.Empty;
                if (model.File != null)
                {
                    string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                    fileName = model.File.FileName;
                    string fullPath=Path.Combine(uploads, fileName);
                    model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                }
                if (model.AuthorId == -1)
                {
                    ViewBag.Message = "Please select an author from the list";
                    var vmodel = new BookAuthorViewModel
                    {
                        Authors = FillSelectList(),
                    };
                    return View(vmodel);
                }
                Book book = new Book
                {
                    Id = model.BookId,
                    Title = model.Title,
                    Description = model.Description,
                    ImageUrl = fileName,
                    Author = authorRepoosity.Find(model.AuthorId)
                };
                bookRepository.Add(book);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookRepository.Find(id);
            var authorId = book.Author == null ? 0: book.Author.Id;

            var viewModel = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = authorId,
                Authors = authorRepoosity.List().ToList(),
                ImageUrl=book.ImageUrl,
            };
            return View(viewModel);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,BookAuthorViewModel viewModel)
        {
            try
            {
                string fileName = string.Empty;
                if (viewModel.File != null)
                {
                    string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                    fileName = viewModel.File.FileName;
                    string fullPath = Path.Combine(uploads, fileName);
                    //@desc Delete old file
                    string oldFileName = bookRepository.Find(viewModel.BookId).ImageUrl;
                    string fullOldPath = Path.Combine(uploads, oldFileName);
                    if (fullPath != oldFileName)
                    {
                        System.IO.File.Delete(fullOldPath);
                        //@desc Save the new file
                        viewModel.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                    }

                }
                Book book = new Book
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Author = authorRepoosity.Find(viewModel.AuthorId),
                    ImageUrl= fileName,
                };
                bookRepository.Update(id,book);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Book book)
        {
            try
            {
               
                var ImageUrl = book.ImageUrl;

                if(ImageUrl != null)
                {
                    string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                    
                    var fullPath = Path.Combine(uploads, ImageUrl);
                    System.IO.File.Delete(fullPath);
                }
                bookRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        List<Author> FillSelectList()
        {
            var authors = authorRepoosity.List().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = " -- Please Select an author -- " });
            return authors;
        }
    }
}
