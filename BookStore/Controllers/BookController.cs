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
                string fileName = UploadFile(model.File)?? UploadFile(model.File);
               
                if (model.AuthorId == -1)
                {
                    ViewBag.Message = "Please select an author from the list";
                    var vmodel = new BookAuthorViewModel
                    {
                        Authors = FillSelectList(),
                    };
                    return View(vmodel);
                }
                var author = authorRepoosity.Find(model.AuthorId);
                Book book = new Book
                {
                    Id = model.BookId,
                    Title = model.Title,
                    Description = model.Description,
                    ImageUrl = fileName,
                    Author = author
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
                string fileName = UploadFile(viewModel.File,viewModel.ImageUrl) ==null?string.Empty: UploadFile(viewModel.File, viewModel.ImageUrl);
               
                Book book = new Book
                {
                    Id= viewModel.BookId,
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Author = authorRepoosity.Find(viewModel.AuthorId),
                    ImageUrl= fileName,
                };
                bookRepository.Update(viewModel.BookId, book);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
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
        string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                string fullPath = Path.Combine(uploads, file.FileName);
                file.CopyTo(new FileStream(fullPath, FileMode.Create));

                return file.FileName;
            }
            return null;
        }
        string UploadFile(IFormFile file, string imageUrl)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");

                string fullPath = Path.Combine(uploads, file.FileName);
                //@desc Delete old file
                string fullOldPath = Path.Combine(uploads, imageUrl);
                if (fullPath != imageUrl)
                {
                    System.IO.File.Delete(fullOldPath);
                    //@desc Save the new file
                    file.CopyTo(new FileStream(fullPath, FileMode.Create));

                }
                return fullPath;
            }
            else
            {
                return imageUrl;
            }

        }
        public ActionResult Search(string term)
        {
            var result = bookRepository.Search(term);
            return View("Index",result);
        }
    }
}
