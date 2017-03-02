using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace LibraryCatalog
{
    public class Patron
    {
        private int _id;
        private string _name;

        public Patron(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }

        public override bool Equals(System.Object otherPatron)
        {
            if(!(otherPatron is Patron))
            {
                return false;
            }
            else
            {
                Patron newPatron = (Patron) otherPatron;
                bool idEquality = this.GetId() == newPatron.GetId();
                bool nameEquality = this.GetName() == newPatron.GetName();
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

            SqlCommand cmd = new SqlCommand("INSERT INTO patrons (name) OUTPUT INSERTED.id VALUES (@Name);", conn);

            cmd.Parameters.Add(new SqlParameter("@Name", this.GetName()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }

            DB.CloseSqlConnection(rdr, conn);
        }


         public static Patron FindPatronByName(string name)
         {
             SqlConnection conn = DB.Connection();
             conn.Open();

             SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE name = @PatronName;", conn);

             cmd.Parameters.Add(new SqlParameter("@PatronName", name));

             SqlDataReader rdr = cmd.ExecuteReader();

             int patronId = 0;
             string patronName = null;

             while(rdr.Read())
             {
                 patronId = rdr.GetInt32(0);
                 patronName = rdr.GetString(1);
             }

             Patron newPatron = new Patron(patronName, patronId);

             DB.CloseSqlConnection(rdr, conn);

             return newPatron;
         }

         public static List<Patron> GetAll()
         {
             List<Patron> allPatrons = new List<Patron>{};

             SqlConnection conn = DB.Connection();
             conn.Open();

             SqlCommand cmd = new SqlCommand("SELECT * FROM patrons", conn);

             SqlDataReader rdr = cmd.ExecuteReader();

             while(rdr.Read())
             {
                 int patronId = rdr.GetInt32(0);
                 string patronName = rdr.GetString(1);
                 Patron newPatron = new Patron(patronName, patronId);
                 allPatrons.Add(newPatron);
             }

             DB.CloseSqlConnection(rdr, conn);

             return allPatrons;
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

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE patrons SET name = @Name WHERE id = @PatronId;", conn);

            cmd.Parameters.Add(new SqlParameter("@Name", name));
            cmd.Parameters.Add(new SqlParameter("@PatronId", this.GetId()));

            cmd.ExecuteNonQuery();

            if(conn != null)
            {
                conn.Close();
            }
        }

        public static void DeleteAll()
        {
            DB.TableDeleteAll("patrons");
        }
    }
}
