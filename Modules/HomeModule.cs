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
        }
    }
}
