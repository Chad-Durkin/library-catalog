using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace LibraryCatalog
{
    public class Author
    {
        private int _id;
        private string _name;

        public Author(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }

        public override bool Equals(System.Object otherAuthor)
        {
            if(!(otherAuthor is Author))
            {
                return false;
            }
            else
            {
                Author newAuthor = (Author) otherAuthor;
                bool idEquality = this.GetId() == newAuthor.GetId();
                bool nameEquality = this.GetName() == newAuthor.GetName();
                return(idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }

         public void Save()
         {
             SqlConnection conn = DB.Connection();
             conn.Open();

             SqlCommand cmd = new SqlCommand("INSERT INTO authors (name) OUTPUT INSERTED.id VALUES (@Name);", conn);

             cmd.Parameters.Add(new SqlParameter("@Name", this.GetName()));

             SqlDataReader rdr = cmd.ExecuteReader();

             while(rdr.Read())
             {
                 this._id = rdr.GetInt32(0);
             }

             DB.CloseSqlConnection(rdr, conn);
         }

         public static List<Author> GetAll()
         {
             List<Author> allAuthors = new List<Author>{};

             SqlConnection conn = DB.Connection();
             conn.Open();

             SqlCommand cmd = new SqlCommand("SELECT * FROM authors", conn);
             
             SqlDataReader rdr = cmd.ExecuteReader();

             while(rdr.Read())
             {
                 int authorId = rdr.GetInt32(0);
                 string authorName = rdr.GetString(1);
                 Author newAuthor = new Author(authorName, authorId);
                 allAuthors.Add(newAuthor);
             }

             DB.CloseSqlConnection(rdr, conn);

             return allAuthors;
         }

         public List<Book> GetBooks()
         {
             List<Book> allBooks = new List<Book>{};

             SqlConnection conn = DB.Connection();
             conn.Open();

             SqlCommand cmd = new SqlCommand("SELECT books.* FROM authors JOIN books_authors ON (authors.id = books_authors.authors_id) JOIN books ON (books.id = books_authors.books_id) WHERE authorss.id = @AuthorsId;", conn);

             cmd.Parameters.Add(new SqlParameter("@AuthorsId", this.GetId().ToString()));

             SqlDataReader rdr = cmd.ExecuteReader();

             while(rdr.Read())
             {
                 int bookId = rdr.GetInt32(0);
                 string bookName = rdr.GetString(1);
                 string bookPublicationDate = rdr.GetDateTime(2).ToString("yyyy-MM-dd");
                 int bookCopyCount = rdr.GetInt32(3);
                 int bookAuthorCount = rdr.GetInt32(4);
                 Book newBook = new Book(bookName, bookPublicationDate, bookCopyCount, bookAuthorCount, bookId);
                 allBooks.Add(newBook);
             }

             DB.CloseSqlConnection(rdr, conn);

             return allBooks;
         }

        public int GetId()
        {
            return _id;
        }

        public void SetId(int id)
        {
            _id = id;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public static void DeleteAll()
        {
            DB.TableDeleteAll("authors");
        }
    }
}
