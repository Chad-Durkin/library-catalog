using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace LibraryCatalog
{
    public class Copy
    {
        private int _id;
        private int _booksId;
        private int _checkedOut;

        public Copy(int booksId, int checkedOut = 0, int id = 0)
        {
            _booksId = booksId;
            _checkedOut = checkedOut;
            _id = id;
        }

        public override bool Equals(System.Object otherCopy)
        {
            if(!(otherCopy is Copy))
            {
                return false;
            }
            else
            {
                Copy newCopy = (Copy) otherCopy;
                bool idEquality = this.GetId() == newCopy.GetId();
                bool booksIdEquality = this.GetBooksId() == newCopy.GetBooksId();
                bool checkedOutEquality = this.GetCheckedOut() == newCopy.GetCheckedOut();
                return(idEquality && booksIdEquality && checkedOutEquality);
            }
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO copies (books_id, checkedout) OUTPUT INSERTED.id VALUES (@BooksId, @Checkedout);", conn);

            cmd.Parameters.Add(new SqlParameter("@BooksId", this.GetBooksId()));
            cmd.Parameters.Add(new SqlParameter("@Checkedout", this.GetCheckedOut()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }

            DB.CloseSqlConnection(rdr, conn);
        }

        public static List<Copy> CopiesOfBook(int bookId)
        {
            List<Copy> allCopies = new List<Copy>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM copies WHERE books_id = @BooksId;", conn);

            cmd.Parameters.Add(new SqlParameter("@BooksId", bookId));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int copyId = rdr.GetInt32(0);
                int copyBooksId = rdr.GetInt32(1);
                int copyCheckedOut = rdr.GetByte(2);
                Copy newCopy = new Copy(copyBooksId, copyCheckedOut, copyId);
                allCopies.Add(newCopy);
            }

            DB.CloseSqlConnection(rdr, conn);

            return allCopies;
        }

        // public static List<Copy> AvailableCopiesOfBook(int bookId)
        // {
        //     List<Copy> allCopies = new List<Copy>{};
        //
        //     SqlConnection conn = DB.Connection();
        //     conn.Open();
        //
        //     SqlCommand cmd = new SqlCommand("SELECT * FROM copies WHERE books_id = @BooksId AND checkedout != @Checkedout;", conn);
        //
        //     cmd.Parameters.Add(new SqlParameter("@BooksId", bookId));
        //     cmd.Parameters.Add(new SqlParameter("@Checkedout", 1));
        //
        //     SqlDataReader rdr = cmd.ExecuteReader();
        //
        //     while(rdr.Read())
        //     {
        //         int copyId = rdr.GetInt32(0);
        //         int copyBooksId = rdr.GetInt32(1);
        //         int copyCheckedOut = rdr.GetByte(2);
        //         Copy newCopy = new Copy(copyBooksId, copyCheckedOut, copyId);
        //         allCopies.Add(newCopy);
        //     }
        //
        //     DB.CloseSqlConnection(rdr, conn);
        //
        //     return allCopies;
        // }

        public static List<Copy> GetAll()
        {
            List<Copy> allCopies = new List<Copy>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM copies", conn);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int copyId = rdr.GetInt32(0);
                int copyBooksId = rdr.GetInt32(1);
                int copyCheckedOut = rdr.GetByte(2);
                Copy newCopy = new Copy(copyBooksId, copyCheckedOut, copyId);
                allCopies.Add(newCopy);
            }

            DB.CloseSqlConnection(rdr, conn);

            return allCopies;
        }

        public int GetId()
        {
            return _id;
        }

        public void SetId(int id)
        {
            _id = id;
        }

        public int GetBooksId()
        {
            return _booksId;
        }

        public void SetBooksId(int booksId)
        {
            _booksId = booksId;
        }

        public int GetCheckedOut()
        {
            return _checkedOut;
        }

        public void SetCheckedOut()
        {
            _checkedOut = 1;

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE copies SET checkedout = @Checkedout WHERE id = @CopyId;", conn);

            cmd.Parameters.Add(new SqlParameter("@Checkedout", "1"));
            cmd.Parameters.Add(new SqlParameter("@CopyId", this.GetId()));

            cmd.ExecuteNonQuery();

            if(conn != null)
            {
                conn.Close();
            }
        }

        public static void DeleteAll()
        {
            DB.TableDeleteAll("copies");
        }
    }
}
