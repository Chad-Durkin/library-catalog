using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace LibraryCatalog
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => {
                Dictionary<string, object> model = new Dictionary<string, object>{};
                model.Add("books", Book.GetAll());
                return View["index.cshtml", model];
            };
            Get["/books/add"] = _ => {
                return View["books_add.cshtml"];
            };
            Post["/books/add"] = _ => {
                Book newBook = new Book(Request.Form["title"], Request.Form["publication-date"]);
                newBook.Save();
                if(Request.Form["author"] != "Unkown")
                {
                    Author newAuthor = new Author(Request.Form["author"]);
                    newAuthor.Save();
                    newBook.AddAuthor(newAuthor.GetId());
                }
                Copy newCopy = new Copy(newBook.GetId());
                newCopy.Save();
                Dictionary<string, object> model = new Dictionary<string, object>{};
                model.Add("books", Book.GetAll());
                return View["index.cshtml", model];
            };
            Get["/book/{id}"] = parameters => {
                Book foundBook = Book.FindById(parameters.id);
                return View["book.cshtml", foundBook];
            };
            Patch["/book/update/{id}"] = parameters => {
                Book foundBook = Book.FindById(parameters.id);
                foundBook.UpdateTitle(Request.Form["title"]);
                // Dictionary<string, object> model = new Dictionary<string, object>{};
                // model.Add("books", Book.GetAll());
                Book newfoundBook = Book.FindById(parameters.id);
                return View["book.cshtml", newfoundBook];
            };
            Delete["/book/delete/{id}"] = parameters => {
                Book foundBook = Book.FindById(parameters.id);
                foundBook.DeleteBook();
                Dictionary<string, object> model = new Dictionary<string, object>{};
                model.Add("books", Book.GetAll());
                return View["index.cshtml", model];
            };
            Get["/books/search"] = _ => {
                Dictionary<string, object> model = new Dictionary<string, object>{};
                model.Add("books", Book.GetAll());
                return View["books_search.cshtml", model];
            };
            Get["/book/search/title"] = _ => {
                Book foundBook = Book.FindByTitle(Request.Query["title"]);
                return View["book.cshtml", foundBook];
            };
            Get["/book/search/author"] = _ => {
                Book foundBook = Book.FindByAuthorName(Request.Query["author"]);
                return View["book.cshtml", foundBook];
            };
            Get["/books/checkout"] = _ => {
                List<Book> allBooks = Book.GetAll();
                return View["checkout_book.cshtml", allBooks];
            };
            Post["/books/checkout"] = _ => {
                Dictionary<string, object> model = new Dictionary<string, object>{};
                bool checkedOutSuccess = false;
                Patron newPatron = new Patron(Request.Form["patron-name"]);
                newPatron.Save();
                Book foundBook = Book.FindByTitle(Request.Query["title"]);
                List<Book> availableBooks = foundBook.BooksNotCheckedOut();
                if(availableBooks.Count > 0)
                {
                    Book checkoutBook = availableBooks[0];
                    List<Copy> bookCopies = Copy.AvailableCopiesOfBook(checkoutBook.GetId());
                    Copy copyToCheckOut = bookCopies[0];
                    Checkout newCheckout = new Checkout(newPatron.GetId(), copyToCheckOut.GetId(), Request.Form["checkout-date"]);
                    newCheckout.Save();
                    copyToCheckOut.SetCheckedOut();
                    model.Add("patron", newPatron);
                    model.Add("book", foundBook);
                    model.Add("checkout", newCheckout);
                    return View["checkedout.cshtml", newCheckout];
                }

                return View["didnt_work.cshtml"];
            };
        }
    }
}
